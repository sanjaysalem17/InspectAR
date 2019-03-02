using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
public class ContinuousDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;
    private float ClearTime;
    private String s;
	// Disable Screen Rotation on that screen
	void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
	}

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sh-cloud-test.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            RestartTime = Time.realtimeSinceStartup;
            ClearTime = Time.realtimeSinceStartup;
        };
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{
        BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();
			RestartTime += Time.realtimeSinceStartup + 1f;
            ClearTime = Time.realtimeSinceStartup + 1.2f;
            Debug.Log("60");
            FirebaseDatabase.DefaultInstance
            .GetReference(barCodeValue)
            .GetValueAsync().ContinueWith(task =>
            {
                Debug.Log("65");
                if (task.IsFaulted)
                {
                    // Handle the error...
             
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("73");
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("75");
                    if (snapshot.Value != null)
                    {
                        String result = "";
                        foreach (var item in snapshot.Children)
                        {
                            result += item.Key.ToString() + ": " + item.Value.ToString() + ", ";
                        }
                        s = result;
 
                    }
                    else
                    {

                        s = "Nonexistent Key";

                    }
                }
            });

			#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
			#endif
		});
	}

	/// <summary>
	/// The Update method from unity need to be propagated
	/// </summary>
	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			StartScanner();
			RestartTime = 0;
		}
        TextHeader.text = s;
        if ( ClearTime < Time.realtimeSinceStartup)
        {
            s = "";
        }
	}

	#region UI Buttons

	public void ClickBack()
	{
		// Try to stop the camera before loading another scene
		StartCoroutine(StopCamera(() => {
			SceneManager.LoadScene("Boot");
		}));
	}

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();
	}

	#endregion
}
