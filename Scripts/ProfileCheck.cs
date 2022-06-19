using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class ProfileCheck : MonoBehaviour
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    public string otherID;
    public Text LogText;

    public GameObject bg;
    bool open = true;

    public Text ClientDisplyName;
    public string todayClientName;
    public string todayClientUid;

    public void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        todayClientUid = DataMngForCounselor.todayCounselData[0];
        todayClientName = DataMngForCounselor.todayCounselData[1];

        //좌측 상단 내담자 이름 표시.
        ClientDisplyName.text = "[" + todayClientName + "]";

        GetProfileData();

    }

    public void ClickGetProfile()
    {
        SetProfile();

        if (open)
        {
            open = false;
            bg.SetActive(true);

        }
        else
        {
            open = true;
            bg.SetActive(false);

        }


    }



    public string[] todayClientProfile; //오늘 상담의 내담자 정보를 저장할 배열.
    public void GetProfileData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(todayClientUid)
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
                         print($"내담자 정보 레코드 개수 : {snapshot.ChildrenCount}"); //데이터 건수 출력
                    
                         Debug.Log("내담자: " + snapshot.Child("userGroup").Value
                               + "\n uid: " + snapshot.Child("uid").Value
                               + "\n email: " + snapshot.Child("email").Value
                               + "\n username: " + snapshot.Child("username").Value
                               + "\n sex: " + snapshot.Child("sex").Value
                               + "\n birth: " + snapshot.Child("birth").Value
                               + "\n job: " + snapshot.Child("job").Value
                               + "\n meal: " + snapshot.Child("meal").Value
                               + "\n sleep: " + snapshot.Child("sleep").Value
                               + "\n exercise: " + snapshot.Child("exercise").Value
                               + "\n emotionCard: " + snapshot.Child("emotionCard").Value
                               );


                         todayClientProfile = new string[] { (string)snapshot.Child("username").Value
                             , (string)snapshot.Child("sex").Value
                             , (string)snapshot.Child("birth").Value
                             , (string)snapshot.Child("job").Value

                             , (string)snapshot.Child("meal").Value
                             , (string)snapshot.Child("sleep").Value
                             , (string)snapshot.Child("exercise").Value
                         };


                         print("내담자 기본정보 불러오기 완료");
                     }

                 });
    }


    public void SetProfile()
    {
    
        LogText.text = (
           "<b>이름</b>                  " + todayClientProfile[0] + "\n" 
         + "<b>성별</b>                  " + todayClientProfile[1] + "\n"
         + "<b>생년월일</b>          " + todayClientProfile[2]+ "\n"
         + "<b>직업</b>                  " + todayClientProfile[3] + "\n"

         + "<b>생활 패턴</b>" + "\n"
         + "  * 식사                       " + todayClientProfile[4] + "\n"
         + "  * 수면                       " + todayClientProfile[5] + "\n"
         + "  * 운동                       " + todayClientProfile[6] + "\n");
    }



}

