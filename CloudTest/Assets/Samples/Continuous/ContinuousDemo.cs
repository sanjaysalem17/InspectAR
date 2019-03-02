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
using Wizcorp.Utils.Logger;
using System.Threading.Tasks;

public class ContinuousDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;
    private float ClearTime;
    private String s;
    private bool Scanning;
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
            Debug.Log("init");
        };
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{

        BarcodeScanner.Scan(async (barCodeType, barCodeValue) =>
        {
            BarcodeScanner.Stop();
            Scanning = true;
            RestartTime += Time.realtimeSinceStartup + 1f;
            ClearTime = Time.realtimeSinceStartup + 1.5f;
            Debug.Log("a");
            DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.GetReference(barCodeValue).GetValueAsync();
            if (snapshot.Value != null)
            {
                String result = "";
                foreach (var item in snapshot.Children)
                {
                    result += item.Key.ToString() + ": " + item.Value.ToString() + ", ";
                }
                s = result;
                Debug.Log(result);
            }
            else
            {
                Debug.Log("Nonexistent Key");
                s = "Nonexistent";
            }
            await new WaitForSeconds(1.0f);
            Scanning = false;
        });
	}

	/// <summary>
	/// The Update method from unity need to be propagated
	/// </summary>
	void FixedUpdate()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup && !Scanning)
		{
			StartScanner();
			RestartTime = 0;
		}
        if (!Scanning)
        {
            TextHeader.text = s;
        }
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
