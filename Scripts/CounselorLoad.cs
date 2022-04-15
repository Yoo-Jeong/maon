using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;



public class CounselorLoad : MonoBehaviour
{
    public GameObject item;              // 리스트 상담사 버튼

    public Toggle familyT, relationshipT; // 상담사 전문분야 선택 토글
    public Text relationshipText;
    public RawImage profileImg, profileImg2;   // 상담사 프로필 이미지

    public Transform parent;
    public GameObject prefab;
    public int num;

    public string major; // 리스트 상담사 간략정보를 담을 변수
    public string Cuid;

    public string selectDay;                // 선택 날짜를 담을 변수
    public Text month, selectDayT;          // 달력의 월, 선택한 날짜 텍스트 변수

    public Text selectTime;                 // 선택한 시간을 담을 텍스트 변수

    // 시간 선택 토글
    public Toggle nine, ten, eleven, twelve, thirteen, fourteen, fifteen, sixteen, seventeen;


    // 상담사 상세 프로필을 담을 변수
    public Text Cname1, Cname2, Cname3, Cmajor, Cintro, career1, career2, career3;

    public Text worry, worry2; // 고민내용을 담을 변수


    // 예약 신청내용 확인 팝업창 안의 텍스트
    public Text Cname4, selectDayT2, selectTime2;


    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
       new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        SetFunction_UI();
        //item.SetActive(false);



        relationshipText = relationshipT.GetComponentInChildren<Text>(); // 토글 텍스트 색 변경을 위해 컴포넌트를 가져옴
        relationshipText.color = Color.black; // 초기 토글 텍스트 색

