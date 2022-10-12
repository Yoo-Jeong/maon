using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;

using Newtonsoft.Json;

public class FeedbackList : MonoBehaviour
{

    public Text counselorName, counselDay, counselTime, concern;
    public Text counselorName2, counselDay2, counselTime2;


    // 내담자 상담예약 정보 관련 리스트.
    public List<string> appointmentCounselorUid = new List<string>();
    public static List<string> appointmentCounselorPushKey = new List<string>();

    
    public static List<string> MyAppoCounselorName = new List<string>();
    public  List<string> appointmentRequestDay = new List<string>();
    public static List<string> appointmentAppDay1 = new List<string>();
    public List<string> appointmentAppDay2 = new List<string>();
    public static List<string> appointmentAppTime = new List<string>();
    public static List<string> appointmentFeedback = new List<string>();
    public static List<string> appointmentWorry = new List<string>();
    public List<string> appointmentDiary = new List<string>();
    public List<long> appointmentProgress = new List<long>();

    int j;

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }



    public void Awake()
    {
        ClearLists();
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
       new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        

        //LoadAppoDataCounselorUid(); //사용안함

        LoadAppoData();

        //이벤트리스너 연결
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId);

        userRef.ChildChanged += HandleChildChanged;
        userRef.Child("appointment").ChildRemoved += HandleChildRemoved;


    }


    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        LoadAppoData();
        Debug.Log(" ChildChanged 이벤트핸들러 ");

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



    public void LoadAppoData()
    {
        ClearLists();

        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.user.UserId)
      .Child("appointment").GetValueAsync().ContinueWithOnMainThread(task =>
      {
          if (task.IsCompleted)
          {
              Debug.Log("내담자 예약정보 불러오기 완료.");

              DataSnapshot snapshot = task.Result;

              string json = task.Result.GetRawJsonValue();


              foreach (DataSnapshot data in snapshot.Children)
              {

                  IDictionary appo = (IDictionary)data.Value;

                  appointmentCounselorUid.Add((string)appo["counselorUid"]);
                  MyAppoCounselorName.Add((string)appo["counselorName"]);
                  appointmentAppDay1.Add((string)appo["appDay1"]);
                  appointmentAppDay2.Add((string)appo["appDay2"]);
                  appointmentAppTime.Add((string)appo["appTime"]);
                  appointmentFeedback.Add((string)appo["feedback"]);
                  appointmentWorry.Add((string)appo["worry"]);
                  appointmentDiary.Add((string)appo["diary"]);
                  appointmentProgress.Add((long)appo["progress"]);
                  appointmentRequestDay.Add((string)appo["requestDay"]);
                 


              }
              CreatekFeedbackList(); // 피드백 프리팹을 생성하는 함수.
          }

      });


    }



    public GameObject feedbackPrefab, clone;
    public Transform feedbackParent;

    public List<GameObject> feedbackPrefabList = new List<GameObject>();
    public List<Button> newFeedbackButton = new List<Button>();
    public List<Button> newFeedbackOpenButton = new List<Button>();

    public List<Text> newFeedbackProcess = new List<Text>();
    public List<Text> newFeedbackName = new List<Text>();
    public List<Text> newFeedbackRequestDay = new List<Text>();
    public List<Text> newFeedbackAppDay1 = new List<Text>();
    public List<Text> newFeedbackAppTIme = new List<Text>();


    /// <summary>
    /// 피드백 프리팹을 생성하는 함수.
    /// </summary>
    public void CreatekFeedbackList()
    {
        j = 0;

        print("예약 갯수" + MyAppoCounselorName.Count);
        print("생성 시작");

        for (int i = 0; i < MyAppoCounselorName.Count; i++)
        {

            if (appointmentProgress[i] == 4)
            {
                clone = Instantiate(feedbackPrefab, feedbackParent);       // feedbackPrefab의 하위로 feedbackPrefab 객체 생성
                feedbackPrefabList.Add(clone);                             // 프리팹 리스트 feedbackPrefabList에 복제된 프리팹 추가
                print("생성 완료" + MyAppoCounselorName.Count);

                print("텍스트 완료 1 : " + i + " / " + j);

                int temp = i;
                
                newFeedbackProcess.Add(feedbackPrefabList[j].transform.GetChild(1).GetComponentInChildren<Text>());
                newFeedbackName.Add(feedbackPrefabList[j].transform.GetChild(2).GetComponentInChildren<Text>());
                newFeedbackRequestDay.Add(feedbackPrefabList[j].transform.GetChild(3).GetComponentInChildren<Text>());
                newFeedbackAppDay1.Add(feedbackPrefabList[j].transform.GetChild(4).GetComponentInChildren<Text>());
                newFeedbackAppTIme.Add(feedbackPrefabList[j].transform.GetChild(5).GetComponentInChildren<Text>());


                newFeedbackName[j].text = MyAppoCounselorName[temp];
                newFeedbackRequestDay[j].text = appointmentRequestDay[temp];
                newFeedbackAppDay1[j].text = appointmentAppDay1[temp];
                newFeedbackAppTIme[j].text = appointmentAppTime[temp];

                print("텍스트 완료 2 : " + i + " / " + j + "/ " + appointmentAppTime[i]);

                feedbackPrefabList[j].transform.GetChild(7).GetComponentInChildren<InputField>().text = appointmentDiary[temp];

                feedbackPrefabList[j].SetActive(true);

              

                print("텍스트 완료 3 : " + i + " / " + j + MyAppoCounselorName[temp]);

                print("버튼 넣기 시작");
                int tempj = j;
                newFeedbackButton.Add(feedbackPrefabList[tempj].transform.GetChild(7).GetComponentInChildren<Button>());
                newFeedbackOpenButton.Add(feedbackPrefabList[tempj].GetComponentInChildren<Button>());

                newFeedbackButton[tempj].onClick.AddListener(() => { InputFeedback(temp, tempj); });
                newFeedbackOpenButton[tempj].onClick.AddListener(() => { ChangeSpacing(tempj); });

                print("버튼 넣기 완료");

                if (appointmentDiary[temp] == "")
                {
                    newFeedbackProcess[j].text = "작성 전";
                }
                else
                {
                    newFeedbackProcess[j].text = "작성완료";
                    newFeedbackButton[j].GetComponent<Image>().enabled = false;
                }

                j++;
                Debug.Log(j);
            }

            else
            {
                Debug.Log("완료된 상담이 없습니다.");
            }

        }


    }



    public string uid;


    /// <summary>
    /// 피드백 프리팹 하위에 있는 저장 버튼을 누르면 실행되는 함수.
    /// 프리팹안의 저장 버튼에 연결되어 있다.
    /// </summary>
    /// <param name="num"></param>
    /// <param name="numj"></param>
    public void InputFeedback(int num, int numj)
    {
        print("저장 버튼 클릭");
        print(num + " / " + numj);

        print(feedbackPrefabList[numj].transform.GetChild(7).GetComponentInChildren<InputField>().text);

        uid = appointmentCounselorUid[num];

        Dictionary<string, object> feedbackSave = new Dictionary<string, object>
        {
            ["diary"] = feedbackPrefabList[numj].transform.GetChild(7).GetComponentInChildren<InputField>().text
        };


        try
        {
            
            reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment").Child(appointmentAppDay2[num]).UpdateChildrenAsync(feedbackSave);
            //reference.Child("CounselorUsers").Child("대인관계").Child(uid).Child("appointment").Child(appointmentAppDay2[num]).UpdateChildrenAsync(feedbackSave);


            newFeedbackProcess[numj].text = "작성완료";
            print("감정일기 작성 완료.");

            newFeedbackButton[numj].GetComponent<Image>().enabled = false;
            //ClearLists();
        }
        catch (NullReferenceException ex)
        {
            print(ex);

            print("null... 널....");
        }

    }


    public void ChangeSpacing(int numj)
    {
        Debug.Log("화살표 클릭");
    }


    public void ClearLists()
    {

        if (feedbackPrefabList != null)
        {

            for (int i = (feedbackPrefabList.Count) - 1; i >= 0; i--)
            {
                Destroy(feedbackPrefabList[i]);

                Destroy(newFeedbackButton[i]);
                Destroy(newFeedbackName[i]);
                Destroy(newFeedbackRequestDay[i]);
                Destroy(newFeedbackAppDay1[i]);
                Destroy(newFeedbackAppTIme[i]);
            }

        }
        j = 0;

        feedbackPrefabList.Clear();

        newFeedbackButton.Clear();
        newFeedbackProcess.Clear();
        newFeedbackName.Clear();
        newFeedbackRequestDay.Clear();
        newFeedbackAppDay1.Clear();
        newFeedbackAppTIme.Clear();

        appointmentCounselorUid.Clear();
        appointmentProgress.Clear();
        MyAppoCounselorName.Clear();
        appointmentAppDay1.Clear();
        appointmentAppDay2.Clear();
        appointmentAppTime.Clear();
        appointmentFeedback.Clear();
        appointmentWorry.Clear();
        appointmentDiary.Clear();


    }

}

