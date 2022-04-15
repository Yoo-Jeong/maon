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
    public static List<string> appointmentCounselorUid = new List<string>();
    public static List<string> appointmentCounselorPushKey = new List<string>();


    public static List<string> appointmentCounselorInCharge = new List<string>();
    public static List<string> appointmentAppDay = new List<string>();
    public static List<string> appointmentAppTime = new List<string>();
    public static List<string> appointmentFeedback = new List<string>();
    public static List<string> appointmentWorry = new List<string>();

    int temp;

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
       new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        //LodAppoData();
        //Invoke("inputAppoData", 2);
        LoadAppoDataJson();


    }

    public void LodAppoData()
    {
        //내담자 상담예약 정보
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.User.UserId)
            .Child("appointment")
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
                        print($"상담예약 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                        print(snapshot.GetRawJsonValue());
                        print(snapshot.Children);



                        //반복 시작
                        foreach (DataSnapshot data in snapshot.Children)
                        {


                            IDictionary appointment = (IDictionary)data.Value;



                            appointmentCounselorInCharge.Add((string)appointment["counselorInCharge"]);
                            appointmentAppDay.Add((string)appointment["appDay"]);
                            appointmentAppTime.Add((string)appointment["appTime"]);
                            appointmentFeedback.Add((string)appointment["feedback"]);
                            appointmentWorry.Add((string)appointment["worry"]);




                        }

                        print("예약 정보 불러오기 완료");

                    }
                    else
                    {
                        Debug.Log("내담자 상담예약 정보 - 로그인이 필요합니다.");
                    }


                });

    }


    public void inputAppoData()
    {

        for (int i = 0; i < 1; i++)
        {
            temp = i;
            print("출력 : " + i + appointmentCounselorInCharge[0]);

            counselorName.text = appointmentCounselorInCharge[0];
            counselorName2.text = appointmentCounselorInCharge[0];

            counselDay.text = appointmentAppDay[0];
            counselDay2.text = appointmentAppDay[0];

            counselTime.text = appointmentAppTime[0];
            counselTime2.text = appointmentAppTime[0];

            concern.text = appointmentWorry[0];

        }


    }


    // ClientAppo clientAppo = new ClientAppo();
    public void LoadAppoDataJson()
    {

        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.User.UserId)
            .Child("appointment").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("completed");

                    DataSnapshot snapshot = task.Result;

                    string json = task.Result.GetRawJsonValue();

                    //clientAppo = JsonUtility.FromJson<ClientAppo>(json);

                    foreach (DataSnapshot data in snapshot.Children)
                    {

                        appointmentCounselorUid.Add((string)data.Key);
                        print(appointmentCounselorUid[0]);


                        //clientAppo.appointmentCounselorUid.Add((string)data.Key);
                        //print(clientAppo.appointmentCounselorUid[0]);

                    }


                }


            });

    }



    public void LoadAppoDataPushKey()
    {
        // for(int i = 0; i < clientAppo.appointmentCounselorUid.Count; i++)
        for (int i = 0; i < appointmentCounselorUid.Count; i++)
        {
            int temp = i;

            
            FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(Auth_Manager.User.UserId)
          .Child("appointment").Child(appointmentCounselorUid[i]).GetValueAsync().ContinueWithOnMainThread(task =>
          {
              if (task.IsCompleted)
              {
                  Debug.Log("completed2");

                  DataSnapshot snapshot = task.Result;

                  string json = task.Result.GetRawJsonValue();

                  foreach (DataSnapshot data in snapshot.Children)
                  {

                      appointmentCounselorPushKey.Add((string)data.Key);
                      print(appointmentCounselorPushKey[temp]);


                      IDictionary appo = (IDictionary)data.Value;

                      appointmentCounselorInCharge.Add((string)appo["counselorInCharge"]);
                      appointmentAppDay.Add((string)appo["appDay"]);
                      appointmentAppTime.Add((string)appo["appTime"]);
                      appointmentFeedback.Add((string)appo["feedback"]);
                      appointmentWorry.Add((string)appo["worry"]);

                      print(appointmentCounselorInCharge[temp]);

                      CreatekFeedbackList();



                  }


              }

          });


        }


    }


    public GameObject feedbackPrefab, clone;
    public Transform feedbackParent;
    public Image imageBox;


    public static List<GameObject> feedbackPrefabList = new List<GameObject>();
    public static Text[] newFeedbackData;
    public static Button[] newFeedbackButton;

    public List<Button> writeList = new List<Button>();
    public void CreatekFeedbackList()
    {

        print("예약 갯수" + appointmentCounselorInCharge.Count);

        print("생성 시작");
        // feedbackPrefab의 하위로 feedbackPrefab 객체 생성
        clone = Instantiate(feedbackPrefab, feedbackParent);
        feedbackPrefabList.Add(feedbackPrefab); // 프리팹 리스트 feedbackPrefabList에 프리팹 추가
        print("생성 완료" + appointmentCounselorInCharge.Count);


        for (int i = 0; i < appointmentCounselorInCharge.Count; i++)
        {
            temp = i;

            print("텍스트 완료 1 : " + i + " / " + temp);

            newFeedbackData = feedbackPrefab.GetComponentsInChildren<Text>();

            newFeedbackButton = feedbackPrefab.GetComponentsInChildren<Button>();

            newFeedbackData[0].text = "작성 전";
            newFeedbackData[1].text = appointmentCounselorInCharge[i];
            newFeedbackData[2].text = appointmentAppDay[i];
            newFeedbackData[3].text = appointmentAppDay[i];
            newFeedbackData[4].text = appointmentAppTime[i];

            print("텍스트 완료 2 : " + i + " / " + temp + "/ " + appointmentAppTime[i]);

            feedbackPrefab.SetActive(true);

            print("텍스트 완료 3 : " + i + " / " + temp);


        }

    }

  
    public GameObject openObj;
    public InputField feedbackField;
    public Text feedbackText;
    public string uid, pushkey;

    public void InputFeedback(int num)
    {
        print("저장 버튼 클릭");
        print(num);

        print(feedbackPrefabList[num].name);
        print(feedbackField.text);

        feedbackPrefabList[num].GetComponentInChildren<InputField>().text = feedbackField.text;

        print(feedbackField.text);

        uid = appointmentCounselorUid[num];
        pushkey = appointmentCounselorPushKey[num];
        print("경로 완료 uid : " + uid + " / PushKey: " + pushkey);


        feedbackText.text = feedbackField.text;

        print("인풋 필드 컴포넌트 불러옴.");

        
        Dictionary<string, object> feedbackSave = new Dictionary<string, object>
        {
            ["feedback"] = feedbackField.text
        };
        print(feedbackSave.Keys);

        print(reference);
      

        try
        {
            print(feedbackSave.Keys + "밸류 : " + feedbackSave.Values + " 개수: " + feedbackSave.Count);

            print("경로 확인 1" + appointmentCounselorUid[num] + " / PushKey: " + appointmentCounselorPushKey[num]);
         
            print(feedbackSave.Keys);

            reference.Child("ClientUsers").Child(Auth_Manager.User.UserId).Child("appointment").Child(uid).Child(pushkey).UpdateChildrenAsync(feedbackSave);
            reference.Child("CounselorUsers").Child("대인관계").Child(uid).Child("appointment").Child(Auth_Manager.User.UserId).Child(pushkey).UpdateChildrenAsync(feedbackSave);

            print("경로 확인 2" + appointmentCounselorUid[num] + " / PushKey: " + appointmentCounselorPushKey[num]);

        }catch(NullReferenceException ex)
        {
            print(ex);

            print("null... 널....");
        }




    }





}