        nineText = nine.GetComponentInChildren<Text>();
        nineText.color = Color.black;



    }


    // 캔버스. 
    public Canvas home, counsel;
    public Canvas Cprofile, Reservation, popup, myPage;
    public Image checkPopup, completePopup;
    public GameObject checkPopupObj;
    public GameObject ReservationObj, CprofileObj;

    // 상담사 상세 프로필 및 예약 캔버스 오픈.
    public void CprofileCan()
    {
        home.enabled = false;
        counsel.enabled = false;

        CprofileObj.SetActive(true);
        Cprofile.enabled = true;
        ReservationObj.SetActive(false);
        Reservation.enabled = false;
        popup.enabled = false;
    }


    // int타입 변수 number2 받아와 
    // prefabList[number2] 의 텍스트 값들로 상세 프로필 화면에 있는 텍스트값을 바꿈
    public void InputData(int number2)
    {
        print("버튼 클릭");

        SelectDay();

        Cname1.text = prefabList[number2].GetComponentInChildren<Text>().text;
        print(prefabList[number2].GetComponentInChildren<Text>().text + "번호 : " + number2);

        Cuid = CounselorList.relationshipUid[number2];

        StartCoroutine(GetTexture(CounselorList.relationshipPic[number2]));

        career1.text = CounselorList.relationshipCareer1[number2];
        career2.text = CounselorList.relationshipCareer2[number2];
        career3.text = CounselorList.relationshipCareer3[number2];


    }


    // 프리팹 관련 변수들.
    //public int number;
    public List<GameObject> prefabList = new List<GameObject>();
    public Text[] newCounselorDataShort;
    public List<Button> btnList = new List<Button>();

    public void Function_Toggle(bool _bool)
    {
        Debug.Log("대인관계 선택 : " + _bool);


        if (_bool == true)
        {
            // 토글 텍스트 색 변경
            relationshipText.color = Color.white;
            relationshipText.text = "<b>대인관계</b>";
            //relationshipText.text = $"<b>{relationshipText.text}</b>";

            major = "대인관계";

            // CounselorList클래스의 대인관계전문 상담사이름의 수만큼 반복한다.
            for (int i = 0; i < CounselorList.relationshipUsername.Count; i++)
            {
                print("리스트 저장 완료2/ " + " i=" + i + "/ 데이터 : " + CounselorList.relationshipUsername[i]);

                prefab = Instantiate(item, parent);  // item의 부모객체 위치에 프리팹 생성
                prefabList.Add(prefab); // 프리팹 리스트 prefabList에 프리팹 추가
                prefab.SetActive(true);


                // 텍스트 리스트 new newCounselorDataShort는 게임오브젝트 변수 prefab의 Text타입인 자식 객체들
                newCounselorDataShort = prefab.GetComponentsInChildren<Text>();
                newCounselorDataShort[0].text = CounselorList.relationshipUsername[i];  // newCounselorDataShort[0]의 텍스트는 상담사 이름
                newCounselorDataShort[1].text = CounselorList.relationshipIntro[i];     //newCounselorDataShort[1]의 텍스트는 한 줄 소개
                newCounselorDataShort[2].text = "대인관계";                             // newCounselorDataShort[2]의 텍스트는 전문분야(대인관계)


                btnList.Add(prefab.GetComponentInChildren<Button>());  // 버튼 리스트 btnList에 게임오브젝트 변수 prefab의 Button타입 객체(각1개임) 추가

                // 프리팹 안에 들어있는 버튼이 클릭 되었을때
                int temp = i;

                // btnLis의 temp번째 버튼이 눌렸을때(onClick) CprofileCan()함수 실행( 상세 프로필화면으로 이동)
                btnList[i].onClick.AddListener(() => { CprofileCan(); });

                // btnLis의 temp번째 버튼이 눌렸을때(onClick) InputData(temp)함수 실행(상세 프로필 화면에 있는 텍스트 값을 해당 프리팹의 데이터로 변경)
                btnList[temp].onClick.AddListener(() => { InputData(temp); });


                prefab.transform.position = new Vector3(1450, 367, 0);

                print("상담사 프리팹 생성" + i);


            }


        }
        else
        {
            relationshipText.color = Color.black;
            relationshipText.text = "대인관계";

            for (int i = CounselorList.relationshipUsername.Count - 1; i >= 0; i--)
            {
                Destroy(prefabList[i]);
                prefabList.Remove(prefabList[i]);
                print("제거" + i);
            }

        }



    }


    private void Function_fToggle(bool _bool)
    {
        Debug.Log("가족 선택 : " + _bool);


        if (_bool == true)
        {

        }
        else
        {
            item.SetActive(false);
        }


    }

    // 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            profileImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            profileImg2.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //prefab.GetComponentInChildren<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }


    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        relationshipT.onValueChanged.AddListener(Function_Toggle);
        familyT.onValueChanged.AddListener(Function_fToggle);
        nine.onValueChanged.AddListener(nine_Toggle);

        ten.onValueChanged.AddListener(ten_Toggle);
        eleven.onValueChanged.AddListener(eleven_Toggle);
        twelve.onValueChanged.AddListener(twelve_Toggle);
        thirteen.onValueChanged.AddListener(thirteen_Toggle);
        fourteen.onValueChanged.AddListener(fourteen_Toggle);
        fifteen.onValueChanged.AddListener(fifteen_Toggle);
        sixteen.onValueChanged.AddListener(sixteen_Toggle);
        seventeen.onValueChanged.AddListener(seventeen_Toggle);
    }

    private void ResetFunction_UI()
    {
        relationshipT.onValueChanged.RemoveAllListeners();
        familyT.onValueChanged.RemoveAllListeners();
        nine.onValueChanged.RemoveAllListeners();

        ten.onValueChanged.RemoveAllListeners();
        eleven.onValueChanged.RemoveAllListeners();
        twelve.onValueChanged.RemoveAllListeners();
        thirteen.onValueChanged.RemoveAllListeners();
        fourteen.onValueChanged.RemoveAllListeners();
        fifteen.onValueChanged.RemoveAllListeners();
        sixteen.onValueChanged.RemoveAllListeners();
        seventeen.onValueChanged.RemoveAllListeners();
    }


    public void SelectDay()
    {
        if (DayBtn.thisDay == null)
        {
            DayBtn.thisDay = DateTime.Now.Day.ToString();
            print(DayBtn.thisDay);
        }
        else
        {
            print(DayBtn.thisDay);

        }
        string monthSub = month.text;
        selectDay = "2022." + monthSub.Substring(0, monthSub.Length - 1) + "." + DayBtn.thisDay + ".";
        selectDayT.text = selectDay;
    }

    private Text nineText;
    private void nine_Toggle(bool _bool)
    {
        Debug.Log("9시 - 10시 : " + _bool);

        if (_bool == true)
        {
            print(nine.GetComponentInChildren<Text>().text);
            selectTime.text = nine.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;

            // 토글 텍스트 색 변경
            nineText.color = Color.white;
            nineText.text = "<b>9:00 - 10:00</b>";

        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;


            // 토글 텍스트 색 변경
            nineText.color = Color.black;
            nineText.text = "9:00 - 10:00";
        }
    }

    private void ten_Toggle(bool _bool)
    {
        Debug.Log("10시 - 11시 : " + _bool);

        if (_bool == true)
        {
            print(ten.GetComponentInChildren<Text>().text);
            selectTime.text = ten.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void eleven_Toggle(bool _bool)
    {
        Debug.Log("11시 - 12시 : " + _bool);

        if (_bool == true)
        {
            print(eleven.GetComponentInChildren<Text>().text);
            selectTime.text = eleven.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void twelve_Toggle(bool _bool)
    {
        Debug.Log("12시 - 13시 : " + _bool);

        if (_bool == true)
        {
            print(twelve.GetComponentInChildren<Text>().text);
            selectTime.text = twelve.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void thirteen_Toggle(bool _bool)
    {
        Debug.Log("13시 - 14시 : " + _bool);

        if (_bool == true)
        {
            print(thirteen.GetComponentInChildren<Text>().text);
            selectTime.text = thirteen.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void fourteen_Toggle(bool _bool)
    {
        Debug.Log("14시 - 15시 : " + _bool);

        if (_bool == true)
        {
            print(fourteen.GetComponentInChildren<Text>().text);
            selectTime.text = fourteen.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }


    private void fifteen_Toggle(bool _bool)
    {
        Debug.Log("15시 - 16시 : " + _bool);

        if (_bool == true)
        {
            print(fifteen.GetComponentInChildren<Text>().text);
            selectTime.text = fifteen.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void sixteen_Toggle(bool _bool)
    {
        Debug.Log("16시 - 17시 : " + _bool);

        if (_bool == true)
        {
            print(sixteen.GetComponentInChildren<Text>().text);
            selectTime.text = sixteen.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }

    private void seventeen_Toggle(bool _bool)
    {
        Debug.Log("17시 - 18시 : " + _bool);

        if (_bool == true)
        {
            print(seventeen.GetComponentInChildren<Text>().text);
            selectTime.text = seventeen.GetComponentInChildren<Text>().text;
            selectTime.color = Color.black;
        }
        else
        {
            selectTime.text = "시간을 선택하세요.";
            selectTime.color = Color.grey;
        }
    }


    public void ReserBtn()
    {
        Cname4.text = Cname2.text;
        selectDayT2.text = selectDay;
        selectTime2.text = selectTime.text;
        worry2.text = worry.text;

    }

    public bool isAppo;
    // 팝업창 최종 예약하기 버튼
    public void ReservationFinal()
    {
        // RDB에 내담자 데이터 저장
        ClientAppo clientAppo = new ClientAppo(Cuid, "", worry2.text, "", selectDayT2.text, selectTime2.text
            , Cname4.text, 0);


        // 상담사 하위에 예약신청 정보 저장
        CounselorAppo counselorAppo = new CounselorAppo(Auth_Manager.User.UserId, "", worry2.text, "", selectDayT2.text, selectTime2.text
            , Cname4.text, 0);


        // 데이터를 json형태로 반환
        string json = JsonUtility.ToJson(clientAppo);
        string json2 = JsonUtility.ToJson(counselorAppo);




        Debug.Log(Auth_Manager.User.UserId + "\n" + Cuid);
        Debug.Log(clientAppo.counselorUid);


        // push(key) 통일해주기
        // 생성된 키의 자식으로 json데이터를 삽입
        reference.Child("ClientUsers").Child(Auth_Manager.User.UserId).Child("appointment").Child(Cuid).Push().SetRawJsonValueAsync(json);
        reference.Child("CounselorUsers").Child(major).Child(Cuid).Child("appointment").Child(Auth_Manager.User.UserId).Push().SetRawJsonValueAsync(json2);

        print("예약 완료");


        // 예약하면 appointmentcheck true로 업데이트
        Dictionary<string, object> isAppo = new Dictionary<string, object>();
        isAppo["appointmentcheck"] = true;
        reference.Child("ClientUsers").Child(Auth_Manager.User.UserId).UpdateChildrenAsync(isAppo);

       


    }




    // 내담자 레코드 하위에 위치한 예약 레코드(appointment)
    class ClientAppo
    {
        // 거절사유, 고민내용, 내담자 후기, 상담날짜, 상담시간, 신청인(내담자)이름
        public string counselorUid, refuse, worry, feedback, appDay, appTime, counselorInCharge;


        // 수락상태, 0:무반응 1:수락 2:거절
        public int progress;

        public ClientAppo(string counselorUid, string refuse, string worry, string feedback,
            string appDay, string appTime, string counselorInCharge, int progress)
        {
            this.counselorUid = counselorUid;
            this.refuse = refuse;
            this.worry = worry;
            this.feedback = feedback;
            this.appDay = appDay;
            this.appTime = appTime;
            this.counselorInCharge = counselorInCharge;
            this.progress = progress;

        }


    }

    // 상담사 레코드 하위에 위치한 예약 레코드(appointment)
    class CounselorAppo
    {
        // 내담자uid, 거절사유, 고민내용, 내담자 후기, 상담날짜, 상담시간, 신청인(내담자)이름
        public string clientUid, refuse, worry, feedback, appDay, appTime, client;

        // 수락상태, 0:무반응 1:수락 2:거절
        public int progress;

        public CounselorAppo(string clientUid, string refuse, string worry, string feedback,
            string appDay, string appTime, string client, int progress)
        {
            this.clientUid = clientUid;
            this.refuse = refuse;
            this.worry = worry;
            this.feedback = feedback;
            this.appDay = appDay;
            this.appTime = appTime;
            this.client = client;
            this.progress = progress;

        }








    } }






    





