using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FeedbackItem : MonoBehaviour
{
    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    public Image imageBox;
    public bool isClose;
    public InputField feedback;

    public GameObject openObj;

    RectTransform rect;

    public void Start()
    {


        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
      new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        isClose = true;


        openObj.SetActive(false);

    }

    public void ClickOpen()
    {

        if (isClose)
        {
            print("작성 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1454, rect.sizeDelta.y + 130);

            openObj.SetActive(true);


            isClose = false;
        }
        else
        {

            print("닫기 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1454, rect.sizeDelta.y - 130);

            openObj.SetActive(false);


            isClose = true;

        }

    }



    public void ClickSave()
    {
        print("저장 버튼 클릭");

        rect = (RectTransform)imageBox.transform;
        rect.sizeDelta = new Vector2(1454, rect.sizeDelta.y - 130);

        openObj.SetActive(false);


        isClose = true;


        // 예약하면 appointmentcheck true로 업데이트
        Dictionary<string, object> feedbackSave = new Dictionary<string, object>();
        feedbackSave["feedback"] = feedback.text;
        print(feedbackSave.Keys);

        reference.Child("ClientUsers").Child(Auth_Manager.User.UserId).Child("appointment").Child("-N-SqsxFFxWCIOESEnRr").Child("-N-Twut--ofzHRFQ6XEF").UpdateChildrenAsync(feedbackSave);

    }



}
