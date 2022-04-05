using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;




public class User_Data : MonoBehaviour
{

    public Text displayname;
    public Text counselorName, counselDay, counselTime, concern;
    public Text counselorName2, counselDay2, counselTime2;

    public GameObject Yapp, Napp;


    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
       new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        Yapp.SetActive(false);
        Napp.SetActive(false);

        LoadUserData();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadUserData()
    {
        if (Auth_Manager.User != null)
        {
            FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.User.UserId)
                    .GetValueAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.IsFaulted)
                        {
                            // Handle the error...
                            print("데이터베이스 읽기 실패...");
                        }

                        // 성공적으로 데이터를 가져왔으면
                        if (task.IsCompleted)
                        {
                            // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                            DataSnapshot snapshot = task.Result;
                            print($"데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력

                            //우측 상단 내담자 이름 표시.
                            displayname.text = snapshot.Child("username").Value.ToString();



                            // 예약 여부 확인 true면 예약있음, false면 예약없음.
                            print(snapshot.Child("appointmentcheck").Value);    
                            // 예약이 있으면 예약일정 화면 표시
                            if ((bool)snapshot.Child("appointmentcheck").Value == true)
                            {
                                
                                Yapp.SetActive(true);
                                Napp.SetActive(false);

                                counselorName.text = snapshot.Child("counselorInCharge").Value.ToString();
                                counselorName2.text = snapshot.Child("counselorInCharge").Value.ToString();

                                counselDay.text = snapshot.Child("appDay").Value.ToString();
                                counselDay2.text = snapshot.Child("appDay").Value.ToString();

                                counselTime.text = snapshot.Child("appTime").Value.ToString();
                                counselTime2.text = snapshot.Child("appTime").Value.ToString();

                                concern.text = snapshot.Child("worry").Value.ToString();


                            }
                            else // 예약이 없으면 예약없는 화면 표시
                            {
                                Yapp.SetActive(false);
                                Napp.SetActive(true);
                            }

                        }
                        else
                        {
                            Debug.Log("로그인이 필요합니다.");
                        }


                    });

        }
    }
}
