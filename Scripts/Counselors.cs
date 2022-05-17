using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


/*파이어베이스RDB에 저장되어있는 상담사 정보 클래스.
(파이어베이스RDB에서의 CounselorUsers하위에 위치한 데이터들을 의미한다.)*/
public class Counselors
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    public string major;  //상담사 전문분야

    public List<string> counselorsUid = new List<string>();        //상담사 Uid를 저장할 리스트
    public List<string> counselorsUsername = new List<string>();   //상담사 이름을 저장할 리스트
    public List<string> counselorsIntro = new List<string>();      //상담사 한줄소개를 저장할 리스트

    //public List<bool> counselorsMajor = new List<bool>();          //상담사 전문분야를 저장할 리스트

    public List<string> counselorsPic = new List<string>();        //상담사 프로필사진 경로를 저장할 리스트
    public List<string> counselorsCareer1 = new List<string>();    //상담사 경력사항1을 저장할 리스트
    public List<string> counselorsCareer2 = new List<string>();    //상담사 경력사항2를 저장할 리스트
    public List<string> counselorsCareer3 = new List<string>();    //상담사 경력사항3을 저장할 리스트
    public List<string> counselorsSex = new List<string>();        //상담사 성별을 저장할 리스트


    public List<bool> Monday = new List<bool>();               //상담사 가능요일 중 월요일을 저장할 리스트
    public List<bool> Tuesday = new List<bool>();              //상담사 가능요일 중 화요일을 저장할 리스트
    public List<bool> Wednesday = new List<bool>();            //상담사 가능요일 중 수요일을 저장할 리스트
    public List<bool> Thursday = new List<bool>();             //상담사 가능요일 중 목요일을 저장할 리스트
    public List<bool> Friday = new List<bool>();               //상담사 가능요일 중 금요일을 저장할 리스트
    public List<bool> Saturday = new List<bool>();             //상담사 가능요일 중 토요일을 저장할 리스트
    public List<bool> Sunday = new List<bool>();               //상담사 가능요일 중 일요일을 저장할 리스트

    public string[] CounselorTime = new string[9];             //상담사 가능시간을 저장할 배열



    // 상담사의 전문분야를 의미하는 string타입 매개변수 major를 받아와서 객체를 생성한다.
    // major는 RDB에서 상담사 정보를 불러올때 필요한 경로지정을 위한것.
    public Counselors(string major)
    {
        this.major = major;      //이 major는 매개변수로 전달받은 string타입 major
        Loadcounselors(major);   


    } //생성자 끝.



    // 상담사의 전문분야를 의미하는 string타입 매개변수 major를 받아와서 상담사의 기본정보를 파이어베이스RDB에서 불러오는 함수.
    public void Loadcounselors(string major)
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // 데이터 한번 읽기 시작.
        FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(major + "상담사 불러오기 실패...");
                }

                // 성공적으로 데이터를 가져왔으면
                if (task.IsCompleted)
                {
                    // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(major + $"상담사 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); 

                    //반복 시작.
                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        // JSON은 사전 형태이기 때문에 딕셔너리 형으로 변환.
                        IDictionary counselor = (IDictionary)data.Value;

                        counselorsUid.Add((string)counselor["uid"]);
                        counselorsUsername.Add((string)counselor["username"]);
                        counselorsIntro.Add((string)counselor["intro"]);

                        //counselorsMajor.Add((bool)counselor["relationship"]);

                        counselorsPic.Add((string)counselor["pic"]);
                        counselorsCareer1.Add((string)counselor["career1"]);
                        counselorsCareer2.Add((string)counselor["career2"]);
                        counselorsCareer3.Add((string)counselor["career3"]);

                        counselorsSex.Add((string)counselor["sex"]);

                    }

                    LoadCounselorDay(major);

                    //콘솔창 확인용 반복문
                    for (int i = 0; i < counselorsUsername.Count; i++)
                    {
                        Debug.Log(major + "상담사 기본정보 리스트 저장 완료/ " + " i=" + i + "/ 상담사 이름 : " + counselorsUsername[i]);
                    }

                }

            });

    }//Loadcounselors(string major) end.




    // 상담사의 전문분야를 의미하는 string타입 매개변수 major를 받아와서 상담사의 가능요일을 파이어베이스RDB에서 불러오는 함수.
    public void LoadCounselorDay(string major)
    {
        for (int i = 0; i < counselorsUsername.Count; i++)
        {
            int temp = i;
            Debug.Log("가능 요일: " + counselorsUsername[i]);

            // 데이터 한번 읽기 시작.
            FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major)
                .Child(counselorsUid[i]).Child("counselorDay").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("가능 요일 데이터베이스 읽기 실패...");
                    }

                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        string DayJson = task.Result.GetRawJsonValue();

                        Debug.Log("가능요일 snapshot.ChildrenCount: " + snapshot.ChildrenCount);

                        Debug.Log(
                            "월요일: " + snapshot.Child("Monday").Value
                            + "\n화요일: " + snapshot.Child("Tuesday").Value                
                            + "\n수요일: " + snapshot.Child("Wednesday").Value                 
                            + "\n목요일: " + snapshot.Child("Thursday").Value                
                            + "\n금요일: " + snapshot.Child("Friday").Value                
                            + "\n토요일: " + snapshot.Child("Saturday").Value                
                            + "\n일요일: " + snapshot.Child("Sunday").Value);


                        Monday.Add((bool)snapshot.Child("Monday").Value);
                        Tuesday.Add((bool)snapshot.Child("Tuesday").Value);
                        Wednesday.Add((bool)snapshot.Child("Wednesday").Value);
                        Thursday.Add((bool)snapshot.Child("Thursday").Value);
                        Friday.Add((bool)snapshot.Child("Friday").Value);
                        Saturday.Add((bool)snapshot.Child("Saturday").Value);
                        Sunday.Add((bool)snapshot.Child("Sunday").Value);


                        Debug.Log("가능요일 저장 완료");
                    }

                });
        }

    } // LoadCounselorDay(string major) end.



    // 상담사의 전문분야를 의미하는 string타입 매개변수 major와 상담사uid를 받아와서 상담사의 가능시간을 파이어베이스RDB에서 불러오는 함수.
    public void LoadCounselorTime(string major, string uid)
    {
            // 데이터 한번 읽기 시작.
            FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major)
                .Child(uid).Child("counselorTime").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("가능 시간 데이터베이스 읽기 실패...");
                    }

                    if (task.IsCompleted)
                    {                        
                        DataSnapshot snapshot = task.Result;

                        string TimeJson = task.Result.GetRawJsonValue();

                        Debug.Log("가능시간 snapshot.ChildrenCount: " + snapshot.ChildrenCount);

                        Debug.Log("9:00-10:00: " + snapshot.Child("nine").Value
                      + "\n10:00-11:00: " + snapshot.Child("ten").Value
                      + "\n11:00-12:00: " + snapshot.Child("eleven").Value
                      + "\n12:00-13:00: " + snapshot.Child("twelve").Value
                      + "\n13:00-14:00: " + snapshot.Child("thirteen").Value
                      + "\n14:00-15:00: " + snapshot.Child("fourteen").Value
                      + "\n15:00-16:00: " + snapshot.Child("fifteen").Value
                      + "\n16:00-17:00: " + snapshot.Child("sixteen").Value
                      + "\n17:00-18:00: " + snapshot.Child("seventeen").Value
                      );

                        
                        CounselorTime[0] = (string)snapshot.Child("nine").Value;
                        CounselorTime[1] = (string)snapshot.Child("ten").Value;
                        CounselorTime[2] = (string)snapshot.Child("eleven").Value;
                        CounselorTime[3] = (string)snapshot.Child("twelve").Value;
                        CounselorTime[4] = (string)snapshot.Child("thirteen").Value;
                        CounselorTime[5] = (string)snapshot.Child("fourteen").Value;
                        CounselorTime[6] = (string)snapshot.Child("fifteen").Value;
                        CounselorTime[7] = (string)snapshot.Child("sixteen").Value;
                        CounselorTime[8] = (string)snapshot.Child("seventeen").Value;


                        Debug.Log("가능시간 저장 완료");

                    }

                });
       

    } // LoadCounselorTime(string major) end.



}// CounselorsData() end.
