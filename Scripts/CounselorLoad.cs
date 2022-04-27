using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class CounselorLoad : MonoBehaviour
{
    // 변수선언 순서는 예약메뉴에서의 과업 흐름을 따라갑니다.


    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용.
    public DatabaseReference reference { get; set; }


    // 날짜 선택을 위한 변수들 선언.
    public Text calendarMonth;              // 달력의 달을 표시한 Text타입 변수(선택날짜를 알기 위해 필요)

    public string appDay1, appDay2;         // 선택한 날짜를 담을 변수, RDB저장 경로를 위한 날짜를 담을 변수


    // 전문분야 선택 토글 관련 변수들 선언.
    public Toggle familyToggle, relationshipToggle; // 전문분야 토글
    private Text relationshipText;                  // 전문분야 토글의 자식 텍스트


    // 전문분야에 따라 상담사의 정보를 저장할 Counselors타입 변수 선언.
    public Counselors relationship;     //전문분야가 대인관계인 상담사들


    // 상담사 프리팹 관련 변수들 선언.
    public Transform parent;                                                // 상담사 프리팹이 생성될 위치의 부모객체의 위치.(parent의 자식으로 프리팹생성)
    public GameObject counselorPrefab, counselorClone;                      // 상담사 프리팹, 복제된 상담사 프리팹

    public List<GameObject> counselorCloneList = new List<GameObject>();    // 복제된 프리팹들을(counselorClone) 담을 리스트

    public List<RawImage> profileImgList = new List<RawImage>();            // counselorClone프리팹들의 자식RawImage를 담을 리스트
    public List<Button> btnList = new List<Button>();                       // 복제된 프리팹들의 자식 버튼을 담을 리스트

    public Text[] newCounselorDataShort;                                    // 복제된 프리팹의 자식 Text타입 게임오브젝트를 담을 배열


    // 시간 선택 토글 변수들 선언.
    public Toggle nine, ten, eleven, twelve, thirteen, fourteen, fifteen, sixteen, seventeen;


    // 선택된 상담사 관련 변수들 선언.
    public string[] seleted = new string[9];            //선택된 상담사의 정보들을 저장할 배열

    public RawImage seletedProfileImg;                  //선택된 상담사의 프로필이미지를 표시할 RawImage
    public Text[] seletedNameText = new Text[4];        //선택된 상담사의 이름을 표시할 Text들을 저장하는 배열
    public Text seletedMajorText;                       //선택된 상담사의 전문분야를 표시할 Text변수
    public Text seletedIntroText;                       //선택된 상담사의 한줄 소개를 표시할 Text변수
    public Text[] seletedCareerText = new Text[3];      //선택된 상담사의 주요경력3가지를 표시할 Text들을 저장하는 배열


    // 선택된 정보 또는 입력된 정보 관련 변수들 선언.
    public Text selectedDay;         //선택된 날짜를 표시할 Text타입 변수       
    public Text selectedTime;        //선택된 시간을 표시할 Text타입 변수
    public Text worryInput;          //고민내용을 입력할 Text타입 변수


    // 예약확인 팝업창 관련 변수들 선언.
    public Text confirmDay;          //예약확인 팝업창에서 선택된 날짜를 표시할 Text타입 변수 
    public Text confirmTime;         //예약확인 팝업창에서 선택된 시간을 표시할 Text타입 변수 
    public Text confirmWorry;        //예약확인 팝업창에서 고민내용을 표시할 Text타입 변수 

    public string requestDay;        //예약을 신청한 날짜를 저장할 string타입 변수

    public string clientName;         //내담자 이름





    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        relationship = new Counselors("대인관계");


        SetFunction_UI();

        // 토글 텍스트 색 변경을 위해 컴포넌트를 가져옴
        relationshipText = relationshipToggle.GetComponentInChildren<Text>();
        relationshipText.color = Color.black; // 초기 토글 텍스트 색

        nineText = nine.GetComponentInChildren<Text>();
        nineText.color = Color.black;




    } // Start().




    public void Function_RelationshipToggle(bool _bool)
    {
        Debug.Log("대인관계 선택 : " + _bool);

        if (_bool == true)
        {
            relationshipText.color = Color.white;     // 토글 텍스트 색 변경.
            relationshipText.text = "<b>대인관계</b>"; // 토글 텍스트 볼드처리.

            SelectDay(); // 선택한 날짜를 저장하는 함수 실행.

            if (Home_Calendar.dayOfWeek == 1)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":월요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Monday);

            }
            else if (Home_Calendar.dayOfWeek == 2)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":화요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Tuesday);
            }
            else if (Home_Calendar.dayOfWeek == 3)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":수요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Wednesday);
            }
            else if (Home_Calendar.dayOfWeek == 4)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":목요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Thursday);
            }
            else if (Home_Calendar.dayOfWeek == 5)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":금요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Friday);
            }
            else if (Home_Calendar.dayOfWeek == 6)
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":토요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Saturday);
            }
            else
            {
                Debug.Log(Home_Calendar.dayOfWeek + ":일요일 / " + "대인관계");
                CreatPrefabs(relationship, relationship.Saturday);
            }

        }
        else
        {
            relationshipText.color = Color.black;
            relationshipText.text = "대인관계";

            DestroyPrefabs(relationship);

        }


    } // Function_RelationshipToggle(bool _bool).



    /* Counselors타입 변수counselors와 int타입 변수num을 받아와
     선택된 상담사의 정보를 저장하고 상세 프로필 화면에 있는 텍스트값을 바꾸는 함수*/
    public void InputSeletedData(Counselors counselors, int num)
    {
        print("프로필 확인 버튼 클릭");

        clientName = User_Data.myName; //내담자(나)의 이름 저장.


        //선택된 상담사의 정보만 seleted[]배열에 저장
        seleted[0] = counselors.major;
        seleted[1] = counselors.counselorsUid[num];
        seleted[2] = counselors.counselorsUsername[num];
        seleted[3] = counselors.counselorsIntro[num];
        seleted[4] = counselors.counselorsPic[num];
        seleted[5] = counselors.counselorsCareer1[num];
        seleted[6] = counselors.counselorsCareer2[num];
        seleted[7] = counselors.counselorsCareer3[num];
        seleted[8] = counselors.counselorsSex[num];

        //선택된 상담사의 전문분야를 표시할 Text타입 변수 seletedMajorText에 선택된 상담사의 전문분야 할당.
        seletedMajorText.text = seleted[0];

        //선택된 상담사의 이름을 표시할 Text타입 배열의 모든 요소에 선택된 상담사의 이름 할당. 
        seletedNameText[0].text = seleted[2];
        seletedNameText[1].text = seleted[2];
        seletedNameText[2].text = seleted[2];
        seletedNameText[3].text = seleted[2];

        //선택된 상담사의 한줄소개를 표시할 Text타입 변수 seletedIntroText에 선택된 상담사의 한줄소개 할당.
        seletedIntroText.text = seleted[3];

        //선택된 상담사의 프로필 이미지를 보여주기 위한 함수 실행.
        StartCoroutine(GetTexture(seleted[4]));

        //선택된 상담사의 주요 경력사항1,2,3을 표시할 Text타입 배열의 요소에 선택된 상담사의 경력1,2,3 각각 할당.
        seletedCareerText[0].text = seleted[5];
        seletedCareerText[1].text = seleted[6];
        seletedCareerText[2].text = seleted[7];


    } // InputSeletedData(Counselors counselors, int num) end.


    private void Function_familyToggle(bool _bool)
    {
        Debug.Log("가족 선택 : " + _bool);


        if (_bool == true)
        {

        }
        else
        {

        }


    } // Function_familyToggle(bool _bool) end.

    //선택된 상담사의 프로필이미지(ReservationProfileCanvas): 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            seletedProfileImg.texture = null;
        }
        else
        {
            seletedProfileImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        }
    }


    // 프리팹용(counselorClone) : 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    IEnumerator GetTexturePrefab(string url, int num)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            profileImgList[num].texture = null;
        }
        else
        {
            profileImgList[num].texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        }
    }



    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        relationshipToggle.onValueChanged.AddListener(Function_RelationshipToggle);
        familyToggle.onValueChanged.AddListener(Function_familyToggle);
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
        relationshipToggle.onValueChanged.RemoveAllListeners();
        familyToggle.onValueChanged.RemoveAllListeners();
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


    // 선택한 날짜를 저장하는 함수.
    public void SelectDay()
    {
        string seletedMonth = calendarMonth.text;    //선택한 날짜의 달을 담을 string타입 변수
        string day = DayBtn.thisDay;

        if (day == null)
        {
            day = DateTime.Now.Day.ToString();
            print("선택 날짜: " + day + "일");
        }
        else
        {
            print("선택 날짜: " + day + "일");

        }

        // seletedMonth가 1자리수면 앞에 "0"삽입(예:4를 04로 바꾼다.)
        if (seletedMonth.Length < 3)
        {
            seletedMonth = seletedMonth.Insert(0, "0");
        }

        appDay2 = "2022" + "년" + seletedMonth + day + "일";


        // 맨 뒤에 글자 "월"을 자른다 (예: 4월을 4로 바꾼다.)
        seletedMonth = seletedMonth.Substring(0, seletedMonth.Length - 1);
        appDay1 = "2022." + seletedMonth + "." + day + ".";

        selectedDay.text = appDay1;

    } // SelectDay() end.



    private Text nineText;
    private void nine_Toggle(bool _bool)
    {
        Debug.Log("9시 - 10시 : " + _bool);

        if (_bool == true)
        {
            print(nine.GetComponentInChildren<Text>().text);
            selectedTime.text = nine.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            nineText.color = Color.white;
            nineText.text = "<b>9:00 - 10:00</b>";

        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;


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
            selectedTime.text = ten.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void eleven_Toggle(bool _bool)
    {
        Debug.Log("11시 - 12시 : " + _bool);

        if (_bool == true)
        {
            print(eleven.GetComponentInChildren<Text>().text);
            selectedTime.text = eleven.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void twelve_Toggle(bool _bool)
    {
        Debug.Log("12시 - 13시 : " + _bool);

        if (_bool == true)
        {
            print(twelve.GetComponentInChildren<Text>().text);
            selectedTime.text = twelve.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void thirteen_Toggle(bool _bool)
    {
        Debug.Log("13시 - 14시 : " + _bool);

        if (_bool == true)
        {
            print(thirteen.GetComponentInChildren<Text>().text);
            selectedTime.text = thirteen.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void fourteen_Toggle(bool _bool)
    {
        Debug.Log("14시 - 15시 : " + _bool);

        if (_bool == true)
        {
            print(fourteen.GetComponentInChildren<Text>().text);
            selectedTime.text = fourteen.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }


    private void fifteen_Toggle(bool _bool)
    {
        Debug.Log("15시 - 16시 : " + _bool);

        if (_bool == true)
        {
            print(fifteen.GetComponentInChildren<Text>().text);
            selectedTime.text = fifteen.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void sixteen_Toggle(bool _bool)
    {
        Debug.Log("16시 - 17시 : " + _bool);

        if (_bool == true)
        {
            print(sixteen.GetComponentInChildren<Text>().text);
            selectedTime.text = sixteen.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }

    private void seventeen_Toggle(bool _bool)
    {
        Debug.Log("17시 - 18시 : " + _bool);

        if (_bool == true)
        {
            print(seventeen.GetComponentInChildren<Text>().text);
            selectedTime.text = seventeen.GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;
        }
    }


    // 첫번째 예약신청 버튼. (팝업창 텍스트 변경)
    public void ReservationTryBtn()
    {
        confirmDay.text = appDay1;
        confirmTime.text = selectedTime.text;
        confirmWorry.text = worryInput.text;

    }


    // 팝업창 최종 예약하기 버튼.
    public void ReservationFinalBtn()
    {
        //예약일자 저장(오늘 날짜)
        requestDay = DateTime.Now.ToString("yyyy.MM.dd");

        print("최종 예약하기 버튼 클릭" + " / 예약 일자:" + requestDay);

        // RDB에 내담자 데이터 저장
        ClientAppo clientAppo = new ClientAppo(
              seleted[1]                       //상담사uid
            , Auth_Manager.User.UserId         //내담자uid
            , ""                               //거절사유
            , confirmWorry.text                //고민내용
            , ""                               //내담자 후기(피드백)
            , requestDay                       //상담 신청 날짜
            , appDay1                          //상담날짜1 (yyyy.mm.dd.)
            , appDay2                          //상담날짜2 (yyyy년mm월dd일)
            , confirmTime.text                 //상담시간
            , 0                                //수락상태, 0:무반응 1:수락 2:거절 
            , seleted[2]                       //상담사 이름
            , seleted[8]                       //상담사 성별
            , clientName                       //내담자 이름
            , "");                             //감정카드



        // 상담사 하위에 예약신청 정보 저장
        CounselorAppo counselorAppo = new CounselorAppo(
              Auth_Manager.User.UserId         //내담자uid
            , seleted[1]                       //상담사uid
            , ""                               //거절사유
            , confirmWorry.text                //고민내용
            , ""                               //내담자 후기(피드백)
            , requestDay                       //상담신청 날짜
            , appDay1                          //상담날짜1 (yyyy.mm.dd.)
            , appDay2                          //상담날짜2 (yyyy년mm월dd일)
            , confirmTime.text                 //상담시간
            , 0                                //수락상태, 0:무반응 1:수락 2:거절
            , seleted[2]                       //상담사 이름
            , clientName);                     //내담자 이름


        // 데이터를 json형태로 반환
        string json = JsonUtility.ToJson(clientAppo);
        string json2 = JsonUtility.ToJson(counselorAppo);


        // 지정된 경로로 json데이터를 삽입
        reference.Child("ClientUsers")
            .Child(Auth_Manager.User.UserId)
            .Child("appointment")
            .Child(appDay2)
            .SetRawJsonValueAsync(json);

        reference.Child("CounselorUsers")
            .Child(seleted[0])
            .Child(seleted[1])
            .Child("appointment")
            .Child(appDay2)
            .SetRawJsonValueAsync(json2);


        Debug.Log("예약 완료");


        // 예약하면 appointmentcheck true로 업데이트
        Dictionary<string, object> isAppo = new Dictionary<string, object>();
        isAppo["appointmentcheck"] = true;

        reference.Child("ClientUsers")
            .Child(Auth_Manager.User.UserId)
            .UpdateChildrenAsync(isAppo);

        reference.Child("CounselorUsers")
            .Child(seleted[0])
            .Child(seleted[1])
            .UpdateChildrenAsync(isAppo);



    } // ReservationFinal() end.



    // Counselors객체와 요일리스트를 매개변수로 받아와서 상담사목록 프리팹을 생성하는 함수.
    public void CreatPrefabs(Counselors counselors, List<bool> week)
    {
        // CounselorList클래스의 대인관계전문 상담사이름의 수만큼 반복한다.
        for (int i = 0; i < counselors.counselorsUsername.Count; i++)
        {
            if (week[i])
            {
                print("리스트 저장 완료2/ " + " i=" + i + "/ 데이터 : " + counselors.counselorsUsername[i]);

                counselorClone = Instantiate(counselorPrefab, parent);   // parent위치에 counselorPrefab을 counselorClone으로 생성
                counselorCloneList.Add(counselorClone);                 // 생성된 클론프리팹을 counselorCloneList에 추가
                counselorClone.SetActive(true);


                // 텍스트 배열 newCounselorDataShort는 생성된 counselorClone프리팹의 Text타입인 자식 객체들
                newCounselorDataShort = counselorClone.GetComponentsInChildren<Text>();
                newCounselorDataShort[0].text = counselors.counselorsUsername[i];  // newCounselorDataShort[0]의 텍스트는 상담사 이름
                newCounselorDataShort[1].text = counselors.counselorsIntro[i];     // newCounselorDataShort[1]의 텍스트는 한 줄 소개
                newCounselorDataShort[2].text = counselors.major;                  // newCounselorDataShort[2]의 텍스트는 전문분야


                // 버튼 리스트 btnList에 counselorClone프리팹 하위에있는 Button(1개임) 추가
                btnList.Add(counselorClone.GetComponentInChildren<Button>());


                // 프리팹 안에 들어있는 프로필확인 버튼이 클릭 되었을때 실행 될 내용들.
                int temp = i; // i값 복사해서 사용. for문 안의 람다식에서 그대로 i를 사용하면 i가 전부 같은 값이 되어버림!!(클로저 때문에)

                // profileImg 리스트에 counselorClone 프리팹 하위에 있는 RawImage 추가
                profileImgList.Add(counselorClone.GetComponentInChildren<RawImage>());
                StartCoroutine(GetTexturePrefab(counselors.counselorsPic[temp], temp)); // 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수실행.
                print("이미지 경로 : " + counselors.counselorsPic[temp]);

                // btnLis의 temp번째 버튼이 눌렸을때(onClick) OpenReservationProfileCanvas()함수 실행(상세 프로필화면으로 이동)
                btnList[temp].onClick.AddListener(() => { GameObject.FindWithTag("CanvasMng").GetComponent<CanvasMng_Home>().OpenReservationProfileCanvas(); });

                // btnLis의 temp번째 버튼이 눌렸을때(onClick) InputData(temp)함수 실행(상세 프로필 화면에 있는 텍스트를 해당 프리팹의 데이터로 변경)
                btnList[temp].onClick.AddListener(() => { InputSeletedData(counselors, temp); });


                counselorClone.transform.position = new Vector3(1450, 367, 0);

                print(counselors.major + "상담사 프리팹 생성" + i + week[i]);

            }
            else
            {
                print(counselors.counselorsUsername + "상담사는 가능한 요일이 아닙니다" + week[i]);
            }
        }
    } // CreatPrefabs(Counselors counselors, List<bool> week) end.


    // Counselors객체를 매개변수로 받아와서 상담사목록 프리팹을 파괴하는 함수.
    public void DestroyPrefabs(Counselors counselors)
    {
        for (int i = counselors.counselorsUsername.Count - 1; i >= 0; i--)
        {
            Destroy(counselorCloneList[i]);
            counselorCloneList.Remove(counselorCloneList[i]);

            btnList.Remove(btnList[i]);

            profileImgList.Remove(profileImgList[i]);

            print(counselors.major + "제거" + i);
        }

    } // DestroyPrefabs(Counselors counselors) end.




    // 내담자 레코드 하위에 위치한 예약 레코드(appointment)
    class ClientAppo
    {
        public string counselorUid   //상담사uid
            , clientUid              //내담자uid
            , refuse                 //거절사유
            , worry                  //고민내용
            , feedback               //내담자 후기(피드백)
            , requestDay             //상담 신청 날짜
            , appDay1                //상담날짜1 (yyyy.mm.dd.)
            , appDay2                //상담날짜2 (yyyy년mm월dd일)
            , appTime                //상담시간

            , counselorName          //상담사 이름
            , counselorSex           //상담사 성별
            , clientName             //내담자 이름

            , emotionCard;           //감정카드


        public int progress;         //수락상태, 0:무반응 1:수락 2:거절


        public ClientAppo(
            string counselorUid
            , string clientUid
            , string refuse
            , string worry
            , string feedback
            , string requestDay
            , string appDay1
            , string appDay2
            , string appTime
            , int progress
            , string counselorName
            , string counselorSex
            , string clientName
            , string emotionCard)

        {
            this.counselorUid = counselorUid;
            this.clientUid = clientUid;
            this.refuse = refuse;
            this.worry = worry;
            this.feedback = feedback;
            this.requestDay = requestDay;
            this.appDay1 = appDay1;
            this.appDay2 = appDay2;
            this.appTime = appTime;
            this.progress = progress;

            this.counselorName = counselorName;
            this.counselorSex = counselorSex;
            this.clientName = clientName;

            this.emotionCard = emotionCard;

        }


    } // ClientAppo end.


    // 상담사 레코드 하위에 위치한 예약 레코드(appointment)
    class CounselorAppo
    {
        public string clientUid  //내담자uid
            , counselorUid       //상담사uid 
            , refuse             //거절사유
            , worry              //고민내용
            , feedback           //내담자 후기(피드백)
            , requestDay         //상담신청 날짜
            , appDay1            //상담날짜1 (yyyy.mm.dd.)
            , appDay2            //상담날짜2 (yyyy년mm월dd일)
            , appTime            //상담시간

            , counselorName      //상담사 이름
            , clientName;        //내담자 이름


        public int progress;     //수락상태, 0:무반응 1:수락 2:거절


        public CounselorAppo(
              string clientUid
            , string counselorUid
            , string refuse
            , string worry
            , string feedback
            , string requestDay
            , string appDay1
            , string appDay2
            , string appTime

            , int progress

            , string counselorName
            , string clientName)

        {
            this.clientUid = clientUid;
            this.counselorUid = counselorUid;
            this.refuse = refuse;
            this.worry = worry;
            this.feedback = feedback;
            this.requestDay = requestDay;
            this.appDay1 = appDay1;
            this.appDay2 = appDay2;
            this.appTime = appTime;
            this.progress = progress;

            this.counselorName = counselorName;
            this.clientName = clientName;

        }

    } // CounselorAppo end.



} // CounselorLoad end.