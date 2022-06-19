using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


//상담소 카드선택
public class CardController_Client : MonoBehaviourPunCallbacks
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    public string todayClientName;  //오늘 상담 내담자의 이름
    public string todayClientUid;   //오늘 상담 내담자의 Uid
    public string todayApp2;        //예약 DB에 접근하기 위한 예약 날짜 정보
    public Text stateText;          //상담사씬에서 보여줄 대기 안내 텍스트

    public GameObject[] rootObj;    //카드 최상위 게임오브젝트
    public PhotonView[] PV;         //포톤뷰 배열   CardController_Client

    public ToggleGroup emotionGroup;  //감정카드 토글 그룹
    public ToggleGroup needGroup;     //욕구카드 토글 그룹
    public Toggle[] emotionToggles;   //감정카드 토글들을 담을 배열
    public Toggle[] needToggles;      //욕구카드 토글들을 담을 배열

    public Button[] selectFinishBtn;  //카드선택 완료 버튼

    public int emotionCount = 3;   //감정카드 선택 제한 횟수 3회
    public int needCount = 3;     //욕구카드 선택 제한 횟수 3회
    public string[] selectedEmotions = new string[3];  //선택한 감정카드 3가지를 저장할 배열
    public string[] selectedNeeds = new string[3];     //선택한 욕구카드 3가지를 저장할 배열

    public string currentEmotion;   //가장 최근 선택한 감정카드 1가지
    public string currentNeed;      //가장 최근 선택한 욕구카드 1가지

    public string emotionNum;
    int emotionNumCount;              //경로 뒤에 붙일 숫자 (예: emotion + emotionNumCount) <이렇게 하면 emotion1이 되도록
    public string needNum;
    int needNumCount;                //경로 뒤에 붙일 숫자 (예: need + needNumCount) <이렇게 하면 need1이 되도록


    public GameObject CardPopup;       //카드 선택결과 안내 팝업
    public Text[] seletedCardText;    //팝업창에서 보여줄 선택한 카드 텍스트들을 담을 배열  [카드],감정을(욕구를) 



    // 상담 마무리화면 레포트관련 변수들 선언 시작
    public Text[] todayCounsel;           //오늘상담 정보를 표시할 텍스트 배열

    public Transform emotionParent, needParent;    //선택한 카드 프리팹이 생성될 위치의 부모객체의 위치.(parent의 자식으로 프리팹생성)
    public GameObject[] CardPrefabs = new GameObject[2];                           //카드 프리팹을 저장할 배열
    public GameObject[] emotionPrefabs = new GameObject[3];   //선택한 감정카드 프리팹을 저장할 배열
    public GameObject[] needPrefabs = new GameObject[3];      //선택한 욕구카드 프리팹을 저장할 배열

    public Text[] emotionReportText;   //레포트 감정카드 하단 글자를 저장할 배열
    public Text[] needReportText;      //레포트 욕구카드 하단 글자를 저장할 배열

    public Text maonText;              //마온 한마디 텍스트

    public InputField feedback;       //피드백 인풋필x


    public void Awake()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        InitEmotionData();
        InitNeedData();
    }

    // Start is called before the first frame update
    void Start()
    {
        CardPopup.SetActive(false);
        SetFunction_UI();   //토글 초기화

        rootObj[0].transform.localScale = new Vector3(0, 0, 0);
        rootObj[1].transform.localScale = new Vector3(0, 0, 0);

        emotionCount = 3;
        needCount = 3;

        emotionNum = "emotion";
        emotionNumCount = 1;
        needNum = "need";
        needNumCount = 1;

        selectFinishBtn[0].interactable = false;
        selectFinishBtn[1].interactable = false;

        Debug.Log("감정카드 선택 제한 : " + emotionCount + "\n" + "욕구카드 선택 제한: " + needCount);



        //상담 마무리 레포트관련
        todayCounsel[0].text = User_Data.todayCounsel[1];     //상담사이름
        todayCounsel[1].text = User_Data.todayCounsel[8];     //상담 분야
        //todayCounsel[1].text = "가족";     //상담 분야
        todayCounsel[2].text = User_Data.todayCounsel[3];     //예약일
        todayCounsel[3].text = User_Data.todayCounsel[4];     //상담일
        todayCounsel[4].text = User_Data.todayCounsel[6];     //상담 시간

        todayCounsel[5].text = User_Data.todayCounsel[1];     //상담사이름 (상담 마무리피드백 화면)

        Debug.Log("오늘상담 분야: " + todayCounsel[1].text);

        PrintMaonText(); //마온 한마디 출력

    }


    //감정카드 선택완료 버튼 클릭
    public void ClickEmotionFinish()
    {
        Debug.Log("실행시작: ClickEmotionFinish()");

        SelectEmotion();

        PV[0].RPC("CloseEmotion", RpcTarget.All);

        View_Controller.counselPopup.enabled = true;
        CardPopup.SetActive(true);
        seletedCardText[0].text = "[ " + currentEmotion + " ]";
        seletedCardText[1].text = "감정을";

        Invoke("DisableCardPopup", 3f);

        UpdateEmotionData();    

        Debug.Log("실행종료: ClickEmotionFinish()");

    }

    //욕구카드 선택완료 버튼 클릭
    public void ClickNeedFinish()
    {
        Debug.Log("실행시작: ClickNeedFinish()");

        SelectNeed();

        PV[0].RPC("CloseNeed", RpcTarget.All);

        View_Controller.counselPopup.enabled = true;
        CardPopup.SetActive(true);
        seletedCardText[0].text = "[ " + currentNeed + " ]";
        seletedCardText[1].text = "욕구를";

        Invoke("DisableCardPopup", 3f);

        UpdateNeedData();

        Debug.Log("실행종료: ClickNeedFinish()");

    }


    //카드팝업 안내창 사라지게 하는 함수
    public void DisableCardPopup()
    {
        View_Controller.counselPopup.enabled = false;
        CardPopup.SetActive(false);

    }


    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    public void OpenEmotion()
    {
        Debug.Log("감정카드 화면 열기");
        rootObj[0].transform.localScale = new Vector3(1, 1, 1);

        //토글그룹의 토글이 하나라도 켜져있다면
        if (emotionGroup.AnyTogglesOn())
        {
            emotionGroup.SetAllTogglesOff(); //토글 그룹에 속한 모든 토글 끄기
           
        }

        selectFinishBtn[0].interactable = false;
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

        //토글그룹의 토글이 하나라도 켜져있다면
        if (needGroup.AnyTogglesOn())
        {
            needGroup.SetAllTogglesOff(); //토글 그룹에 속한 모든 토글 끄기
           
        }

        selectFinishBtn[1].interactable = false;
    }

    [PunRPC]
    public void CloseNeed()
    {
        Debug.Log("욕구카드 화면 닫기");
        rootObj[1].transform.localScale = new Vector3(0, 0, 0);
    }


    //마스터 권한 가져오기
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

    //감정카드 선택 토글 값 저장
    public void SelectEmotion()
    {
        // 토글이 켜지면
        if (emotionToggles[0].isOn)
        {         
            currentEmotion = ("행복");
            Debug.Log("선택: 행복");

           

        }
        else if (emotionToggles[1].isOn)
        {
            currentEmotion = ("분노");
            Debug.Log("선택: 분노");

 

        }
        else if (emotionToggles[2].isOn)
        {
            currentEmotion = ("슬픔");
            Debug.Log("선택: 슬픔");

       

        }
        else if (emotionToggles[3].isOn)
        {
            currentEmotion = ("놀라움");
            Debug.Log("선택: 놀라움");

           

        }
        else if (emotionToggles[4].isOn)
        {
            currentEmotion = ("혐오");
            Debug.Log("선택: 혐오");

          
        }
        else if (emotionToggles[5].isOn)
        {
            currentEmotion = ("공포");
            Debug.Log("선택: 공포");

    

        }
        

    }


    //욕구카드 선택 토글 값 저장
    public void SelectNeed()
    {
        // 토글이 켜지면
        if (needToggles[0].isOn)
        {
            currentNeed = ("자신감");
            Debug.Log("선택: 자신감");

        

        }
        else if (needToggles[1].isOn)
        {
            currentNeed = ("휴식");
            Debug.Log("선택: 휴식");

          
        }
        else if (needToggles[2].isOn)
        {
            currentNeed = ("공감");
            Debug.Log("선택: 공감");

        

        }
        else if (needToggles[3].isOn)
        {
            currentNeed = ("애정");
            Debug.Log("선택: 애정");

    

        }
        else if (needToggles[4].isOn)
        {
            currentNeed = ("해결책");
            Debug.Log("선택: 해결책");

      

        }
        else if (needToggles[5].isOn)
        {
            currentNeed = ("성취");
            Debug.Log("선택: 성취");

       

        }
       

    }


    //상담예약 진행상황을 4로 업데이트(상담종료)
    public void UpdateAppoProgress()
    {
        Debug.Log("상담종료 업데이트 시작");

       
        Dictionary<string, object> updateProgress = new Dictionary<string, object>();
        updateProgress["progress"] = 4;

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateProgress);

        Debug.Log("상담종료 업데이트 완료");
    }


    public void UpdateFeedback()
    {
        Debug.Log("피드백 저장 시작");

        Dictionary<string, object> updateFeedback = new Dictionary<string, object>();
        updateFeedback["feedback"] = feedback.text;

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateFeedback);

        reference.Child("CounselorUsers").Child(User_Data.todayCounsel[8]).Child(User_Data.todayCounsel[0]).Child("appointment")
           .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateFeedback);

        Debug.Log("피드백 저장 완료");
    }


    //선택한 감정카드 정보를 DB에 업데이트하는 함수.
    public void UpdateEmotionData()
    {
        Debug.Log("감정카드 정보 DB에 업데이트 시작");

        emotionNum = "emotion";

        emotionNum = emotionNum + emotionNumCount;
        Debug.Log(emotionNum + " / 상담사쪽: " + CardController.emotionCount);

        selectedEmotions[emotionCount - emotionCount] = currentEmotion;

        //선택한 감정카드 값 저장
        Dictionary<string, object> updateEmotionCard = new Dictionary<string, object>();
        updateEmotionCard[emotionNum] = selectedEmotions[emotionCount - emotionCount];

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateEmotionCard);

        //선택한 감정카드를 레포트에 프리팹으로 생성
        emotionPrefabs[emotionNumCount - 1] = Instantiate(CardPrefabs[0], emotionParent);
        emotionPrefabs[emotionNumCount - 1].GetComponentInChildren<Text>().text = currentEmotion;
        emotionReportText[emotionNumCount - 1].text = "[" + currentEmotion + "] ";

        emotionNumCount++;
     
        Debug.Log("감정카드 정보 DB에 업데이트 완료 / " + emotionCount + " / emotionNumCount: " + emotionNumCount);
    }


    //선택한 욕구카드 정보를 DB에 업데이트하는 함수.
    public void UpdateNeedData()
    {
        Debug.Log("욕구카드 정보 DB에 업데이트 시작");

        needNum = "need";

        needNum = needNum + needNumCount;
        Debug.Log(needNum + " / 상담사쪽: " + CardController.needCount);

        selectedNeeds[needCount - needCount] = currentNeed;

        //선택한 욕구카드 값 저장
        Dictionary<string, object> updateNeedCard = new Dictionary<string, object>();
        updateNeedCard[needNum] = selectedNeeds[needCount - needCount];

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateNeedCard);

        //선택한 감정카드를 레포트에 프리팹으로 생성
        needPrefabs[needNumCount - 1] = Instantiate(CardPrefabs[1], needParent);
        needPrefabs[needNumCount - 1].GetComponentInChildren<Text>().text = currentNeed;
        needReportText[needNumCount - 1].text = "[" + currentNeed + "] ";

        needNumCount++;

        Debug.Log("욕구카드 정보 DB에 업데이트 완료 / " + needCount + " / emotionNumCount: " + needNumCount);
    }



    public void InitEmotionData()
    {
        Debug.Log("감정카드 정보DB 초기화");

        //감정카드 정보DB 초기화
        Dictionary<string, object> updateEmotionCard = new Dictionary<string, object>();
        updateEmotionCard["emotion1"] = "";
        updateEmotionCard["emotion2"] = "";
        updateEmotionCard["emotion3"] = "";

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateEmotionCard);

        Debug.Log("감정카드 정보DB 초기화 완료 / " + emotionCount + " / emotionNumCount: " + emotionNumCount);
     
    }


    public void InitNeedData()
    {
        Debug.Log("욕구카드 정보DB 초기화");

        //욕구카드 정보DB 초기화
        Dictionary<string, object> updateNeedCard = new Dictionary<string, object>();
        updateNeedCard["need1"] = "";
        updateNeedCard["need2"] = "";
        updateNeedCard["need3"] = "";

        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).Child("appointment")
            .Child(User_Data.todayCounsel[5]).UpdateChildrenAsync(updateNeedCard);

        Debug.Log("욕구카드 정보DB 초기화 완료 / " + needCount + " / emotionNumCount: " + needNumCount);

    }


    /*  public void GetData()
      {
          PV.RPC("GetEmotionData", RpcTarget.MasterClient, emotionCount);       
      }*/


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

                          + "\n 욕구카드1 : " + snapshot.Child("need1").Value
                          + "\n 욕구카드2 : " + snapshot.Child("need2").Value
                          + "\n 욕구카드3 : " + snapshot.Child("need3").Value
                          );


                          selectedEmotions[0] = (string)(snapshot.Child("emotion1").Value);
                          selectedEmotions[1] = (string)(snapshot.Child("emotion2").Value);
                          selectedEmotions[2] = (string)(snapshot.Child("emotion3").Value);


                          selectedNeeds[0] = (string)(snapshot.Child("need1").Value);
                          selectedNeeds[1] = (string)(snapshot.Child("need2").Value);
                          selectedNeeds[2] = (string)(snapshot.Child("need3").Value);


                          currentEmotion = selectedEmotions[emotionCount - emotionCount];


                          stateText.text = (todayClientName + " 님은 현재" + "\n" + "\n"
                         + currentEmotion + " 의 감정을" + "\n" + "\n"
                         + "느끼고 있습니다.");


                          print("내담자 감정 카드 불러오기 완료");
                      }

                  });

    }


    public void OnToggleBtn()
    {
        selectFinishBtn[0].interactable = true;
        selectFinishBtn[1].interactable = true;
    }


    public void PrintMaonText()
    {
        if(todayCounsel[1].text == "가족")
        {
            maonText.text = "우리는 대부분 스스로의 의지와 상관없이 맺어진 가족과 함께 살아가죠. 그렇기에 가족에게 느끼는 불편하고 싫은 감정은 당연해요. 그런 감정을 외면하지말고 부딪혀 보면 어떨까요? 진심을 말로 표현하는 것은 소통의 첫걸음이랍니다.";

        }else if (todayCounsel[1].text == "나자신")
        {
            maonText.text = "당신을 끊임없이 다른 사람으로 만들려고 노력하는 세상에서 당신 자신이 되는 것이 가장 큰 성취입니다. 끝나지 않는 시련은 없어요. 행복이 찾아올 때까지 스스로를 끊임없이 응원해주세요 :)";
        }
        else if (todayCounsel[1].text == "대인관계")
        {
            maonText.text = "4-4-1-1 법칙에 따르면 10명 중 4명은 나에게 무관심하고, 4명은 나를 싫어하고, 1명은 보통이고, 1명만이 나를 좋아한다고 해요. 모두 나를 좋아할 필요는 없어요.";
        }
        else if (todayCounsel[1].text == "연애")
        {
            maonText.text = "나 자신을 알고 상대와 맞춰가며 앞으로 나아가는 사랑을 하길 응원해요. 당신을 있는 그대로 사랑해 주는 소중한 인연과 만나 행복하길 바라요.";
        }
        else if (todayCounsel[1].text == "직장")
        {
            maonText.text = "나에게 완벽하게 딱 맞는 직장은 없어요. 지금 힘든 일도 성장의 기회라고 생각하고 극복해 보는 건 어떨까요? 오늘도 열심히 하려는 당신을 응원해요!";
        }
        else if(todayCounsel[1].text == "진로취업")
        {
            maonText.text = "실패했을 때보다 포기했을 때 잃는 게 더 많다고 해요. 지금 나는 잠시 쉬어가는 것일 뿐! 절대 포기하지 말고 다시 천천히 시작해봐요 :)";
        }
        else
        {
            maonText.text = "";
        }

    }



    // 방 퇴장
    public void OutRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("방 퇴장");
    }


   
    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        emotionToggles[0].onValueChanged.AddListener(Function_tgHappy);
        emotionToggles[1].onValueChanged.AddListener(Function_tgAnger);
        emotionToggles[2].onValueChanged.AddListener(Function_tgSad);
        emotionToggles[3].onValueChanged.AddListener(Function_tgSurprise);
        emotionToggles[4].onValueChanged.AddListener(Function_tgHate);
        emotionToggles[5].onValueChanged.AddListener(Function_tgHorror);

        needToggles[0].onValueChanged.AddListener(Function_tgConfidence);
        needToggles[1].onValueChanged.AddListener(Function_tgRest);
        needToggles[2].onValueChanged.AddListener(Function_tgSympathy);
        needToggles[3].onValueChanged.AddListener(Function_tgLove);
        needToggles[4].onValueChanged.AddListener(Function_tgSolution);
        needToggles[5].onValueChanged.AddListener(Function_tgAchievement);

    }

    private void ResetFunction_UI()
    {
        emotionToggles[0].onValueChanged.RemoveAllListeners();
        emotionToggles[1].onValueChanged.RemoveAllListeners();
        emotionToggles[2].onValueChanged.RemoveAllListeners();
        emotionToggles[3].onValueChanged.RemoveAllListeners();
        emotionToggles[4].onValueChanged.RemoveAllListeners();
        emotionToggles[5].onValueChanged.RemoveAllListeners();

        needToggles[0].onValueChanged.RemoveAllListeners();
        needToggles[1].onValueChanged.RemoveAllListeners();
        needToggles[2].onValueChanged.RemoveAllListeners();
        needToggles[3].onValueChanged.RemoveAllListeners();
        needToggles[4].onValueChanged.RemoveAllListeners();
        needToggles[5].onValueChanged.RemoveAllListeners();

    }


    public void Function_tgHappy(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[0].GetComponentInChildren<Text>().color = Color.white;
        }
        else {
            emotionToggles[0].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgAnger(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[1].GetComponentInChildren<Text>().color = Color.white;

        }
        else
        {
            emotionToggles[1].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgSad(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[2].GetComponentInChildren<Text>().color = Color.white;
   
        }
        else
        {
            emotionToggles[2].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgSurprise(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[3].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            emotionToggles[3].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgHate(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[4].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            emotionToggles[4].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgHorror(bool _bool)
    {
        if (_bool)
        {
            emotionToggles[5].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            emotionToggles[5].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }


    public void Function_tgConfidence(bool _bool)
    {
        if (_bool)
        {
            needToggles[0].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[0].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgRest(bool _bool)
    {
        if (_bool)
        {
            needToggles[1].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[1].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgSympathy(bool _bool)
    {
        if (_bool)
        {
            needToggles[2].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[2].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgLove(bool _bool)
    {
        if (_bool)
        {
            needToggles[3].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[3].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgSolution(bool _bool)
    {
        if (_bool)
        {
            needToggles[4].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[4].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

    public void Function_tgAchievement(bool _bool)
    {
        if (_bool)
        {
            needToggles[5].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            needToggles[5].GetComponentInChildren<Text>().color = new Color(133 / 255f, 154 / 255f, 94 / 255f);
        }
    }

}