// 내담자 레코드 하위에 위치한 예약 레코드(appointment)
class ClientAppo
{
    // 상담사uid, 거절사유, 고민내용, 내담자 후기, 상담날짜, 상담시간, 상담사이름
    public string refuse, worry, feedback, appDay, appTime, client;

    // 수락상태, 0:무반응 1:수락 2:거절
    public int progress;



    public List<string> appointmentCounselorUid = new List<string>();
    public List<string> appointmentCounselorPushKey = new List<string>();



    public ClientAppo() { }


    public ClientAppo(string refuse, string worry, string feedback,
        string appDay, string appTime, string client, int progress)
    {

        this.refuse = refuse;
        this.worry = worry;
        this.feedback = feedback;
        this.appDay = appDay;
        this.appTime = appTime;
        this.client = client;
        this.progress = progress;



    }



}






/*{ "-N-SqsxFFxWCIOESEnRr":
    { "-N-Twut--ofzHRFQ6XEF":
        { "appDay":"2022.4.29."
            ,"appTime":"16:00 - 17:00"
            ,"counselorInCharge":"\uae40\uc0c1\ub2f4"
            ,"counselorUid":"-N-SqsxFFxWCIOESEnRr"
            ,"feedback":""
            ,"progress":0
            ,"refuse":""
            ,"worry":"\uc0c1\ub2f4 \uc2e0\uccad"
            } 
} 
}*/


/*{ "clientUid":null
    ,"pushKey":null
    ,"refuse":null
    ,"worry":null
    ,"feedback":null
    ,"appDay":null
    ,"appTime":null
    ,"client":null
    ,"progress":0
    }*/

