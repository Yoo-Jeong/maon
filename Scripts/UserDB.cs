using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase.Auth;


public class UserDB : MonoBehaviour
{

    static FirebaseFirestore db;

    //public InputField nicknameField;
    //public static string nicknameText;

    void Update()
    {
        //nicknameText = nicknameField.text;
    }

    public void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        Debug.Log("시작");
    }

    public void ReadData()
    {
        CollectionReference usersRef = db.Collection("counselor");
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log(String.Format("User: {0}", document.Id));
                Dictionary<string, object> documentDictionary = document.ToDictionary();

                Debug.Log(String.Format("name: {0}", documentDictionary["name"] + " 상담사"));
                //NameText.text = documentDictionary["name"] + " 상담사";

                Debug.Log(String.Format("한줄소개: {0}", documentDictionary["introduction"]));
                //IntroText.text = documentDictionary["introduction"] + "";

                Debug.Log(String.Format("전문분야: {0}", documentDictionary["major"]));

                Debug.Log(String.Format("경력: {0}", documentDictionary["career"]));
                //careerText.text = documentDictionary["career"] + "";
            }

            Debug.Log("상담사 정보 읽기 완료");
        });
    }
}