using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


// 상담사 목록 데이터를 전문분야에 따라 나눠 불러오는 클래스.
public class CounselorListMng : MonoBehaviour
{

    public List<string> relationshipUid = new List<string>();
    public List<string> relationshipUsername = new List<string>();
    public List<string> relationshipIntro = new List<string>();

    //public List<bool> relationshipMajor = new List<bool>();

    public List<string> relationshipPic = new List<string>();
    public List<string> relationshipCareer1 = new List<string>();
    public List<string> relationshipCareer2 = new List<string>();
    public List<string> relationshipCareer3 = new List<string>();


    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        relationshipUid.Clear();
        relationshipPic.Clear();


        RelationshipCounselorData();



    }


    public void RelationshipCounselorData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child("대인관계")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    print("대인관계 상담사 불러오기 실패...");
                }
                // 성공적으로 데이터를 가져왔으면
                if (task.IsCompleted)
                {
                    // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                    DataSnapshot snapshot = task.Result;
                    print($"대인관계 상담사 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력

                    //반복 시작.
                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        // JSON은 사전 형태이기 때문에 딕셔너리 형으로 변환.
                        IDictionary relationship = (IDictionary)data.Value;

                        /* Debug.Log("상담사: " + relationship["userGroup"]
                               + "\n uid: " + relationship["uid"]
                               + "\n email: " + relationship["email"]
                               + "\n pic: " + relationship["pic"]
                               + "\n username: " + relationship["username"]
                               + "\n sex: " + relationship["sex"]
                               + "\n intro: " + relationship["intro"]
                               + "\n family: " + relationship["family"]
                               + "\n myself: " + relationship["myself"]
                               + "\n relationship: " + relationship["relationship"]
                               + "\n romance: " + relationship["romance"]
                               + "\n work: " + relationship["work"]
                               + "\n career: " + relationship["career"]
                               + "\n career1: " + relationship["career1"]
                               + "\n career2: " + relationship["career2"]
                               + "\n career3: " + relationship["career3"]
                               + "\n appointment: " + relationship["appointment"]
                               );*/


                        relationshipUid.Add((string)relationship["uid"]);
                        relationshipUsername.Add((string)relationship["username"]);
                        relationshipIntro.Add((string)relationship["intro"]);

                        //relationshipMajor.Add((bool)relationship["relationship"]);

                        relationshipPic.Add((string)relationship["pic"]);
                        relationshipCareer1.Add((string)relationship["career1"]);
                        relationshipCareer2.Add((string)relationship["career2"]);
                        relationshipCareer3.Add((string)relationship["career3"]);


                    }

                    print("대인관계 리스트 완료");

                    for (int i = 0; i < relationshipUsername.Count; i++)
                    {
                        print("대인관계 저장 완료/ " + " i=" + i + "/ 상담사 이름 : " + relationshipUsername[i]);
                    }


                }

            });



    } // RelationshipCounselorData() end.



    // 상담사의 가능요일을 불러오는 함수.
    public void RelationshipDayData()
    {
        for (int i = 0; i < relationshipUid.Count; i++)
        {
            int temp = i;
            print(relationshipUid[i]);

            FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child("대인관계")
                .Child(relationshipUid[i]).Child("counselorDay").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        print("데이터베이스 읽기 실패...");
                    }

                    if (task.IsCompleted)
                    {
                        Debug.Log("가능요일 불러오기 성공");
                        DataSnapshot snapshot = task.Result;

                        string json = task.Result.GetRawJsonValue();

                        print(snapshot.ChildrenCount);
                        print("가능요일 : " + json);

                        Debug.Log("월요일: " + snapshot.Child("Monday").Value
                      + "\n화요일: " + snapshot.Child("Tuesday").Value
                      + "\n수요일: " + snapshot.Child("Wednesday").Value
                      + "\n목요일: " + snapshot.Child("Thursday").Value
                      + "\n금요일: " + snapshot.Child("Friday").Value
                      + "\n토요일: " + snapshot.Child("Saturday").Value
                      + "\n일요일: " + snapshot.Child("Sunday").Value);

                    }

                });

        }


    } // RelationshipDayData() end.





} // end.




// 상담사 레코드 하위에 위치한 가능예약 요일레코드(day)
class CounselorDay
{
    public bool Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday;

    public CounselorDay(bool Monday, bool Tuesday, bool Wednesday
        , bool Thursday, bool Friday, bool Saturday, bool Sunday)
    {
        this.Monday = Monday;
        this.Tuesday = Tuesday;
        this.Wednesday = Wednesday;
        this.Thursday = Thursday;
        this.Friday = Friday;
        this.Saturday = Saturday;
        this.Sunday = Sunday;

    }

}




// 상담사 레코드 하위에 위치한 가능예약 시간레코드(time)
class CounselorTime
{
    public string nine, ten, eleven, twelve, thirteen, fourteen
        , fifteen, sixteen, seventeen;

    public CounselorTime(string nine, string ten, string eleven, string twelve, string thirteen, string fourteen
        , string fifteen, string sixteen, string seventeen)
    {
        this.nine = nine;
        this.ten = ten;
        this.eleven = eleven;
        this.twelve = twelve;
        this.thirteen = thirteen;
        this.fourteen = fourteen;
        this.fifteen = fifteen;
        this.sixteen = sixteen;
        this.seventeen = seventeen;

    }

}