using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class JoinManager : MonoBehaviour
{
    public DatabaseReference reference { get; set; }

    Firebase.Auth.FirebaseAuth auth;


  //  public GameObject info, lifePattern; // 페이지 전환

    string userGroup;

    public InputField EmailInput, PasswordInput, PasswordCheckInput, NameInput, IntroInput, CareerInput;
    public Text emailCheck;
    public GameObject PasswordOK;
    public Toggle Male, Female;
    public bool emailOK;
    public Button JoinButton;

    public Toggle family, myself, relationship, love, job, course; // 전문 분야
    public Toggle sun, mon, tue, wed, thur, fri, sat; // 상담 가능요일
    public Toggle T1, T2,  T3, T4, T5, T6, T7, T8, T9, T10; // 상담 가능시간
    public Toggle AllSelect; // 상담 시간 전체 선택

    // 입력 정보
    public string email, password, passwordCheck, username, sex, introduce, career, major, counselday, counseltime;

    private bool RegiOK;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
        new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        reference = FirebaseDatabase.DefaultInstance.RootReference;

      //  info.SetActive(true);
       // lifePattern.SetActive(false);
       // SetFunction_UI();
        PasswordOK.SetActive(false);
     //   SetDropdowonOptions();

        RegiOK = false;

        // 기본 값 설정
        userGroup = "상담사";
        
        InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크

    }


    // 최종 회원가입 버튼.
    public void RegisterBtn()
    {

        MajorToggle(); //전공
        CounselDayToggle(); //상담 요일
        CounselTimeToggle(); // 상담 시간

        // 회원가입
        // Async가 완료된것을 확인 할 수 있는 방법 찾아보기.
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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

                // 회원가입 성공시
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("회원가입 성공: {0} ({1})", name, newUser.UserId);

               /* print(RegiOK);
                RegiOK = true;
                print(RegiOK);

                // RDB에 상담사 데이터 저장
                CounselUser counselUser = new CounselUser(userGroup, email, username, sex, introduce, career, major, counselday, counseltime,
                   false, "", "", "", "");

                // 데이터를 json형태로 반환
                string json = JsonUtility.ToJson(counselUser);

                // 생성된 키의 자식으로 json데이터를 삽입
                reference.Child("CounselUser").Child(newUser.UserId).SetRawJsonValueAsync(json);*/

               

            }
           
            
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
            SceneManager.LoadScene("Counselor_JoinFinish_Scene");  //회원가입에 성공했을 때 씬이 넘어가고 싶은데 작동 안함
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
            print("비밀번호가 일치하지 않습니다.");

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
      
        username = EmailInput.text;
        username = username.Substring(0, username.LastIndexOf('@'));


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
           print("비밀번호가 일치합니다.");

            PasswordOK.SetActive(true);
          // info.SetActive(false);
          // lifePattern.SetActive(true);

        }
        /* else if (emailOK == false)   //아이디 중복체크 부분
         {
             print("이미 존재하는 아이디입니다.");
         }
         else
         {
             PasswordOK.SetActive(false);
             print("비밀번호가 일치하지 않습니다.");
         }*/

    }


    // 생활패턴 -> 기본정보 이전버튼
    public void RegiterBefore()
    {
      //  info.SetActive(true);
     //   lifePattern.SetActive(false);

        MajorToggle();
        CounselDayToggle();
        CounselTimeToggle();
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
            counselday = sun.GetComponentInChildren<Text>().text;
        }
        else if (mon.isOn)
        {
            counselday = mon.GetComponentInChildren<Text>().text;
        }
        else if (tue.isOn)
        {
            counselday = tue.GetComponentInChildren<Text>().text;
        }
        else if (wed.isOn)
        {
            counselday = wed.GetComponentInChildren<Text>().text;
        }
        else if (thur.isOn)
        {
            counselday = thur.GetComponentInChildren<Text>().text;
        }
        else if (fri.isOn)
        {
            counselday = fri.GetComponentInChildren<Text>().text;
        }
        else
        {
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
        else if (T2.isOn)
        {
            counseltime = T2.GetComponentInChildren<Text>().text;
        }
        else if (T3.isOn)
        {
            counseltime = T3.GetComponentInChildren<Text>().text;
        }
        else if (T4.isOn)
        {
            counseltime = T4.GetComponentInChildren<Text>().text;
        }
        else if (T5.isOn)
        {
            counseltime = T5.GetComponentInChildren<Text>().text;
        }
        else if (T6.isOn)
        {
            counseltime = T6.GetComponentInChildren<Text>().text;
        }
        else if (T7.isOn)
        {
            counseltime = T7.GetComponentInChildren<Text>().text;
        }
        else if (T8.isOn)
        {
            counseltime = T8.GetComponentInChildren<Text>().text;
        }
        else if (T9.isOn)
        {
            counseltime = T9.GetComponentInChildren<Text>().text;
        }
        else if(T10.isOn)
        {
            counseltime = T10.GetComponentInChildren<Text>().text;
        }

        if (AllSelect.isOn)
        {
            counseltime = "All time";
            // 전체 시간 추가 방법 찾기
        }
    }
    }
    





    // 내담자 정보를 담는 ClientUser 클래스.
    class CounselUser
    {
        
        public string userGroup, email, username, sex, introduce, career, major, counselday, counseltime;
        public bool appointment;
        public string counselorInCharge, appDay, appTime, worry;


        // 상담자 생성자.
        public CounselUser(string userGroup, string email, string username, string sex, string introduce, string career,
            string major, string counselday, string counseltime,
            bool appointment, string counselorInCharge, string appDay, string appTime, string worry)
        {
            this.userGroup = userGroup;
            this.email = email;
            this.username = username;
            this.sex = sex;
            this.introduce = introduce;
            this.career = career;
            this.major = major;
            this.counselday = counselday;
            this.counseltime = counseltime;

            this.appointment = appointment;
            this.counselorInCharge = counselorInCharge;
            this.appDay = appDay;
            this.appTime = appTime;
            this.worry = worry;
        }
    }


