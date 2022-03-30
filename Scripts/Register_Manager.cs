using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class Register_Manager : MonoBehaviour
{
    public DatabaseReference reference { get; set; }

    Firebase.Auth.FirebaseAuth auth;


    public GameObject info, lifePattern; // 페이지 전환

    string userGroup;

    public InputField EmailInput, PasswordInput, PasswordCheckInput, DisplayNameInput, BirthInput, JobInput;
    public Text emailCheck;
    public GameObject PasswordOK;
    public Toggle Male, Female;
    public bool emailOK;
    public Dropdown years, months, day;

    public Toggle mzero, mone, mtwo, mthree, mfour, mfive; // 식사 횟수
    public Toggle ezero, eone, ethree, efive; // 운동 횟수

    public Slider slider;
    public Text sleepTime; // 슬라이더 변화값 보여주는 텍스트
    private int min = 0;
    private int max = 24;

    // 입력 정보
    public string email, password, passwordCheck, username, displayName, sex, birth, job, meal, sleep, exercise;



    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
        new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        info.SetActive(true);
        lifePattern.SetActive(false);
        SetFunction_UI();
        PasswordOK.SetActive(false);
        SetDropdowonOptions();

        // 기본 값 설정
        userGroup = "내담자";
        sex = "남";
        meal = "0 끼";
        sleep = "7 시간";
        exercise = "0회";

        InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크

    }


    // 최종 회원가입 버튼.
    public void RegisterBtn()
    {

        MealToggle(); //식사 횟수
        ExerciseToggle(); //운동 횟수

        // 회원가입
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
      

            // 회원가입 성공시
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("회원가입 성공: {0} ({1})", displayName, newUser.UserId);

           

            // RDB에 내담자 데이터 저장
            ClientUser clientUser = new ClientUser(userGroup, email, displayName, sex, birth, job, meal, sleep, exercise,
               false, "", "", "", "");

            // 데이터를 json형태로 반환
            string json = JsonUtility.ToJson(clientUser);

            // 생성된 키의 자식으로 json데이터를 삽입
            reference.Child("ClientUsers").Child(newUser.UserId).SetRawJsonValueAsync(json);



            //SceneManager.LoadScene("LogIn_Scene");  //회원가입에 성공했을 때 씬이 넘어가고 싶은데 작동 안함

        });
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
            print("비밀번호가 일치합니다.");
        }
    }


    // 기본정보 -> 생활패턴 다음 버튼
    public void RegiterNext()
    {
        email = EmailInput.text;
        password = PasswordInput.text;
        passwordCheck = PasswordCheckInput.text;
        displayName = DisplayNameInput.text;
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

        //birth = BirthInput.text;
        birth = years.options[years.value].text + months.options[months.value].text + day.options[day.value].text;
        print(birth);
        job = JobInput.text;


        if ((password == passwordCheck && password != ""))
        {
            print("비밀번호가 일치합니다.");

            PasswordOK.SetActive(true);
            info.SetActive(false);
            lifePattern.SetActive(true);

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
        info.SetActive(true);
        lifePattern.SetActive(false);

        MealToggle();
        ExerciseToggle();
    }


    // 식사횟수 토글 텍스트 값을 받아오는 함수.
    void MealToggle()
    {
        if (mzero.isOn)
        {
            meal = mzero.GetComponentInChildren<Text>().text;
        }
        else if (mone.isOn)
        {
            meal = mone.GetComponentInChildren<Text>().text;
        }
        else if (mtwo.isOn)
        {
            meal = mtwo.GetComponentInChildren<Text>().text;
        }
        else if (mthree.isOn)
        {
            meal = mthree.GetComponentInChildren<Text>().text;
        }
        else if (mfour.isOn)
        {
            meal = mfour.GetComponentInChildren<Text>().text;
        }
        else
        {
            meal = mfive.GetComponentInChildren<Text>().text;
        }
    }


    // 운동횟수 토글 텍스트 값을 받아오는 함수.
    void ExerciseToggle()
    {
        if (ezero.isOn)
        {
            exercise = ezero.GetComponentInChildren<Text>().text;
        }
        else if (eone.isOn)
        {
            exercise = eone.GetComponentInChildren<Text>().text;
        }
        else if (ethree.isOn)
        {
            exercise = ethree.GetComponentInChildren<Text>().text;
        }
        else
        {
            exercise = efive.GetComponentInChildren<Text>().text;
        }
    }


    //생년월일 드롭다운 옵션목록을 생성하는 함수.
    public void SetDropdowonOptions()
    {
        //년도
        years.options.Clear();
        for (int i = 2022; i > 1939; i--)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = i.ToString() + "년";
            years.options.Add(option);
        }

        //월
        months.options.Clear();
        for (int i = 1; i < 13; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = i.ToString() + "월";
            months.options.Add(option);
        }

        //일
        day.options.Clear();
        for (int i = 1; i < 32; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = i.ToString() + "일";
            day.options.Add(option);
        }

    }


    // 기존에 있던 UI 관련 기능을 모두 삭제하고 슬라이더에 기능을 할당. (초기 UI의 기능을 할당해야 할 타이밍에 삽입)
    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        slider.onValueChanged.AddListener(Function_Slider);

    }

    // 슬라이더를 드래그했을 때 함수 발동, 반환 값은 float 값을 반환.
    private void Function_Slider(float time)
    {
        sleepTime.text = time.ToString() + " 시간";
        sleep = time.ToString() + "시간";
        Debug.Log("Slider Dragging!\n" + time);
    }

    //  슬라이더와 버튼 기능 전체 삭제하고 슬라이더의 maxValue(최댓값), minValue(최솟값), wholeNumbers(정수값으로 표시할지 여부) 설정.
    private void ResetFunction_UI()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.maxValue = max;
        slider.minValue = min;
        slider.wholeNumbers = true;
    }



    // 내담자 정보를 담는 ClientUser 클래스.
    class ClientUser
    {
        public string userGroup, email, username, sex, birth, job, meal, sleep, exercise;
        public bool appointment;
        public string counselorInCharge, appDay, appTime, worry;


        // 내담자 생성자.
        public ClientUser(string userGroup, string email, string username, string sex, string birth, string job,
            string meal, string sleep, string exercise,
            bool appointment, string counselorInCharge, string appDay, string appTime, string worry)
        {
            this.userGroup = userGroup;
            this.email = email;
            this.username = username;
            this.sex = sex;
            this.birth = birth;
            this.job = job;
            this.meal = meal;
            this.sleep = sleep;
            this.exercise = exercise;

            this.appointment = appointment;
            this.counselorInCharge = counselorInCharge;
            this.appDay = appDay;
            this.appTime = appTime;
            this.worry = worry;
        }
    }


}
