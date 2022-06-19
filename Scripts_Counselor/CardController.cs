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


public class CardController : MonoBehaviourPunCallbacks
{
    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    public Text ClientDisplyName;   //오늘 상담 내담자의 이름을 표시할 Text
    public string todayClientName;  //오늘 상담 내담자의 이름
    public string todayClientUid;   //오늘 상담 내담자의 Uid
    public string todayApp2;        //예약 DB에 접근하기 위한 예약 날짜 정보

    public GameObject[] rootObj;    //카드 최상위 게임오브젝트

    public PhotonView[] PV;           //포톤 뷰

    public GameObject[] contentsBg;   //~물어보기 범위 게임오브젝트
    public GameObject[] question;     //궁금하신가요? 게임오브젝트
    public GameObject[] yesButton;    //네 버튼
    public GameObject[] state;        //내담자가 카드를 고르는 중 게임오브젝트
    public Text[] stateText;          //내담자가 카드를 고르는 중 텍스트
    public GameObject[] reButton;     //다시 물어보기 게임오브젝트


    public static int emotionCount = 3;                    //감정카드 선택 제한 횟수 3회
    public static int needCount = 3;                     //욕구카드 선택 제한 횟수 3회
    public string[] selectedEmotions = new string[3];     //선택한 감정카드 3가지를 저장할 배열
    public string[] selectedNeeds = new string[3];        //선택한 욕구카드 3가지를 저장할 배열

    public string emotionNum;
    int emotionNumCount;             //경로 뒤에 붙일 숫자 (예: emotion + emotionNumCount) <이렇게 하면 emotion1이 되도록
    public string needNum;
    int needNumCount;                //경로 뒤에 붙일 숫자 (예: need + needNumCount) <이렇게 하면 need1이 되도록

    public string currentEmotion;   //가장 최근 선택한 감정카드 1가지
    public string currentNeed;      //가장 최근 선택한 욕구카드 1가지

    bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        for (int i = 0; i < rootObj.Length; i++)
        {
            rootObj[i].transform.localScale = new Vector3(0, 0, 0);
            reButton[i].SetActive(false);
            stateText[i].text = state[i].GetComponent<Text>().text;
        }
  
        emotionCount = 3;
        needCount = 3;

        emotionNum = "emotion";
        emotionNumCount = 0;
        needNum = "need";
        needNumCount = 0;
        Debug.Log("감정카드 선택 제한 : " + emotionCount + "\n" + "욕구카드 선택 제한: " + needCount);

     
        todayClientUid = DataMngForCounselor.todayCounselData[0];   //오늘 상담 내담자의 Uid (파이어베이스 경로를 찾기 위해 필요)
        todayClientName = DataMngForCounselor.todayCounselData[1];  //오늘 상담 내담자의 이름
        todayApp2 = DataMngForCounselor.todayCounselData[3];        //오늘 상담 날짜(yyyy년mm월dd일)


