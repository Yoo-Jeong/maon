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

    public static string myName;

    // 내담자 상담예약 정보 관련 리스트.
    public static List<string> appointmentCounselorInCharge = new List<string>();
    public static List<string> appointmentAppDay = new List<string>();
    public static List<string> appointmentAppTime = new List<string>();
    public static List<string> appointmentFeedback = new List<string>();
    public List<string> appointmentWorry = new List<string>();

    int temp;

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

            // 내담자 기본정보
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
                            print($"유저 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력

                            //우측 상단 내담자 이름 표시.
                            displayname.text = snapshot.Child("username").Value.ToString();
                            myName = snapshot.Child("username").Value.ToString();



                            // 예약 여부 확인 true면 예약있음, false면 예약없음.
                            print(snapshot.Child("appointmentcheck").Value);
                            // 예약이 있으면 예약일정 화면 표시
                            if ((bool)snapshot.Child("appointmentcheck").Value == true)
                            {

                                Yapp.SetActive(true);
                                Napp.SetActive(false);

                                LoadMyAppoData();

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



    // RDB에서 예약정보를 읽어오는 함수.
    public void LoadMyAppoData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.User.UserId)
      .Child("appointment").GetValueAsync().ContinueWithOnMainThread(task =>
      {
          if (task.IsFaulted)
          {
              // Handle the error...
              print("예약정보 데이터베이스 읽기 실패...");
          }

          if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;

              //appoCount = (int)snapshot.ChildrenCount;


              foreach (DataSnapshot data in snapshot.Children)
              {

                  IDictionary appo = (IDictionary)data.Value;

                  counselorName.text = ((string)appo["counselorName"]);
                  counselorName2.text = ((string)appo["counselorName"]);

                  counselDay.text = ((string)appo["appDay1"]);
                  counselDay2.text = ((string)appo["appDay1"]);

                  counselTime.text = ((string)appo["appTime"]);
                  counselTime2.text = ((string)appo["appTime"]);

                  concern.text = ((string)appo["worry"]);

             
                  Debug.Log("예약날짜: " + (string)appo["appDay1"]);


              }

              //LoadRequestList();


              Debug.Log("예약정보 추가 완료.");
          }

      });


    }




}



