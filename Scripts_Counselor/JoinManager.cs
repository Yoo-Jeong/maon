using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class JoinManager : MonoBehaviour
{
    public DatabaseReference reference { get; set; }

    Firebase.Auth.FirebaseAuth auth;
    //인증을 관리할 객체
    public static FirebaseUser User;

    //  public GameObject info, lifePattern; // 페이지 전환

    public Button goLogin2;

    string userGroup;

    public Text ChEmail;
    public Text emailCheck;
    public InputField EmailInput, PasswordInput, PasswordCheckInput, NameInput, IntroInput, CareerInput1, CareerInput2, CareerInput3;
    public GameObject PasswordOK;
    public GameObject IDCheckOK, IDCheckNO, IDCheckEmail;
    private bool RegiOK;
    public bool emailOK;
    public Button IdCheck;
    public Button JoinButton;
    public Toggle Male, Female;
    public Toggle family, myself, relationship, love, job, course; // 전문 분야
    public Toggle sun, mon, tue, wed, thur, fri, sat; // 상담 가능요일
    public Toggle T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, allSelect; // 상담 가능시간
   // 입력 정보
    public string email, password, passwordCheck, username, sex, introduce, career1,career2,career3, major, counselday, counseltime;


    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
        new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        reference = FirebaseDatabase.DefaultInstance.RootReference;


        PasswordOK.SetActive(false);


        RegiOK = false;

        // 기본 값 설정
        userGroup = "상담사";
        InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크

        goLogin2.onClick.AddListener(Auth_Manager.Instance.OnAuthCanvas); //로그인 필드 활성화

       // CounselTimeToggle();



    }


    // 상담시간 전체 선택 버튼
    public void CounselTimeAll()
    {
        if (allSelect.isOn )
        {
            T1.isOn = true;
            T2.isOn = true;
            T3.isOn = true;
            T4.isOn = true;
            T5.isOn = true;
            T6.isOn = true;
            T7.isOn = true;
            T8.isOn = true;
            T9.isOn = true;
            T10.isOn = true;
        }
        else
        {
        
            T1.isOn = false;
            T2.isOn = false;
            T3.isOn = false;
            T4.isOn = false;
            T5.isOn = false;
            T6.isOn = false;
            T7.isOn = false;
            T8.isOn = false;
            T9.isOn = false;
            T10.isOn = false;
        }


    }
    // 최종 회원가입 버튼.
    public void RegisterBtn()
    {

        MajorToggle(); //전공
        CounselDayToggle(); //상담 요일
        CounselTimeToggle(); // 상담 시간

        // 회원가입

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            else
            {
                print("회원가입 성공1");

                // 회원가입 성공시
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("회원가입 성공: {0} ({1})", username, newUser.UserId);
                print("회원가입 성공2");
       

                // 상담사 기본정보
                CounselorUser counselorUser = new CounselorUser("상담사", newUser.UserId, email, username, sex,
                    introduce,  major,  career1, career2, career3,  false);

                // 상담사 예약정보
                CounselorAppo counselorAppo = new CounselorAppo("uid", "", "", "", "", "", "", 0);

                // 상담사 가능 시간
                CounselorTime counselorTime = new CounselorTime("9:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00"
                   , "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00", "17:00-18:00");

                // 상담사 가능 요일
                CounselorDay counselorDay = new CounselorDay(true, true, true, true, true, true, true);


                // 데이터를 json형태로 반환.
                string json = JsonUtility.ToJson(counselorUser);
                string json2 = JsonUtility.ToJson(counselorAppo);
                string json3 = JsonUtility.ToJson(counselorTime);
                string json4 = JsonUtility.ToJson(counselorDay);

                // root의 자식 clientUser key 값을 추가.
                string key = reference.Child("CounselorUsers").Push().Key;

                // 해당 전문 분야 하위에 상담사 데이터 저장. (전문 분야로 쿼리하기 위해..?)
                if (counselorUser.major == "가족")
                {
                    reference.Child("CounselorUsers").Child("가족").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("가족").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("가족").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("가족").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);


                }
                else if (counselorUser.major == "나 자신")
                {
                    reference.Child("CounselorUsers").Child("나자신").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("나자신").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("나자신").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("나자신").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);
                }
                else if (counselorUser.major == "대인관계")
                {
                    reference.Child("CounselorUsers").Child("대인관계").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("대인관계").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("대인관계").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("대인관계").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);

                }
                else if (counselorUser.major == "연애")
                {
                    reference.Child("CounselorUsers").Child("연애").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("연애").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("연애").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("연애").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);
                }
                else if (counselorUser.major == "직장")
                {
                    reference.Child("CounselorUsers").Child("직장").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("직장").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("직장").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("직장").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);
                }
                else
                {
                    reference.Child("CounselorUsers").Child("진로취업").Child(newUser.UserId).SetRawJsonValueAsync(json);

                    reference.Child("CounselorUsers").Child("진로취업").Child(newUser.UserId).Child("appointment").Child(key).SetRawJsonValueAsync(json2);

                    reference.Child("CounselorUsers").Child("진로취업").Child(newUser.UserId).Child("counselorTime").SetRawJsonValueAsync(json3);

                    reference.Child("CounselorUsers").Child("진로취업").Child(newUser.UserId).Child("counselorDay").SetRawJsonValueAsync(json4);
                }

                print(RegiOK);
                RegiOK = true;
                print(RegiOK);

            }
            //RegiterNext();
            //RegiCheck();

        });

        Invoke("RegiCheck", 2);


        /*if (RegiOK == true)
        {
            print("테스트2");
            SceneManager.LoadScene("Scenes/LogIn_Scene");  //회원가입에 성공했을 때 씬이 넘어가고 싶은데 작동 안함
            print("테스트3");
        }*/
    }
    //회원가입이 완료됐는지 확인 기다리기위한 함수.
    public void RegiCheck()
    {
        if (RegiOK == true)
        {
            print("테스트2");
            SceneManager.LoadScene("Scenes/SignIn_Scene");  //회원가입에 성공했을 때 씬이 넘어가고 싶은데 작동 안함
            print("테스트3");

            print(RegiOK);
            RegiOK = false;
            print(RegiOK);
        }
    }
    // 비밀번호 일치 확인 함수
    public void PasswordCheck()
    {
        password = PasswordInput.text;
        passwordCheck = PasswordCheckInput.text;

        if ((password != passwordCheck) || (password == ""))
        {
            PasswordOK.SetActive(false);
           // print("비밀번호가 일치하지 않습니다.");

        }
        else
        {
            PasswordOK.SetActive(true);
            //  print("비밀번호가 일치합니다.");
        }
    }
    // 기본정보 -> 생활패턴 다음 버튼
    public void RegiterNext()
    {
        email = EmailInput.text;
        password = PasswordInput.text;
        passwordCheck = PasswordCheckInput.text;
        introduce = IntroInput.text;
        username = NameInput.text;
        career1 = CareerInput1.text;
        career2 = CareerInput2.text;
        career3 = CareerInput3.text;
        MajorToggle();
        if (Male.isOn)
        {
            sex = Male.GetComponentInChildren<Text>().text;
        }
        else
        {
            sex = Female.GetComponentInChildren<Text>().text;
        }




        if ((password == passwordCheck && password != ""))
        {
          //  print("비밀번호가 일치합니다.");

            PasswordOK.SetActive(true);
            // info.SetActive(false);
            // lifePattern.SetActive(true);

        }
 
     
    }
    // 생활패턴 -> 기본정보 이전버튼
    public void RegiterBefore()

    {
       CounselDayToggle();
       CounselTimeToggle();
    }
    //ID 중복체크 후 팝업띄우는 함수
    public void IDCheck()
    {
        string userexception = "The password is invalid or the user does not have a password.";
        string userexception2 = "The email address is badly formatted.";
        string userexception3 = "An email address must be provided.";
       // string userexception4 = "There is no user record corresponding to this identifier.";

        string errorcheck;


        auth.SignInWithEmailAndPasswordAsync(ChEmail.text, " ")  // 로그인 진행 , 비밀번호 임의값 추가
                 .ContinueWithOnMainThread(task =>
                 {
                     //Debug.Log($"Sign in status : {task.Status}");


                     if (task.IsFaulted)
                     {
                         Debug.LogError(task.Exception);
                         errorcheck = task.Exception.ToString();
                         if (errorcheck.Contains(userexception))
                         {
                             Debug.Log("이미 존재하는 아이디 입니다.");
                             IDCheckNO.SetActive(true);
                         }
                         else if (errorcheck.Contains(userexception2))
                         {
                             Debug.Log("이메일 형식이 올바르지 않습니다.");
                             IDCheckEmail.SetActive(true);
                         }
                         else if (errorcheck.Contains(userexception3))
                         {
                             Debug.Log("아이디를 입력 해 주세요");
                             IDCheckEmail.SetActive(true);
                         }
                         //else if (errorcheck.Contains(userexception4))
                         //{
                         //    Debug.Log("존재하지 않는 이메일 입니다");
                         //    IDCheckEmail.SetActive(true);
                         //}
                         else
                         {
                             Debug.Log("사용 가능한 아이디 입니다.");
                             IDCheckOK.SetActive(true);
                         }
                     }
                     else if (task.IsCanceled)
                     {
                         Debug.LogError("로그인 실패");
                         // LogText.text = "취소되었습니다.";
                     }

                 });


     
        


    }

    // 전문분야 토글 텍스트 값을 받아오는 함수.
    void MajorToggle()
    {
        if (family.isOn)
        {
           major = family.GetComponentInChildren<Text>().text;

        }
        else if (myself.isOn)
        {
            major = myself.GetComponentInChildren<Text>().text;
        }
        else if (relationship.isOn)
        {
            major = relationship.GetComponentInChildren<Text>().text;
        }
        else if (love.isOn)
        {
            major = love.GetComponentInChildren<Text>().text;
        }
        else if (job.isOn)
        {
            major = job.GetComponentInChildren<Text>().text;
        }
        else
        {
            major = course.GetComponentInChildren<Text>().text;
        }
    }

    

    // 상담가능 요일 토글 텍스트 값을 받아오는 함수.
    void CounselDayToggle()
    {
        if (sun.isOn)
        {
            sun.isOn = true;
            counselday = sun.GetComponentInChildren<Text>().text;
        }
        else if (mon.isOn)
        {
            mon.isOn = true;
            counselday = mon.GetComponentInChildren<Text>().text;
        }
        else if (tue.isOn)
        {
            tue.isOn = true;
            counselday = tue.GetComponentInChildren<Text>().text;
        }
        else if (wed.isOn)
        {
            wed.isOn = true;
            counselday = wed.GetComponentInChildren<Text>().text;
        }
        else if (thur.isOn)
        {
            thur.isOn = true;
            counselday = thur.GetComponentInChildren<Text>().text;
        }
        else if (fri.isOn)
        {
            fri.isOn = true;
            counselday = fri.GetComponentInChildren<Text>().text;
        }
        else
        {
            sat.isOn = true;
            counselday = sat.GetComponentInChildren<Text>().text;
        }
    }

    // 상담 가능 시간 토글의 텍스트를 받아오는 함수
    void CounselTimeToggle()
    {
        if (T1.isOn)
        {
            counseltime = T1.GetComponentInChildren<Text>().text;
        }
        if (T2.isOn)
        {
            counseltime = T2.GetComponentInChildren<Text>().text;
        }
        if (T3.isOn)
        {
            counseltime = T3.GetComponentInChildren<Text>().text;
        }
        if (T4.isOn)
        {
            counseltime = T4.GetComponentInChildren<Text>().text;
        }
        if (T5.isOn)
        {
            counseltime = T5.GetComponentInChildren<Text>().text;
        }
        if (T6.isOn)
        {
            counseltime = T6.GetComponentInChildren<Text>().text;
        }
        if (T7.isOn)
        {
            counseltime = T7.GetComponentInChildren<Text>().text;
        }
        if (T8.isOn)
        {
            counseltime = T8.GetComponentInChildren<Text>().text;
        }
        if (T9.isOn)
        {
            counseltime = T9.GetComponentInChildren<Text>().text;
        }
        if (T10.isOn)
        {
            counseltime = T10.GetComponentInChildren<Text>().text;
        }

    }

   


    // 회원가입 버튼 함수(DBtest)
    class CounselorUser
    {
        // 기본 정보 : 상담사그룹, 이메일, 프로필이미지 경로, 이름, 성별, 한줄소개, 경력사항1, 경력사항2, 경력사항3, 상담가능요일, 상담가능시간
        public string userGroup, uid, email, username, sex, intro, career1, career2, career3, major;

        // 전문 분야 : 가족, 나 자신, 대인관계, 연애, 직장, 진로/취직
        //public bool family, myself, relationship, romance, work, career;

        public bool appointment;  // 예약여부




        // 상담사 생성자.
        public CounselorUser(string userGroup, string uid, string email,  string username, string sex, string intro, string major, string career1, string career2, string career3,  bool appointment)
        {
            this.userGroup = userGroup;
            this.uid = uid;
            this.email = email;
            this.username = username;
            this.sex = sex;
            this.intro = intro;

            this.major = major;

            this.career1 = career1;
            this.career2 = career2;
            this.career3 = career3;

            //this.family = family;
            //this.myself = myself;
            //this.relationship = relationship;
            //this.romance = romance;
            //this.work = work;
            //this.career = career;






            this.appointment = appointment;

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

    }

    // 상담사 레코드 하위에 위치한 가능예약 시간레코드(time)
    class Counselortime
    {
        public string nine, ten, eleven, twelve, thirteen, fourteen , fifteen, sixteen, seventeen;

        public Counselortime(string nine, string ten, string eleven, string twelve, string thirteen, string fourteen
            , string fifteen, string sixteen, string seventeen)
        {
            this.nine = nine;
            this.ten = ten;
            this.eleven = eleven;
            this.twelve = twelve;
            this.thirteen = thirteen;
            this.fourteen = fourteen;
            this.fifteen = fifteen;
            this.sixteen = sixteen;
            this.seventeen = seventeen;

        }

    }

    // 상담사 레코드 하위에 위치한 가능예약 요일레코드(day)
    class Counselorday
    {
        public bool Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday;

        public Counselorday(bool Monday, bool Tuesday, bool Wednesday
            , bool Thursday, bool Friday, bool Saturday, bool Sunday)
        {
            this.Monday = Monday;
            this.Tuesday = Tuesday;
            this.Wednesday = Wednesday;
            this.Thursday = Thursday;
            this.Friday = Friday;
            this.Saturday = Saturday;
            this.Sunday = Sunday;

        }

    }
}

   



