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
    public Text calendarMonth;            // 달력의 달을 표시한 Text타입 변수(선택날짜를 알기 위해 필요)

    public static string appDay1;         // 선택한 날짜를 담을 변수
    public string appDay2;                //RDB저장 경로를 위한 날짜를 담을 변수


    // 전문분야 선택 토글 관련 변수들 선언.
    public ToggleGroup majorToggleGroup;
    public Toggle familyToggle, myselfToggle, relationshipToggle, loveToggle, jobToggle, courseToggle; // 전문분야 토글
    private Text familyText, myselfText, relationshipText, loveText, jobText, courseText;              // 전문분야 토글의 자식 텍스트


    // 전문분야에 따라 상담사의 정보를 저장할 Counselors타입 변수 선언.
    public Counselors family;           //전문분야가 가족인 상담사들
    public Counselors myself;           //전문분야가 나 자신인 상담사들
    public Counselors relationship;     //전문분야가 대인관계인 상담사들
    public Counselors love;             //전문분야가 연애인 상담사들
    public Counselors job;              //전문분야가 직장인 상담사들
    public Counselors course;           //전문분야가 진로/취업인 상담사들


    // 상담사 프리팹 관련 변수들 선언.
    public Transform parent;                                                // 상담사 프리팹이 생성될 위치의 부모객체의 위치.(parent의 자식으로 프리팹생성)
    public GameObject counselorPrefab, counselorClone;                      // 상담사 프리팹, 복제된 상담사 프리팹

    public List<GameObject> counselorCloneList = new List<GameObject>();    // 복제된 프리팹들을(counselorClone) 담을 리스트

    public List<RawImage> profileImgList = new List<RawImage>();            // counselorClone프리팹들의 자식RawImage를 담을 리스트
    public Texture baseTexture;                                             // 상담사 프로필 이미지가 없을때 적용할 텍스쳐
    public List<Button> btnList = new List<Button>();                       // 복제된 프리팹들의 자식 버튼을 담을 리스트

    public Text[] newCounselorDataShort;                                    // 복제된 프리팹의 자식 Text타입 게임오브젝트를 담을 배열


    public GameObject toggleParent;
    // 시간 선택 토글 변수들 선언.
    public ToggleGroup timeToggleGroup;
    public static Toggle[] timeToggle = new Toggle[9];


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

    public Button reservationBtn;
 

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        family = new Counselors("가족");
        myself = new Counselors("나자신");
        relationship = new Counselors("대인관계");
        love = new Counselors("연애");
        job = new Counselors("직장");
        course = new Counselors("진로취업");


        InitUI(); //ui 초기화
        SetFunction_UI();


      

    } // Start().

    /// <summary>
    /// 전문분야 토글이 켜져있으면 끄는 함수.
    /// </summary>
    public void CleanToggle()
    {
        if (majorToggleGroup.AnyTogglesOn())
        {
            majorToggleGroup.SetAllTogglesOff();
        }
    }

    /// <summary>
    /// 전문분야 토글이 켜져있으면 끄는 함수.
    /// </summary>
    public void CleanTimeToggle()
    {
        if (timeToggleGroup.AnyTogglesOn())
        {
            timeToggleGroup.SetAllTogglesOff();
        }
    }

    /// <summary>
    /// 전문분야 string과 Counselors클래스를 매개변수로 받아서 가능한 요일에따라 상담사 프리팹을 생성하는 함수.
    /// </summary>
    /// <param name="majorString"></param>
    /// <param name="majorClass"></param>
    public void CreatMajor(string majorString, Counselors majorClass)
    {
        Debug.Log(majorString + " 선택 : ");

        if (majorClass != null)
        {

            if (Calendar_Reservation.dayOfWeek == 1)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":월요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Monday);

            }
            else if (Calendar_Reservation.dayOfWeek == 2)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":화요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Tuesday);
            }
            else if (Calendar_Reservation.dayOfWeek == 3)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":수요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Wednesday);
            }
            else if (Calendar_Reservation.dayOfWeek == 4)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":목요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Thursday);
            }
            else if (Calendar_Reservation.dayOfWeek == 5)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":금요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Friday);
            }
            else if (Calendar_Reservation.dayOfWeek == 6)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":토요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Saturday);
            }
            else if (Calendar_Reservation.dayOfWeek == 0)
            {
                Debug.Log(Calendar_Reservation.dayOfWeek + ":일요일 / " + majorString);
                CreatPrefabs(majorClass, majorClass.Sunday);
            }
            else
            {
                Debug.Log(majorString + " 전문분야 상담사가 없습니다.");
            }

        }
        else
        {
            Debug.Log("상담사 클래스 매개변수 null");
        }

    }


    /// <summary>
    /// 가족 토글On 하는 함수.
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_familyToggle(bool _bool)
    {
        if (_bool == true)
        {
            familyText.color = Color.white;     // 토글 텍스트 색 변경.
            familyText.text = "<b>가족</b>";    // 토글 텍스트 볼드처리.

            CreatMajor("가족", family);

        }
        else
        {
            familyText.color = Color.black;
            familyText.text = "가족";

            DestroyPrefabs(family);

        }


    } // Function_familyToggle(bool _bool).


    /// <summary>
    /// 나 자신 토글On 하는 함수.
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_myselfToggle(bool _bool)
    {
        if (_bool == true)
        {
            myselfText.color = Color.white;         // 토글 텍스트 색 변경.
            myselfText.text = "<b>나 자신</b>";     // 토글 텍스트 볼드처리.

            CreatMajor("나 자신", myself);
        }
        else
        {
            myselfText.color = Color.black;
            myselfText.text = "나 자신";

            DestroyPrefabs(myself);
        }

    } // Function_myselfToggle(bool _bool).


    /// <summary>
    /// 대인관계 토글On
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_RelationshipToggle(bool _bool)
    {
        if (_bool == true)
        {

            relationshipText.color = Color.white;     // 토글 텍스트 색 변경.
            relationshipText.text = "<b>대인관계</b>"; // 토글 텍스트 볼드처리.

            CreatMajor("대인관계", relationship);
           
        }
        else
        {
            relationshipText.color = Color.black;
            relationshipText.text = "대인관계";

            DestroyPrefabs(relationship);  

        }

    } // Function_RelationshipToggle(bool _bool).


    /// <summary>
    /// 연애 토글On 하는 함수.
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_loveToggle(bool _bool)
    {
        if (_bool == true)
        {
            loveText.color = Color.white;     // 토글 텍스트 색 변경.
            loveText.text = "<b>연애</b>"; // 토글 텍스트 볼드처리.

            CreatMajor("연애", love);
        }
        else
        {
            loveText.color = Color.black;
            loveText.text = "연애";

            DestroyPrefabs(love);
        }

    } // Function_loveToggle(bool _bool).


    /// <summary>
    /// 직장 토글On 하는 함수.
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_jobToggle(bool _bool)
    {
        if (_bool == true)
        {
            jobText.color = Color.white;     // 토글 텍스트 색 변경.
            jobText.text = "<b>직장</b>";    // 토글 텍스트 볼드처리.

            CreatMajor("직장", job);
        }
        else
        {
            jobText.color = Color.black;
            jobText.text = "직장";

            DestroyPrefabs(job);
        }

    } // Function_jobToggle(bool _bool).


    /// <summary>
    /// 진로/취업 토글On 하는 함수.
    /// </summary>
    /// <param name="_bool"></param>
    public void Function_courseToggle(bool _bool)
    {
        if (_bool == true)
        {
            courseText.color = Color.white;     // 토글 텍스트 색 변경.
            courseText.text = "<b>진로/취업</b>";    // 토글 텍스트 볼드처리.

            CreatMajor("진로/취업", course);
        }
        else
        {
            courseText.color = Color.black;
            courseText.text = "진로/취업";

            DestroyPrefabs(course);
        }

    } // Function_courseToggle(bool _bool).


    /// <summary>
    /// Counselors타입 변수counselors와 int타입 변수num을 받아와 
    /// 선택된 상담사의 정보를 저장하고 상세 프로필 화면에 있는 텍스트값을 바꾸는 함수.
    /// </summary>
    /// <param name="counselors"></param>
    /// <param name="num"></param>
    public void InputSeletedData(Counselors counselors, int num)
    {
        print("프로필 확인 버튼 클릭");
   
        clientName = User_Data.username; //내담자(나)의 이름 저장.

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

        SelectDay();

        counselors.LoadCounselorTime(counselors.major, seleted[1]);

    } // InputSeletedData(Counselors counselors, int num) end.


    /// <summary>
    /// 선택된 상담사의 프로필이미지(ReservationProfileCanvas): 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            seletedProfileImg.texture = baseTexture;
        }
        else
        {
            seletedProfileImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    /// <summary>
    /// 프리팹용(counselorClone) : 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    IEnumerator GetTexturePrefab(string url, int num)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
          /*  if (num <= profileImgList.Count)
            {
                profileImgList[num].texture = baseTexture;
            }*/
        }
        else
        {
            if (num <= profileImgList.Count)
            {
                profileImgList[num].texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }

        }
    }



    public void InitUI()
    {
        /*    nine = toggleParent.transform.GetChild(0).GetComponent<Toggle>();      
            ten = toggleParent.transform.GetChild(1).GetComponent<Toggle>();
            eleven = toggleParent.transform.GetChild(2).GetComponent<Toggle>();
            twelve = toggleParent.transform.GetChild(3).GetComponent<Toggle>();
            thirteen = toggleParent.transform.GetChild(4).GetComponent<Toggle>();
            fourteen = toggleParent.transform.GetChild(5).GetComponent<Toggle>();
            fifteen = toggleParent.transform.GetChild(6).GetComponent<Toggle>();
            sixteen = toggleParent.transform.GetChild(7).GetComponent<Toggle>();
            seventeen = toggleParent.transform.GetChild(8).GetComponent<Toggle>();*/

        for (int i = 0; i < timeToggle.Length; i++)
        {
            int tmep = i;
            timeToggle[tmep] = toggleParent.transform.GetChild(tmep).GetComponent<Toggle>();    
        }

        // 토글 텍스트 색 변경을 위해 컴포넌트를 가져옴
        familyText = familyToggle.GetComponentInChildren<Text>();
        myselfText = myselfToggle.GetComponentInChildren<Text>();
        relationshipText = relationshipToggle.GetComponentInChildren<Text>();
        loveText = loveToggle.GetComponentInChildren<Text>();
        jobText = jobToggle.GetComponentInChildren<Text>();
        courseText = courseToggle.GetComponentInChildren<Text>();

        familyText.color = Color.black; // 초기 토글 텍스트 색
        myselfText.color = Color.black;
        relationshipText.color = Color.black;
        loveText.color = Color.black;
        jobText.color = Color.black;
        courseText.color = Color.black;

        nineText = timeToggle[0].GetComponentInChildren<Text>();
        tenText = timeToggle[1].GetComponentInChildren<Text>();
        elevenText = timeToggle[2].GetComponentInChildren<Text>();
        twelveText = timeToggle[3].GetComponentInChildren<Text>();
        thirteenText = timeToggle[4].GetComponentInChildren<Text>();
        fourteenText = timeToggle[5].GetComponentInChildren<Text>();
        fifteenText = timeToggle[6].GetComponentInChildren<Text>();
        sixteenText = timeToggle[7].GetComponentInChildren<Text>();
        seventeenText = timeToggle[8].GetComponentInChildren<Text>();

        nineText.color = Color.black;
        tenText.color = Color.black;
        elevenText.color = Color.black;
        twelveText.color = Color.black;
        thirteenText.color = Color.black;
        fourteenText.color = Color.black;
        fifteenText.color = Color.black;
        sixteenText.color = Color.black;
        seventeenText.color = Color.black;

    }


    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();

        familyToggle.onValueChanged.AddListener(Function_familyToggle);
        myselfToggle.onValueChanged.AddListener(Function_myselfToggle);
        relationshipToggle.onValueChanged.AddListener(Function_RelationshipToggle);
        loveToggle.onValueChanged.AddListener(Function_loveToggle);
        jobToggle.onValueChanged.AddListener(Function_jobToggle);
        courseToggle.onValueChanged.AddListener(Function_courseToggle);

        /*
                nine.onValueChanged.AddListener(nine_Toggle);
                ten.onValueChanged.AddListener(ten_Toggle);
                eleven.onValueChanged.AddListener(eleven_Toggle);
                twelve.onValueChanged.AddListener(twelve_Toggle);
                thirteen.onValueChanged.AddListener(thirteen_Toggle);
                fourteen.onValueChanged.AddListener(fourteen_Toggle);
                fifteen.onValueChanged.AddListener(fifteen_Toggle);
                sixteen.onValueChanged.AddListener(sixteen_Toggle);
                seventeen.onValueChanged.AddListener(seventeen_Toggle);*/

        timeToggle[0].onValueChanged.AddListener(nine_Toggle);
        timeToggle[1].onValueChanged.AddListener(ten_Toggle);
        timeToggle[2].onValueChanged.AddListener(eleven_Toggle);
        timeToggle[3].onValueChanged.AddListener(twelve_Toggle);
        timeToggle[4].onValueChanged.AddListener(thirteen_Toggle);
        timeToggle[5].onValueChanged.AddListener(fourteen_Toggle);
        timeToggle[6].onValueChanged.AddListener(fifteen_Toggle);
        timeToggle[7].onValueChanged.AddListener(sixteen_Toggle);
        timeToggle[8].onValueChanged.AddListener(seventeen_Toggle);

    }

    private void ResetFunction_UI()
    {
        familyToggle.onValueChanged.RemoveAllListeners();
        myselfToggle.onValueChanged.RemoveAllListeners();
        relationshipToggle.onValueChanged.RemoveAllListeners();
        loveToggle.onValueChanged.RemoveAllListeners();
        jobToggle.onValueChanged.RemoveAllListeners();
        courseToggle.onValueChanged.RemoveAllListeners();

        /*  nine.onValueChanged.RemoveAllListeners();
          ten.onValueChanged.RemoveAllListeners();
          eleven.onValueChanged.RemoveAllListeners();
          twelve.onValueChanged.RemoveAllListeners();
          thirteen.onValueChanged.RemoveAllListeners();
          fourteen.onValueChanged.RemoveAllListeners();
          fifteen.onValueChanged.RemoveAllListeners();
          sixteen.onValueChanged.RemoveAllListeners();
          seventeen.onValueChanged.RemoveAllListeners();*/

        timeToggle[0].onValueChanged.RemoveAllListeners();
        timeToggle[1].onValueChanged.RemoveAllListeners();
        timeToggle[2].onValueChanged.RemoveAllListeners();
        timeToggle[3].onValueChanged.RemoveAllListeners();
        timeToggle[4].onValueChanged.RemoveAllListeners();
        timeToggle[5].onValueChanged.RemoveAllListeners();
        timeToggle[6].onValueChanged.RemoveAllListeners();
        timeToggle[7].onValueChanged.RemoveAllListeners();
        timeToggle[8].onValueChanged.RemoveAllListeners();
    }


    /// <summary>
    /// 선택한 날짜를 저장하는 함수.
    /// </summary>
    public void SelectDay()
    {
        string seletedMonth = calendarMonth.text;    //선택한 날짜의 달을 담을 string타입 변수
        string day = DayPrefab.thisDay;

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
        if (day.Length < 2)
        {
            day = day.Insert(0, "0");
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



    private Text nineText, tenText, elevenText, twelveText, thirteenText, fourteenText, fifteenText, sixteenText, seventeenText;
    private void nine_Toggle(bool _bool)
    {
        Debug.Log("9시 - 10시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[0].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[0].GetComponentInChildren<Text>().text;
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
            print(timeToggle[1].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[1].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            tenText.color = Color.white;
            tenText.text = "<b>10:00 - 11:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            tenText.color = Color.black;
            tenText.text = "10:00 - 11:00";
        }
    }

    private void eleven_Toggle(bool _bool)
    {
        Debug.Log("11시 - 12시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[2].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[2].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            elevenText.color = Color.white;
            elevenText.text = "<b>11:00 - 12:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            elevenText.color = Color.black;
            elevenText.text = "11:00 - 12:00";
        }
    }

    private void twelve_Toggle(bool _bool)
    {
        Debug.Log("12시 - 13시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[3].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[3].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            twelveText.color = Color.white;
            twelveText.text = "<b>12:00 - 13:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            twelveText.color = Color.black;
            twelveText.text = "12:00 - 13:00";
        }
    }

    private void thirteen_Toggle(bool _bool)
    {
        Debug.Log("13시 - 14시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[4].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[4].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            thirteenText.color = Color.white;
            thirteenText.text = "<b>13:00 - 14:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            thirteenText.color = Color.black;
            thirteenText.text = "13:00 - 14:00";
        }
    }

    private void fourteen_Toggle(bool _bool)
    {
        Debug.Log("14시 - 15시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[5].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[5].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            fourteenText.color = Color.white;
            fourteenText.text = "<b>14:00 - 15:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            fourteenText.color = Color.black;
            fourteenText.text = "14:00 - 15:00";
        }
    }


    private void fifteen_Toggle(bool _bool)
    {
        Debug.Log("15시 - 16시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[6].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[6].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            fifteenText.color = Color.white;
            fifteenText.text = "<b>15:00 - 16:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            fifteenText.color = Color.black;
            fifteenText.text = "15:00 - 16:00";
        }
    }

    private void sixteen_Toggle(bool _bool)
    {
        Debug.Log("16시 - 17시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[7].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[7].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            sixteenText.color = Color.white;
            sixteenText.text = "<b>16:00 - 17:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            sixteenText.color = Color.black;
            sixteenText.text = "16:00 - 17:00";
        }
    }

    private void seventeen_Toggle(bool _bool)
    {
        Debug.Log("17시 - 18시 : " + _bool);

        if (_bool == true)
        {
            print(timeToggle[8].GetComponentInChildren<Text>().text);
            selectedTime.text = timeToggle[8].GetComponentInChildren<Text>().text;
            selectedTime.color = Color.black;

            // 토글 텍스트 색 변경
            seventeenText.color = Color.white;
            seventeenText.text = "<b>17:00 - 18:00</b>";
        }
        else
        {
            selectedTime.text = "시간을 선택하세요.";
            selectedTime.color = Color.grey;

            // 토글 텍스트 색 변경
            seventeenText.color = Color.black;
            seventeenText.text = "17:00 - 18:00";
        }
    }

    /// <summary>
    /// 첫번째 예약신청 버튼을 누르면 실행되는 함수. (팝업창 텍스트 변경)
    /// </summary> 
    public void ReservationTryBtn()
    {
        confirmDay.text = appDay1;
        confirmTime.text = selectedTime.text;
        confirmWorry.text = worryInput.text;

    }

    /// <summary>
    /// 팝업창 최종 예약하기 버튼을 누르면 실행되는 함수.
    /// </summary>
    public void ReservationFinalBtn()
    {
        //예약일자 저장(오늘 날짜)
        requestDay = DateTime.Now.ToString("yyyy.MM.dd");

        print("최종 예약하기 버튼 클릭" + " / 예약 일자:" + requestDay);

        // RDB에 내담자 데이터 저장
        ClientAppo clientAppo = new ClientAppo(
              seleted[1]                       //상담사uid
            , Auth_Manager.user.UserId         //내담자uid
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
            , seletedMajorText.text            //상담사 전문분야
            , clientName                       //내담자 이름
            , ""                               //감정카드1                           
            , ""                               //감정카드2     
            , ""                               //감정카드3     
            , ""                               //욕구카드1     
            , ""                               //욕구카드2   
            , ""                               //욕구카드3
            , "");                             //감정일기


        // 상담사 하위에 예약신청 정보 저장
        CounselorAppo counselorAppo = new CounselorAppo(
              Auth_Manager.user.UserId         //내담자uid
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
            .Child(Auth_Manager.user.UserId)
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


        /*   // 예약하면 appointmentcheck true로 업데이트
           Dictionary<string, object> isAppo = new Dictionary<string, object>();
           isAppo["appointmentcheck"] = true;

           reference.Child("ClientUsers")
               .Child(Auth_Manager.user.UserId)
               .UpdateChildrenAsync(isAppo);

           reference.Child("CounselorUsers")
               .Child(seleted[0])
               .Child(seleted[1])
               .UpdateChildrenAsync(isAppo);*/



    } // ReservationFinal() end.


    /// <summary>
    /// Counselors객체와 요일리스트를 매개변수로 받아와서 상담사목록 프리팹을 생성하는 함수.
    /// </summary>
    /// <param name="counselors"></param>
    /// <param name="week"></param>
    public void CreatPrefabs(Counselors counselors, List<bool> week)
    {
        if (counselors != null)
        {
            // CounselorList클래스의 대인관계전문 상담사이름의 수만큼 반복한다.
            for (int i = 0; i < counselors.counselorsUsername.Count; i++)
            {
                int temp = i; // i값 복사해서 사용. for문 안의 람다식에서 그대로 i를 사용하면 i가 전부 같은 값이 되어버림!!(클로저 때문에)

                counselorClone = Instantiate(counselorPrefab, parent);   // parent위치에 counselorPrefab을 counselorClone으로 생성
                counselorCloneList.Add(counselorClone);                 // 생성된 클론프리팹을 counselorCloneList에 추가

                // 버튼 리스트 btnList에 counselorClone프리팹 하위에있는 Button(1개임) 추가
                btnList.Add(counselorClone.GetComponentInChildren<Button>());

                // profileImg 리스트에 counselorClone 프리팹 하위에 있는 RawImage 추가
                profileImgList.Add(counselorClone.GetComponentInChildren<RawImage>());


                if (week[i])
                {
                    print("리스트 저장 완료2/ " + " i=" + i + "/ 데이터 : " + counselors.counselorsUsername[i]);

                    counselorCloneList[temp].SetActive(true);

                    // 텍스트 배열 newCounselorDataShort는 생성된 counselorClone프리팹의 Text타입인 자식 객체들
                    newCounselorDataShort = counselorClone.GetComponentsInChildren<Text>();
                    newCounselorDataShort[0].text = counselors.counselorsUsername[i];  // newCounselorDataShort[0]의 텍스트는 상담사 이름
                    newCounselorDataShort[1].text = counselors.counselorsIntro[i];     // newCounselorDataShort[1]의 텍스트는 한 줄 소개
                    newCounselorDataShort[2].text = counselors.major;                  // newCounselorDataShort[2]의 텍스트는 전문분야

                    StartCoroutine(GetTexturePrefab(counselors.counselorsPic[temp], temp)); // 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수실행.
                    Debug.Log("이미지 경로 : " + counselors.counselorsPic[temp]);

                    // btnLis의 temp번째 버튼이 눌렸을때(onClick) OpenReservationProfileCanvas()함수 실행(상세 프로필화면으로 이동)
                    btnList[temp].onClick.AddListener(() => { GameObject.FindWithTag("CanvasMng").GetComponent<CanvasMng_Home>().OpenReservationProfileCanvas(); });

                    // btnLis의 temp번째 버튼이 눌렸을때(onClick) InputData(temp)함수 실행(상세 프로필 화면에 있는 텍스트를 해당 프리팹의 데이터로 변경)
                    btnList[temp].onClick.AddListener(() => { InputSeletedData(counselors, temp); });


                    counselorClone.transform.position = new Vector3(1450, 367, 0);

                    print(counselors.major + "상담사 프리팹 생성 / " + i + week[i]);

                }
                else
                {
                    counselorCloneList[temp].SetActive(false);
                    print(counselors.counselorsUsername[i] + " 상담사는 가능한 요일이 아닙니다 / " + week[i]);
                }
            }
        }
        else
        {
            Debug.Log(counselors.major + " 전문분야 상담사가 없습니다.");
        }
    } // CreatPrefabs(Counselors counselors, List<bool> week) end.


    /// <summary>
    /// Counselors객체를 매개변수로 받아와서 상담사목록 프리팹을 파괴하는 함수.
    /// </summary>
    /// <param name="counselors"></param>
    public void DestroyPrefabs(Counselors counselors)
    {
        if (counselors != null)
        {
            for (int i = counselorCloneList.Count - 1; i >= 0; i--)
            {
                int temp = i;

                counselorCloneList[temp].SetActive(true);

                Destroy(counselorCloneList[temp]);
                counselorCloneList.Remove(counselorCloneList[temp]);


                Destroy(btnList[temp]);
                btnList.Remove(btnList[temp]);

                Destroy(profileImgList[temp]);
                profileImgList.Remove(profileImgList[temp]);

                print(counselors.major + "제거" + temp);
            }
        }
        else
        {
            Debug.Log("널..");
        }
    } // DestroyPrefabs(Counselors counselors) end.



    /// <summary>
    /// 내담자 레코드 하위에 위치한 예약 레코드(appointment)
    /// </summary>
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
            , counselorMajor         //상담사 전문분야
            , clientName             //내담자 이름

            , emotion1               //감정카드1
            , emotion2               //감정카드2
            , emotion3               //감정카드3
            , need1                  //욕구카드1
            , need2                  //욕구카드2
            , need3                 //욕구카드3
            , diary;                  //감정일기


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
            , string counselorMajor
            , string clientName
            , string emotion1
            , string emotion2
            , string emotion3
            , string need1
            , string need2
            , string need3
            , string diary)

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
            this.counselorMajor = counselorMajor;
            this.clientName = clientName;

            this.emotion1 = emotion1;
            this.emotion2 = emotion2;
            this.emotion3 = emotion3;

            this.need1 = need1;
            this.need2 = need2;
            this.need3 = need3;

            this.diary = diary;

        }


    } // ClientAppo end.

    /// <summary>
    /// 상담사 레코드 하위에 위치한 예약 레코드(appointment)
    /// </summary> 
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