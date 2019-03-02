using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class CloudTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sh-cloud-test.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
  .GetReference("Brian_Mac")
  .GetValueAsync().ContinueWith(task => {
      if (task.IsFaulted)
      {
              // Handle the error...
          }
      else if (task.IsCompleted)
      {
          DataSnapshot snapshot = task.Result;
              // Do something with snapshot...
              Debug.Log(snapshot);
          }
  });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
