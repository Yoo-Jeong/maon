using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class EmotionCtrl_counselor : MonoBehaviourPunCallbacks
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    public GameObject emotionCard;
    public GameObject bg;
    public GameObject state;
    public GameObject question;
    public GameObject yesbtn;

    public GameObject re;

    public string currentEmotion;


    public PhotonView PV;

    public string otherID;
    public Text stateText;

    public Text ClientDisplyName;
    public string todayClientName;
    public string todayClientUid;

    public string currentCard;

    bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        emotionCard.transform.localScale = new Vector3(0, 0, 0);
        re.SetActive(false);
        stateText.text = state.GetComponent<Text>().text;


        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        todayClientUid = GameObject.FindWithTag("DataMngForCounselor").GetComponent<DataMngForCounselor>().todayCounselData[0];
        todayClientName = GameObject.FindWithTag("DataMngForCounselor").GetComponent<DataMngForCounselor>().todayCounselData[1];


    }


    public void BtnClick()
    {
        if (open)
        {
            open = false;

            state.SetActive(false);
            bg.SetActive(true);
            re.SetActive(false);
            yesbtn.SetActive(true);
            question.SetActive(true);
        }
        else
        {
            open = true;

            bg.SetActive(false);
            re.SetActive(false);
            yesbtn.SetActive(false);
            question.SetActive(false);
        }


    }




    public void OnEmotionCard()
    {

        OwnerTake();

        re.SetActive(false);
        SetStateText();
        PV.RPC("OpenEmotion", RpcTarget.All);
        question.SetActive(false);
        yesbtn.SetActive(false);
        state.SetActive(true);

    }


    [PunRPC]
    public void OpenEmotion()
    {
        Debug.Log("감정카드 화면 열기");
        emotionCard.transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void CloseEmotion()
    {
        Debug.Log("감정카드 화면 닫기");
        emotionCard.transform.localScale = new Vector3(0, 0, 0);
    }


    public void OwnerGive()
    {
        Debug.Log("권한 양도");
        //emotion.RequestOwnership();

    }


    //마스터 권한
    public void OwnerTake()
    {
        if (emotionCard.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("소유자 입니다.");
        }
        else
        {
            emotionCard.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("소유권을 가져옵니다.");
        }
    }

    public void GetData()
    {

        PV.RPC("GetEmotionData", RpcTarget.MasterClient);

    }

    [PunRPC]
    public void GetEmotionData()
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


                   /*       Debug.Log("내담자: " + snapshot.Child("userGroup").Value
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
                                );*/


                          currentCard = (string)(snapshot.Child("emotionCard").Value);

                          stateText.text = (todayClientName + " 님은 현재" + "\n" + "\n"
                         + currentCard + " 의 감정을" + "\n" + "\n"
                         + "느끼고 있습니다.");

                          print("내담자 기본정보 불러오기 완료");
                      }

                  });


       

        WaitRe();

        /* var request = new GetUserDataRequest() { PlayFabId = otherID };

         PlayFabClientAPI.GetUserData(request, (result) =>
            stateText.text = (
              "김내담 님은 현재" + "\n" + "\n"
              + result.Data["감정"].Value + " 의 감정을" + "\n" + "\n"
              + "느끼고 있습니다."),

            (error) => print("데이터 불러오기 실패"));

         Invoke("WaitRe", 0.8f);*/

    }

    public void WaitRe()
    {
        re.SetActive(true);
    }

    public void SetStateText()
    {
        stateText.text = ("..." + "\n" + "\n" + "내담자가 감정카드를" + "\n" + "고르고 있습니다.");
    }



    // 방 퇴장
    public void OutRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("방 퇴장");
    }


}