        //ChildChanged 이벤트 리스너 연결
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(todayClientUid);
        userRef.ChildChanged += HandleChildChanged;

    }


    //ChildChanged 이벤트 핸들러 구현
    void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        
        GetEmotionData(emotionCount);  //감정카드 정보 불러오기
        GetNeedData(needCount);        //욕구카드 정보 불러오기
        Debug.Log(" ChildChanged 이벤트핸들러 ");

    }


    //감정 물어보기 버튼 클릭
    public void ClickEmotionBtn()
    {
        if (open)
        {
            state[0].SetActive(false);
            contentsBg[0].SetActive(true);
            reButton[0].SetActive(false);
            yesButton[0].SetActive(true);
            question[0].SetActive(true);

            if (emotionCount <= 0)
            {
                yesButton[0].GetComponent<Button>().interactable = false;
                Debug.Log("감정카드를 이미 3회 선택하셨습니다.");
            }

            open = false;
        }

        else
        {          
            contentsBg[0].SetActive(false);
            reButton[0].SetActive(false);
            yesButton[0].SetActive(false);
            question[0].SetActive(false);

            open = true;
        }


    }


    //욕구 물어보기 버튼 클릭
    public void ClickNeedBtn()
    {
        if (open)
        {
            state[1].SetActive(false);
            contentsBg[1].SetActive(true);
            reButton[1].SetActive(false);
            yesButton[1].SetActive(true);
            question[1].SetActive(true);

            if (needCount <= 0)
            {
                yesButton[1].GetComponent<Button>().interactable = false;
                Debug.Log("욕구카드를 이미 3회 선택하셨습니다.");
            }

            open = false;
        }
        else
        {
            contentsBg[1].SetActive(false);
            reButton[1].SetActive(false);
            yesButton[1].SetActive(false);
            question[1].SetActive(false);

            open = true;
        }


    }


   
    //감정카드의 네 버튼을 클릭하면 내담자화면에 감정카드 화면을 연다.
    public void ClickYesEmotionBtn()
    {
        if (emotionCount > 0) //감정카드 선택 횟수가 1회이상 남았으면
        {
            OwnerTake(); //마스터 권한 가져오기

            reButton[0].SetActive(false);

            PV[0].RPC("OpenEmotion", RpcTarget.All); //포톤 방에 입장해 있는 모든 사람들에게 OpenEmotion()함수 실행(감정카드 화면 열기)

            question[0].SetActive(false);
            yesButton[0].SetActive(false);
            state[0].SetActive(true);
            stateText[0].text = (". . ." + "\n" + "\n" + "내담자가 감정카드를" + "\n" + "고르고 있습니다.");


            Debug.Log("감정카드 선택 남은 횟수: " + emotionCount);
            emotionCount--;
            Debug.Log("이번 감정카드 선택이 끝나면 남는 횟수: " + emotionCount);

            
        }
        else
        {
            Debug.Log("감정카드를 이미 3회 선택하셨습니다.");
            yesButton[0].GetComponent<Button>().interactable = false;

        }

    }


    //욕구카드의 네 버튼을 클릭하면 내담자화면에 감정카드 화면을 연다.
    public void ClickYesNeedBtn()
    {
        if (needCount > 0) //욕구카드 선택 횟수가 1회이상 남았으면
        {
         
            OwnerTake(); //마스터 권한 가져오기

            reButton[1].SetActive(false);

            PV[0].RPC("OpenNeed", RpcTarget.All); //포톤 방에 입장해 있는 모든 사람들에게 OpenNeed()함수 실행(욕구카드 화면 열기)

            question[1].SetActive(false);
            yesButton[1].SetActive(false);
            state[1].SetActive(true);
            stateText[1].text = (". . ." + "\n" + "\n" + "\n" + "내담자가 욕구카드를" + "\n" + "고르고 있습니다.");

            Debug.Log("욕구카드 선택 남은 횟수: " + needCount);
            needCount--;
            Debug.Log("이번 욕구카드 선택이 끝나면 남는 횟수: " + needCount);
        }
        else
        {
            Debug.Log("욕구카드를 이미 3회 선택하셨습니다.");
            yesButton[1].GetComponent<Button>().interactable = false;

   
        }

    }


    [PunRPC]
    public void OpenEmotion()
    {   
        Debug.Log("감정카드 화면 열기");
        rootObj[0].transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void CloseEmotion()
    {
        Debug.Log("감정카드 화면 닫기");
        rootObj[0].transform.localScale = new Vector3(0, 0, 0);
    }



    [PunRPC]
    public void OpenNeed()
    {
        Debug.Log("욕구카드 화면 열기");
        rootObj[1].transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void CloseNeed()
    {
        Debug.Log("욕구카드 화면 닫기");
        rootObj[1].transform.localScale = new Vector3(0, 0, 0);
    }



    //마스터 권한이 없으면 권한을 가져오는 함수
    public void OwnerTake()
    {
        if (PV[0].GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("마스터 입니다.");
        }
        else
        {
            PV[0].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("마스터 권한을 가져옵니다.");
        }
    }


    //파이어베이스에서 감정카드 정보를 불러오는 함수
    [PunRPC]
    public void GetEmotionData(int emotionCount)
    {
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(todayClientUid).Child("appointment").Child(todayApp2)
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
                          Debug.Log($"내담자 예약정보 레코드 개수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                          Debug.Log("감정카드1 : " + snapshot.Child("emotion1").Value
                          + "\n 감정카드2 : " + snapshot.Child("emotion2").Value
                          + "\n 감정카드3 : " + snapshot.Child("emotion3").Value
                          );


                          selectedEmotions[0] = (string)(snapshot.Child("emotion1").Value);
                          selectedEmotions[1] = (string)(snapshot.Child("emotion2").Value);
                          selectedEmotions[2] = (string)(snapshot.Child("emotion3").Value);


                          if (selectedEmotions[emotionNumCount] != "")
                          {
                              currentEmotion = selectedEmotions[emotionNumCount];
                              emotionNumCount++;


                              stateText[0].text = (todayClientName + " 님은 현재" + "\n" + "\n"
                             + currentEmotion + " 의 감정을" + "\n" + "\n"
                             + "느끼고 있습니다.");
                          }

                          Debug.Log("GetEmotionData: " + emotionCount + "/ " + emotionNumCount);

                          Debug.Log("내담자 감정카드 불러오기 완료");
                      }

                  });


        if (emotionCount > 0)
        {
            reButton[0].SetActive(true);
            reButton[0].GetComponent<Button>().interactable = false;
            Invoke("useEmotionReplayBtn", 3f);
        }
       
    }


    //파이어베이스에서 욕구카드 정보를 불러오는 함수
    [PunRPC]
    public void GetNeedData(int needCount)
    {
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(todayClientUid).Child("appointment").Child(todayApp2)
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
                          Debug.Log($"내담자 예약정보 레코드 개수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                          Debug.Log("욕구카드1 : " + snapshot.Child("need1").Value
                          + "\n 욕구카드2 : " + snapshot.Child("need2").Value
                          + "\n 욕구카드3 : " + snapshot.Child("need3").Value
                          );
    
                          selectedNeeds[0] = (string)(snapshot.Child("need1").Value);
                          selectedNeeds[1] = (string)(snapshot.Child("need2").Value);
                          selectedNeeds[2] = (string)(snapshot.Child("need3").Value);

                          if (selectedNeeds[needNumCount] != "")
                          {
                              currentNeed = selectedNeeds[needNumCount];
                              needNumCount++;

                              stateText[1].text = (todayClientName + " 님에게는 현재" + "\n" + "\n"
                             + currentNeed + " 이 필요합니다.");
                          }

                          Debug.Log("GetNeedData: " + needCount + "/ " + needNumCount);

                          Debug.Log("내담자 욕구카드 불러오기 완료");
                      }

                  });

        if (needCount > 0)
        {
            reButton[1].SetActive(true);
            reButton[1].GetComponent<Button>().interactable = false;
            Invoke("useNeedReplayBtn", 3f);
        }


    }


    //3초 후에 감정카드 다시물어보기 버튼을 활성화시키기 위한 함수
    public void useEmotionReplayBtn()
    {
        reButton[0].GetComponent<Button>().interactable = true;
    }


    //3초 후에 욕구카드 다시물어보기 버튼을 활성화시키기 위한 함수
    public void useNeedReplayBtn()
    {
        reButton[1].GetComponent<Button>().interactable = true;
    }



    // 포톤 방 퇴장
    public void OutRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("포톤 방 퇴장합니다.");
    }


}

