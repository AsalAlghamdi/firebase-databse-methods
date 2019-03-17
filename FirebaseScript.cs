using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseScript : MonoBehaviour
{

    DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("URL");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //_________________________
        
        saveData();
        saveDataAsJson();
        saveDataWithPuch();

        DatabaseReference deletedChild = reference.Child("Deleted Child");
        deletedChild.SetValueAsync("will be deleted");
        deleteData(deletedChild);

        retrieveData();
    }

    public string saveData()
    {
        reference.Child("users").Child("user1").Child("name").SetValueAsync("Asal Alghamdi"); 
        reference.Child("users").Child("user1").Child("city").SetValueAsync("Jeddah");

        reference.Child("users").Child("user2").Child("name").SetValueAsync("Sara");
        reference.Child("users").Child("user2").Child("city").SetValueAsync("Riyadh");
        
        return "Save data to firebase Done.";
    }
    
    private string saveDataAsJson()
    {
        User user = new User("Lama", "Jeddah");
        string json = JsonUtility.ToJson(user);

        // you can use it like this too
        // string json = "{\"name\":\"Lama\",\"city\":\"Jeddah\"}";

        reference.Child("users").Child("user3").SetRawJsonValueAsync(json);

        return "Save data to firebase Done.";
    }

    public string saveDataWithPuch()
    {
        //uniqueName takes the key that has genrated by Puch() method
        DatabaseReference uniqueName = reference.Child("users").Push();
        uniqueName.Child("name").SetValueAsync("Ali");
        uniqueName.Child("city").SetValueAsync("Makkah");

        return "Save data with Puch Done.";
    }

    private string deleteData(DatabaseReference referenceD)
    {
        referenceD.RemoveValueAsync();

        return "Delete data from firebase Done.";
    }

    private void retrieveData()
    {
        reference.Child("users").Child("user1").Child("name").GetValueAsync().ContinueWith(task => {
            //
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string print = snapshot.Value.ToString();
                Debug.Log(print);
            }
        });

        reference.Child("users").GetValueAsync().ContinueWith(task => {
            //
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string print = snapshot.GetRawJsonValue();
                Debug.Log(print);
            }
        });

        reference.Child("users").Child("user1").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot info in snapshot.Children)
                    {
                        Debug.Log(info.Key + " :" + info.Value);
                    }
                }
        });

        reference.Child("users").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    long print = snapshot.ChildrenCount;
                    Debug.Log("Number of users" + print);
                }
        });
    }

}

public class User
{
    public string name;
    public string city;

    public User()
    {
    }

    public User(string name, string city)
    {
        this.name = name;
        this.city = city;
    }
}