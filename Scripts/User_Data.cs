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

    public Text displayname, displayname2;
    public Text counselorName, counselDay, counselTime, concern;
    public Text counselorName2, counselDay2, counselTime2;


    //상담 탭 텍스트들
    public Text counselorName3, counselDay3, counselTime3;
    //마이페이지 텍스트 배열
    public Text[] mypageInfo = new Text[8];

    public GameObject Yapp, Napp, Yapp_Counsel, Napp_Counsel;

    public string todayYear;
    public string todayMonth;
    public string todayDay;
    public string todayString, todayString2;

    public GameObject counselStart;

    // 내담자 상담예약 정보 관련 리스트.
    public List<string> MyAppoCounselorUid = new List<string>();
    public static List<string> MyAppoCounselorName = new List<string>();
    public static List<string> MyAppoCounselorSex = new List<string>();
    public static List<string> MyAppoRequestDay = new List<string>();
    public static List<string> MyAppoAppDay1 = new List<string>();
    public static List<string> MyAppoAppDay2 = new List<string>();
    public List<string> MyAppoAppTime = new List<string>();
    public List<string> MyAppoWorry = new List<string>();
    public List<long> MyAppoProgress = new List<long>();
    public static List<string> MyAppoRefuse = new List<string>();
    public static List<string> MyAppoFeedback = new List<string>();


    // 오늘 예약 정보를 담을 배열
    public static string[] todayCounsel = new string[10];


    int temp;


    public Button logoutBtn;

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    DateTime today, appoday;
    

    public void Awake()
    {
        ClearLists();


        Yapp.SetActive(false);
        Napp.SetActive(true);
        Yapp_Counsel.SetActive(false);
        Napp_Counsel.SetActive(false);

       

        todayYear = DateTime.Now.ToString("yyyy");
        todayMonth = DateTime.Now.ToString("MM");
        todayDay = DateTime.Now.ToString("dd");
        todayString = todayYear + "." + todayMonth + "." + todayDay + ".";
        todayString2 = todayYear + "년" + todayMonth + "월" + todayDay + "일";
    }

    public void Start()
    {
        
        //LoadMyInfo();

        //이벤트리스너 연결
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId);
        userRef.Child("appointment").ChildChanged += HandleChildChanged;                         //내담자 유저 하위에 있는 내용에 대한 변경을 읽고 수신 대기
        userRef.Child("appointment").ChildAdded += HandleChildAdded;    //내담자 유저의 하위에 있는 appointment에 대한 목록 추가를 읽고 수신 대기

        userRef.Child("appointment").Child(todayString2).ValueChanged += HandleValueChanged;

        userRef.Child("appointment").ChildRemoved += HandleChildRemoved;

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
        LoadMyAppoData();
    }


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log(" ChildAdded 이벤트핸들러 ");
        LoadMyAppoData();
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        Debug.Log(" ValueChanged 이벤트핸들러 ");

        LoadMyInfo();

    }

    void HandleChildRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Debug.Log(" ChildRemoved 이벤트핸들러 ");
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

                        mypageInfo[0].text = username;
                        mypageInfo[1].text = email;
                        mypageInfo[2].text = sex + "성";
                        mypageInfo[3].text = birth;
                        mypageInfo[4].text = job;
                        mypageInfo[5].text = meal;
                        mypageInfo[6].text = sleep;
                        mypageInfo[7].text = exercise;

                      

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
        displayname2.text = username;

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

                  MyAppoCounselorUid.Add((string)appo["counselorUid"]);
                  MyAppoCounselorName.Add((string)appo["counselorName"]);
                  MyAppoCounselorSex.Add((string)appo["counselorSex"]);

                  MyAppoRequestDay.Add((string)appo["requestDay"]);

                  MyAppoAppDay1.Add((string)appo["appDay1"]);
                  MyAppoAppDay2.Add((string)appo["appDay2"]);
                  MyAppoAppTime.Add((string)appo["appTime"]);

                  MyAppoWorry.Add((string)appo["worry"]);
                  MyAppoProgress.Add((long)appo["progress"]);
                  MyAppoRefuse.Add((string)appo["refuse"]);
                  MyAppoFeedback.Add((string)appo["feedback"]);

                  
                  todayCounsel[2] = ((string)appo["counselorSex"]);
                  Debug.Log("오늘 상담사 :" + todayCounsel[2]);
              }

              CreatRequestPrefabs();

              compareCounselDay();

              Debug.Log("예약정보 추가 완료.");
          }

      });

    }


    public void compareCounselDay()
    {
        Debug.Log("실행 시작: compareCounselDay()");

        for (int i = 0; i <= MyAppoAppDay1.Count; i++)
        {
            //오늘날짜와 상담날짜 비교
            today = DateTime.Now.Date;
            appoday = Convert.ToDateTime(MyAppoAppDay1[i]).Date;

            int result = DateTime.Compare(today, appoday);

            if (result == -1) //오늘 날짜보다 상담날짜가 더 빠르다면
            {
                Debug.Log("예정된 상담이 있습니다. (오늘은 아님)" + MyAppoAppDay1[i] + "/ 진행상황" + MyAppoProgress[i] );

            } 
            else if ( result == 1)
            {
                Debug.Log("예정된 상담이 없습니다." + MyAppoAppDay1[i] + "/ 진행상황" + MyAppoProgress[i]);

            }
            else
            {
                temp = i;
                Debug.Log("오늘 예정된 상담이 있습니다." + MyAppoAppDay1[i] + "/ 진행상황" + MyAppoProgress[i]);

                if (MyAppoProgress[temp] == 1)
                {
                    Debug.Log("/ 진행상황" + MyAppoProgress[temp]);
             
                    todayCounsel[0] = MyAppoCounselorUid[temp];
                    todayCounsel[1] = MyAppoCounselorName[temp];
                    //todayCounsel[2] = MyAppoCounselorSex[temp];
                    todayCounsel[3] = MyAppoRequestDay[temp];
                    todayCounsel[4] = MyAppoAppDay1[temp];
                    todayCounsel[5] = MyAppoAppDay2[temp];
                    todayCounsel[6] = MyAppoAppTime[temp];
                    todayCounsel[7] = MyAppoWorry[temp];
                    //todayCounsel[9] = MyAppoRefuse[temp];
                    //todayCounsel[10] = MyAppoFeedback[temp];

                    counselorName.text = todayCounsel[1];
                    counselorName2.text = todayCounsel[1];
                    counselorName3.text = todayCounsel[1];
                    counselDay.text = todayCounsel[4];
                    counselDay2.text = todayCounsel[4];
                    counselDay3.text = todayCounsel[4];
                    counselTime.text = todayCounsel[6];
                    counselTime2.text = todayCounsel[6];
                    counselTime3.text = todayCounsel[6];
                    concern.text = todayCounsel[7];

                    Yapp.SetActive(true);
                    Napp.SetActive(false);
                    Yapp_Counsel.SetActive(true);
                    Napp_Counsel.SetActive(false);
                }
                else
                {
                    Debug.Log("/ 진행상황2 " + MyAppoProgress[temp]);
                    Yapp.SetActive(false);
                    Napp.SetActive(true);
                    Yapp_Counsel.SetActive(false);
                    Napp_Counsel.SetActive(true);
                }
            }
        }

        Debug.Log("실행 완료: compareCounselDay()");

    }



    // 예약신청 진행상황(상담탭) 프리팹 관련 변수들 선언.
    public Transform requestParent;                             // 예약신청 프리팹이 생성될 위치의 부모객체의 위치.(parent의 자식으로 프리팹생성)
    public GameObject requestPrefab, requestClone;              // 예약신청 프리팹, 복제된 예약신청 프리팹

    public List<GameObject> requestCloneList = new List<GameObject>();    // 복제된 프리팹들을(counselorClone) 담을 리스트

    public Text[] newrequestCloneData;           //requestClone의 자식 Text타입 게임오브젝트를 담을 배열

    public void CreatRequestPrefabs() {

        Debug.Log("실행 시작 : CreatRequestPrefabs();");

        // CounselorList클래스의 대인관계전문 상담사이름의 수만큼 반복한다.
        for (int i = 0; i < MyAppoCounselorName.Count; i++)
        {
            requestClone = Instantiate(requestPrefab, requestParent);   // parent위치에 requestPrefab requestClone 생성
            requestCloneList.Add(requestClone);                  // 생성된 클론프리팹을 requestCloneList 추가
            requestClone.SetActive(true);

            // 텍스트 배열 newCounselorDataShort는 생성된 counselorClone프리팹의 Text타입인 자식 객체들
            newrequestCloneData = requestClone.GetComponentsInChildren<Text>();
            

            if (MyAppoProgress[i] == 0)
            {
                newrequestCloneData[0].text = "승인 중";   // newrequestCloneData[0]의 텍스트는 진행상황

            }else if (MyAppoProgress[i] == 1)
            {
                newrequestCloneData[0].text = "상담 확정";

            }else if (MyAppoProgress[i] == 2)
            {
                newrequestCloneData[0].text = "상담 거절";
            }
            else
            {
                newrequestCloneData[0].text = "상담 완료";
            }

            
            newrequestCloneData[1].text = MyAppoCounselorName[i];         // newrequestCloneData[1]의 텍스트는 상담사 이름
            newrequestCloneData[2].text = MyAppoRequestDay[i];            // newrequestCloneData[2]의 텍스트는 예약일자
            newrequestCloneData[3].text = MyAppoAppDay1[i];               // newrequestCloneData[3]의 텍스트는 상담일자
            newrequestCloneData[4].text = MyAppoAppTime[i];               // newrequestCloneData[4]의 텍스트는 상담시간
            

            //requestCloneList[i].transform.GetChild(7).GetComponentInChildren<Text>().text = MyAppoWorry[i];
           
        }
        Debug.Log("실행 완료 : CreatRequestPrefabs();");

    }




    //리스트들 초기화
    public void ClearLists()
    {

        Debug.Log("리스트 초기화");
        MyAppoCounselorUid.Clear();
        MyAppoCounselorName.Clear();
        MyAppoRequestDay.Clear();
        MyAppoAppDay1.Clear();
        MyAppoAppDay2.Clear();
        MyAppoAppTime.Clear();
        MyAppoWorry.Clear();
        MyAppoProgress.Clear();
        MyAppoRefuse.Clear();
        MyAppoFeedback.Clear();


        if (requestCloneList != null)
        {
            for (int i = (requestCloneList.Count) - 1; i >= 0; i--)
            {
           
                Destroy(requestCloneList[i]);
                //Destroy(acceptBtnList[i]);

            }
            //requestCloneList.Clear();
        }
        
    }


}



