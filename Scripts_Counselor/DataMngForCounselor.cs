using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;



public class DataMngForCounselor : MonoBehaviour
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }  

    string family = "가족";
    string myself = "나 자신";
    string relationship = "대인관계";
    string romance = "연애";
    string work = "직장";
    string career = "진로/취업";
  

    public Text displayname;
    public string myMajor;
    public bool isAppointment;
    public int appoCount;

    public List<string> MyAppoclientUid = new List<string>();
    public List<string> MyAppoClientname = new List<string>(); 
    public List<string> MyAppoAppDay1 = new List<string>();
    public List<string> MyAppoAppDay2 = new List<string>();
    public List<string> MyAppoAppTime = new List<string>();
    public List<string> MyAppoWorry = new List<string>();
    public List<long> MyAppoProgress = new List<long>();

    public Text todayName, todayCounsel, todayTimeCounsel; // 우측 상단 상담소 입장관련 텍스트들.


    public string todayYear;
    public string todayMonth;
    public string todayDay;

    public string todayString;

    public GameObject counselStart;


    public Button logoutBtn;

  

    private void Awake()
    {
        //ClearLists();

        counselStart.SetActive(false);

        todayYear  = DateTime.Now.ToString("yyyy");
        todayMonth = DateTime.Now.ToString("MM");
        todayDay = DateTime.Now.ToString("dd");
        todayString = todayYear + "." + todayMonth + "." + todayDay + ".";

        print(todayString);
    }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;    

        isAppointment = false;
        LoadMyData();

        logoutBtn.onClick.AddListener(Auth_Manager.Instance.Logout);

    }


    //상담사 유저 하위에 있는 내용에 대한 변경을 읽고 수신 대기하는 이벤트핸들러 구현
    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log(" ChildChanged 이벤트핸들러 ");

        LoadMyAppoData(myMajor);
    }


    public void LoadMyData()
    {
           
        if (Auth_Manager.user != null)
        {         
                LoadMyInfo(family);        
                LoadMyInfo(myself);         
                LoadMyInfo(relationship);                      
                LoadMyInfo(romance);         
                LoadMyInfo(work);         
                LoadMyInfo(career);           
        }


    }


    public void LoadMyInfo(string major)
    {
        FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major).Child(Auth_Manager.user.UserId)
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
                           print( major + $" 하위에 있는가? : {snapshot.ChildrenCount}"); //데이터 건수 출력

                            
                           if (snapshot.ChildrenCount > 0)
                           {
                               //이벤트리스너 연결
                               var userRef = FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major).Child(Auth_Manager.user.UserId);
                               userRef.ChildChanged += HandleChildChanged;          //상담사 유저 하위에 있는 내용에 대한 변경을 읽고 수신 대기

                               myMajor = major;

                               //우측 상단 내담자 이름 표시.
                               displayname.text = snapshot.Child("username").Value.ToString();

                               Debug.Log("상담사: " + snapshot.Child("userGroup").Value
                                     + "\n uid: " + snapshot.Child("uid").Value
                                     + "\n email: " + snapshot.Child("email").Value
                                     + "\n pic: " + snapshot.Child("pic").Value
                                     + "\n username: " + snapshot.Child("username").Value
                                     + "\n sex: " + snapshot.Child("sex").Value
                                     + "\n intro: " + snapshot.Child("intro").Value
                                     + "\n family: " + snapshot.Child("family").Value
                                     + "\n myself: " + snapshot.Child("myself").Value
                                     + "\n relationship: " + snapshot.Child("relationship").Value
                                     + "\n romance: " + snapshot.Child("romance").Value
                                     + "\n work: " + snapshot.Child("work").Value
                                     + "\n career: " + snapshot.Child("career").Value
                                     + "\n career1: " + snapshot.Child("career1").Value
                                     + "\n career2: " + snapshot.Child("career2").Value
                                     + "\n career3: " + snapshot.Child("career3").Value
                                     + "\n appointmentcheck: " + snapshot.Child("appointmentcheck").Value
                                     );

                               isAppointment = (bool)snapshot.Child("appointmentcheck").Value;

                               //만약 예약이 있다면 예약 목록 불러오기 실행.
                               if (isAppointment)
                               {
                                   Debug.Log(isAppointment);
                                   Debug.Log(major);
                                   LoadMyAppoData(major);
                                   print("상담 예약이 있습니다.");
                               }
                               else
                               {
                                   print("상담 예약이 없습니다.");
                               }

                               print("내 기본정보 불러오기 완료");
                           }


                       }
                       else
                       {
                           Debug.Log("로그인이 필요합니다.");
                       }


                   });
    }

    


    // RDB에서 예약정보를 읽어오는 함수.
    public void LoadMyAppoData(string major)
    {
        ClearLists();

        Debug.Log("실행 시작: LoadMyAppoData(string major)");

        FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child(major).Child(Auth_Manager.user.UserId)
      .Child("appointment").GetValueAsync().ContinueWithOnMainThread(task =>
      {
          if (task.IsFaulted)
          {
              // Handle the error...
              print("예약정보 데이터베이스 읽기 실패: LoadMyAppoData(string major)");
          }

          if (task.IsCompleted)
          {

              Debug.Log("예약정보 데이터베이스 읽기 성공: LoadMyAppoData(string major)");

              DataSnapshot snapshot = task.Result;
              appoCount = (int)snapshot.ChildrenCount;


              foreach (DataSnapshot data in snapshot.Children)
              {
           
                  IDictionary appo = (IDictionary)data.Value;
             
                  MyAppoclientUid.Add((string)appo["clientUid"]);
                  MyAppoClientname.Add((string)appo["clientName"]);
                  MyAppoAppDay1.Add((string)appo["appDay1"]);
                  MyAppoAppDay2.Add((string)appo["appDay2"]);
                  MyAppoAppTime.Add((string)appo["appTime"]);
                  MyAppoWorry.Add((string)appo["worry"]);
                  MyAppoProgress.Add((long)appo["progress"]);

              }

              LoadRequestList();
             
              
          }

          Debug.Log("예약정보 추가 완료: LoadMyAppoData(string major).");

      });


    }




    public Transform parent;                        // 신청 온 예약 프리팹이 생성될 위치의 부모객체의 위치
    public GameObject requestPrefab, requestClone;  // 신청 온 예약 프리팹, 복제된 신청 온 예약 프리팹 (requestClone은 인스펙터 창에서 드래그앤드롭 안해도 됨)

    public List<GameObject> requestCloneList = new List<GameObject>();    //생성된 프리팹을 담을 리스트.
    public Text[] newRequestData;                                         // 프리팹 안의 텍스트타입 게임오브젝트를 담을 배열.
    public List<Button> acceptBtnList = new List<Button>();               // 생성된 프리팹들의 수락 버튼들을 담을 리스트.
    public List<Button> refuseBtnList = new List<Button>();               // 생성된 프리팹들의 거절 버튼들을 담을 리스트.



    public void LoadRequestList()
    {
        Debug.Log("실행 시작: LoadRequestList()");
       

        // 예약을 신청한 날짜의 개수만큼 반복한다.
        for (int i = 0; i < appoCount; i++)
        {
          
            // 프리팹 안에 들어있는 프로필확인 버튼이 클릭 되었을때 실행 될 내용들.
            int temp = i; // i값 복사해서 사용. for문 안의 람다식에서 그대로 i를 사용하면 i가 전부 같은 값이 되어버림!!(클로저 때문에)

            // 텍스트 배열 newRequestData는 생성된 requestClone프리팹의 Text타입인 자식 객체들
            newRequestData = requestPrefab.GetComponentsInChildren<Text>();
            newRequestData[0].text = "* 신청인: " + MyAppoClientname[i];  // newRequestData[0]의 텍스트는 신청인 이름
            newRequestData[1].text = "* 상담 일시: " + MyAppoAppDay1[i] + "      " + MyAppoAppTime[i];  // newRequestData[1]의 텍스트는 상담 일시
            newRequestData[2].text = "* 고민 내용: " + MyAppoWorry[i];  // newRequestData[2]의 텍스트는 고민 내용 

            requestClone = Instantiate(requestPrefab, parent);   // parent위치에 requestPrefab을 requestClone으로 생성
            requestCloneList.Add(requestClone);                 // 생성된 클론프리팹을 requestCloneList에 추가 
            requestCloneList[i].SetActive(true);
   

            print("버튼 넣기 시작" + requestCloneList[i].transform.GetChild(4).GetComponentInChildren<Button>().name);
            // 수락버튼 리스트 acceptBtnList에 requestClone프리팹 하위에있는 Button(1개임) 추가
            acceptBtnList.Add(requestCloneList[i].transform.GetChild(4).GetComponentInChildren<Button>());
            // 거절버튼 리스트 refuseBtnList requestClone프리팹 하위에있는 Button(1개임) 추가
            refuseBtnList.Add(requestCloneList[i].transform.GetChild(5).GetComponentInChildren<Button>());
            print("버튼 넣기 완료");


            acceptBtnList[temp].onClick.AddListener(() => { AcceptRequest(temp); });
            refuseBtnList[temp].onClick.AddListener(() => { RefuseRequest(temp); });

            if (MyAppoProgress[i] == 0)
            {
                Debug.Log(MyAppoProgress[i] + "은 신청 온 예약");
              
            }
            else if (MyAppoProgress[i] == 1)
            {
                Debug.Log(MyAppoProgress[i] + "은 수락된 예약");
                Destroy(requestCloneList[i]);

                AcceptedCounsel(i); //수락된 프리팹 생성

            }
            else
            {
                Debug.Log(MyAppoProgress[i] + "은 거절된 예약");
                Destroy(requestCloneList[i]);

            }


        }

        Debug.Log("실행 완료: LoadRequestList()");

    }




    // int타입 변수 num 받아와  requestCloneList[num]의 텍스트를 불러올 수 있음.
    public void AcceptRequest(int num)
    {
        print("수락 버튼 클릭");
        print(requestCloneList[num].GetComponentInChildren<Text>().text + "수락 : " + num);

        // 수락하면 progress 1로 업데이트
        Dictionary<string, object> isAccept = new Dictionary<string, object>();
        isAccept["progress"] = 1;
        reference.Child("ClientUsers").Child(MyAppoclientUid[num]).Child("appointment").Child(MyAppoAppDay2[num]).UpdateChildrenAsync(isAccept);
        reference.Child("CounselorUsers").Child(myMajor).Child(Auth_Manager.user.UserId).Child("appointment").Child(MyAppoAppDay2[num]).UpdateChildrenAsync(isAccept);


        Destroy(requestCloneList[num]);
        //requestCloneList.Remove(requestCloneList[num]);
        //acceptBtnList.Remove(acceptBtnList[num]);
        //refuseBtnList.Remove(refuseBtnList[num]);
        //MyAppoProgress[num] = 1;
        print("수락 제거" + num);

        AcceptedCounsel(num);

    } // AcceptRequest(int num).

    public void RefuseRequest(int num)
    {
        print("거절 버튼 클릭");
        print(requestCloneList[num].GetComponentInChildren<Text>().text + "거절 : " + num);

        // 거절하면 progress 2로 업데이트
        Dictionary<string, object> isRefuse = new Dictionary<string, object>();
        isRefuse["progress"] = 2;
        reference.Child("ClientUsers").Child(MyAppoclientUid[num]).Child("appointment").Child(MyAppoAppDay2[num]).UpdateChildrenAsync(isRefuse);
        reference.Child("CounselorUsers").Child(myMajor).Child(Auth_Manager.user.UserId).Child("appointment").Child(MyAppoAppDay2[num]).UpdateChildrenAsync(isRefuse);


        Destroy(requestCloneList[num]);
        //requestCloneList.Remove(requestCloneList[num]);
        //acceptBtnList.Remove(acceptBtnList[num]);
        //refuseBtnList.Remove(refuseBtnList[num]);
        //MyAppoProgress[num] = 2;
        print("거절 제거" + num);

    } // RefuseRequest(int num).





    public Transform acceptedparent;                        // 수락된 프리팹이 생성될 위치의 부모객체의 위치
    public GameObject acceptedPrefab, acceptedClone;        // 수락된 프리팹, 수락된 예약 프리팹(acceptedClone 인스펙터 창에서 드래그앤드롭 안해도 됨)

    public List<GameObject> acceptedCloneList = new List<GameObject>();    //생성된 프리팹을 담을 리스트.
    public Text[] newAcceptedData;                                         // 프리팹 안의 텍스트타입 게임오브젝트를 담을 배열.

    public string[] todayCounselData;      //오늘 상담의 내담자 정보를 저장할 배열.

    public void AcceptedCounsel(int num)
    {

        acceptedClone = Instantiate(acceptedPrefab, acceptedparent);   //parent위치에 acceptedPrefab을 acceptedClone으로 생성
        acceptedCloneList.Add(acceptedClone);                          //생성된 클론프리팹을 acceptedCloneList 추가
        acceptedClone.SetActive(true);

       
        // 텍스트 배열 newAcceptedData 생성된 acceptedClone프리팹의 Text타입인 자식 객체들
        newAcceptedData = acceptedClone.GetComponentsInChildren<Text>();
        newAcceptedData[0].text = "* 내담자: " + MyAppoClientname[num];  // newAcceptedData[0]의 텍스트는 신청인 이름
        newAcceptedData[1].text = "* 상담 일시: " + MyAppoAppDay1[num] + "      " + MyAppoAppTime[num];  // newAcceptedData[1]의 텍스트는 상담 일시


        if (MyAppoAppDay1[num] == todayString)
        {
            print("오늘 예정된 상담이 있습니다.");
            todayName.text = MyAppoClientname[num];
            todayCounsel.text = MyAppoAppDay1[num];
            todayTimeCounsel.text = MyAppoAppTime[num];

            todayCounselData = new string[] { MyAppoclientUid[num], MyAppoClientname[num], MyAppoAppDay1[num],MyAppoAppDay2[num]
                    , MyAppoAppTime[num], MyAppoWorry[num]};
         

            counselStart.SetActive(true);
 

        }


    }


    //리스트들 초기화
    public void ClearLists()
    {
        MyAppoclientUid.Clear();
        MyAppoClientname.Clear();
        MyAppoAppDay1.Clear();
        MyAppoAppDay2.Clear();
        MyAppoAppTime.Clear();
        MyAppoWorry.Clear();
        MyAppoProgress.Clear();

      

        if(requestCloneList != null)
        {
            for(int i = (requestCloneList.Count)-1; i >= 0; i--)
            {
               /* DestroyImmediate(requestCloneList[i], true);
                DestroyImmediate(acceptBtnList[i], true);
                DestroyImmediate(refuseBtnList[i], true);*/

                Destroy(requestCloneList[i]);
                Destroy(acceptBtnList[i]);
                Destroy(refuseBtnList[i]);
            }
        }

        if (acceptedCloneList != null)
        {
            for (int i = (acceptedCloneList.Count) - 1; i >= 0; i--)
            {
                //DestroyImmediate(acceptedCloneList[i], true);
                Destroy(acceptedCloneList[i]);
            }
        }

        requestCloneList.Clear();
        acceptBtnList.Clear();
        refuseBtnList.Clear();


        acceptedCloneList.Clear();


    }


}
