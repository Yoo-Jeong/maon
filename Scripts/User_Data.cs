using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class User_Data : MonoBehaviour
{

    //내담자 유저의 기본정보를 저장할 string타입 변수들
    public static string userGroup, uid, email, username, sex, birth, job, meal, sleep, exercise, emotionCard;
    public static bool appointmentcheck;                                //내담자 유저의 예약여부 판단을 저장할 bool타입 변수

    public Text displayname;
    public Text counselorName, counselDay, counselTime, concern;
    public Text counselorName2, counselDay2, counselTime2;

    public GameObject Yapp, Napp;

    public string todayYear;
    public string todayMonth;
    public string todayDay;
    public string todayString;

    public GameObject counselStart;

    // 내담자 상담예약 정보 관련 리스트.
    public List<string> MyAppoCounselorUid = new List<string>();
    public List<string> MyAppoClientname = new List<string>();
    public static List<string> MyAppoRequestDay = new List<string>();
    public static List<string> MyAppoAppDay1 = new List<string>();
    public static List<string> MyAppoAppDay2 = new List<string>();
    public List<string> MyAppoAppTime = new List<string>();
    public List<string> MyAppoWorry = new List<string>();
    public List<long> MyAppoProgress = new List<long>();
    public static List<string> MyAppoRefuse = new List<string>();
    public static List<string> MyAppoFeedback = new List<string>();


    int temp;


    public Button logoutBtn;

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    DateTime today, appoday;
    

    public void Awake()
    {
        Yapp.SetActive(false);
        Napp.SetActive(true);

        ClearLists();

        todayYear = DateTime.Now.ToString("yyyy");
        todayMonth = DateTime.Now.ToString("MM");
        todayDay = DateTime.Now.ToString("dd");
        todayString = todayYear + "." + todayMonth + "." + todayDay + ".";
    }

    public void Start()
    {
        
        LoadMyInfo();

        //이벤트리스너 연결
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId);
        userRef.ChildChanged += HandleChildChanged;                         //내담자 유저 하위에 있는 내용에 대한 변경을 읽고 수신 대기
        userRef.Child("appointment").ChildAdded += HandleChildAdded;    //내담자 유저의 하위에 있는 appointment에 대한 목록 추가를 읽고 수신 대기
        
        

        logoutBtn.onClick.AddListener(Auth_Manager.Instance.Logout); //로그아웃 버튼

    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log(" ChildChanged 이벤트핸들러 ");
        LoadMyInfo();  
    }


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log(" ChildAdded 이벤트핸들러 ");
     
    }

  


    public void LoadMyInfo()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        // 내담자 기본정보
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId)
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error...
                        Debug.Log("데이터베이스 읽기 실패...");
                    }

                    // 성공적으로 데이터를 가져왔으면
                    if (task.IsCompleted)
                    {
                        // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                        DataSnapshot snapshot = task.Result;
                        Debug.Log($"내담자 유저 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                        Debug.Log("유저그룹 : " + snapshot.Child("userGroup").Value
                                + "\nuid : " + snapshot.Child("uid").Value
                                + "\n이메일 : " + snapshot.Child("email").Value
                                + "\n이름 : " + snapshot.Child("username").Value
                                + "\n성별 : " + snapshot.Child("sex").Value
                                + "\n생년월일 : " + snapshot.Child("birth").Value
                                + "\n직업 : " + snapshot.Child("job").Value
                                + "\n식사 횟수 : " + snapshot.Child("meal").Value
                                + "\n수면시간 : " + snapshot.Child("sleep").Value
                                + "\n운동 횟수 : " + snapshot.Child("exercise").Value
                                + "\n예약 여부 판단: " + snapshot.Child("appointmentcheck").Value
                                + "\n감정카드 : " + snapshot.Child("emotionCard").Value
                               );


                        userGroup = (string)snapshot.Child("userGroup").Value;
                        uid = (string)snapshot.Child("uid").Value;
                        email = (string)snapshot.Child("email").Value;
                        username = (string)snapshot.Child("username").Value;
                        sex = (string)snapshot.Child("sex").Value;
                        birth = (string)snapshot.Child("birth").Value;
                        job = (string)snapshot.Child("job").Value;
                        meal = (string)snapshot.Child("meal").Value;
                        sleep = (string)snapshot.Child("sleep").Value;
                        exercise = (string)snapshot.Child("exercise").Value;
                        appointmentcheck = (bool)snapshot.Child("appointmentcheck").Value;
                        emotionCard = (string)snapshot.Child("emotionCard").Value;


                        SettingHomeUI();
                    }
                    else
                    {
                        Debug.Log("로그인이 필요합니다.");
                    }



                });

    }

    // 내담자 홈 화면 ui
    public void SettingHomeUI()
    {
        displayname.text = username;

        // 예약이 있으면 예약일정 화면 표시
        if (appointmentcheck == true)
        {
            Debug.Log("예약 일정이 있습니다.");
            LoadMyAppoData();
        }
        else // 예약이 없으면 예약없는 화면 표시
        {
            Yapp.SetActive(false);
            Napp.SetActive(true);
        }
    }


    // RDB에서 내담자 유저의 예약정보를 읽어오는 함수.
    public void LoadMyAppoData()
    {
        ClearLists();

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId)
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


                  MyAppoCounselorUid.Add((string)appo["counselorUid"]);
                  MyAppoClientname.Add((string)appo["clientName"]);
                  MyAppoRequestDay.Add((string)appo["requestDay"]);

                  MyAppoAppDay1.Add((string)appo["appDay1"]);
                  MyAppoAppDay2.Add((string)appo["appDay2"]);
                  MyAppoAppTime.Add((string)appo["appTime"]);

                  MyAppoWorry.Add((string)appo["worry"]);
                  MyAppoProgress.Add((long)appo["progress"]);
                  MyAppoRefuse.Add((string)appo["refuse"]);
                  MyAppoFeedback.Add((string)appo["feedback"]);


              }


              for (int i = 0; i <= MyAppoAppDay1.Count; i++)
              {
                  //오늘날짜와 상담날짜 비교
                  today = DateTime.Now.Date;
                  appoday = Convert.ToDateTime(MyAppoAppDay1[i]).Date;

                  int result = DateTime.Compare(today, appoday);

                  if (result == -1) //오늘 날짜보다 상담날짜가 더 빠르다면
                  {
                      Debug.Log("예정된 상담이 있습니다. (오늘은 아님)" + MyAppoAppDay1[i]);

                      Yapp.SetActive(true);
                      Napp.SetActive(false);

                  }
                  else if (result == 0) //오늘 날짜보다 상담날짜가 같다면
                  {
                      Debug.Log("오늘 예정된 상담이 있습니다." + MyAppoAppDay1[i]);

                      Yapp.SetActive(true);
                      Napp.SetActive(false);
                  }
                  else
                  {
                      Debug.Log("예정된 상담이 없습니다." + MyAppoAppDay1[i]);

                      //Yapp.SetActive(false);
                      //Napp.SetActive(true);

                  }
              }

              

              Debug.Log("예약정보 추가 완료.");
          }

      });


    }

    //리스트들 초기화
    public void ClearLists()
    {
        MyAppoCounselorUid.Clear();
        MyAppoClientname.Clear();
        MyAppoRequestDay.Clear();
        MyAppoAppDay1.Clear();
        MyAppoAppDay2.Clear();
        MyAppoAppTime.Clear();
        MyAppoWorry.Clear();
        MyAppoProgress.Clear();
        MyAppoRefuse.Clear();
        MyAppoFeedback.Clear();
    }


}



