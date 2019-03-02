using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using System;
using TMPro;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{
    public string hash;
    private bool inuse;
    public GameObject header;
    public GameObject info;
    private String s;
    private String title = "";
    private String infot = "";
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sh-cloud-test.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Init");

    }

    // Update is called once per frame
    void Update()
    {
        FirebaseDatabase.DefaultInstance
  .GetReference(hash)
  .GetValueAsync().ContinueWith(task =>
  {
      inuse = true;
      if (task.IsFaulted)
      {
          Debug.Log("Error");
          // Handle the error...
      }
      else if (task.IsCompleted)
      {
          DataSnapshot snapshot = task.Result;

          foreach (var item in snapshot.Children)
          {
              if ( i > 2)
              {
                  break;
              }
              if (item.Key.ToString() == "name")
              {

                  title = item.Value.ToString();
              }
              else
              {
                  infot += item.Key.ToString() + ": " + item.Value.ToString() + "\n";
              }
              i++;
          }
          inuse = false;
      }
  });
        if (!inuse)
        {
            header.GetComponent<TextMeshProUGUI>().text = (title);
            info.GetComponent<TextMeshProUGUI>().SetText(infot);
        }
    }
}
