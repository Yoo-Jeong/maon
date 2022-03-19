using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Resiter_Manager : MonoBehaviour
{
    public GameObject info, lifePattern; // 페이지 전환

    public InputField EmailInput, PasswordInput, PasswordCheckInput, DisplayNameInput, BirthInput, JobInput;
    public Text emailCheck;
    public GameObject PasswordOK;
    public Toggle Male, Female;
    public bool emailOK;
    

    public Toggle mzero, mone, mtwo, mthree, mfour, mfive; // 식사 횟수
    public Toggle ezero, eone, ethree, efive; // 운동 횟수

    public Slider slider;
    public Text sleepTime; // 슬라이더 변화값 보여주는 텍스트
    private int min = 0;
    private int max = 24;

    // 입력 정보
    public string email, password, passwordCheck, username, displayName, sex, birth, job, meal, sleep, exercise; 
    //private string reservation, counselor, counselDay, counselTime, concern;


    private void Start()
    {
        info.SetActive(true);
        lifePattern.SetActive(false);
        SetFunction_UI();
        PasswordOK.SetActive(false);

        // 기본 값
        sex = "남";
        meal = "0 끼";
        sleep = "7 시간";
        exercise = "0회";
      /*  reservation = "0";
        counselor = "";
        counselDay = "";
        counselTime = "";
        concern = "";*/

        emailOK = false;
        InvokeRepeating("PasswordCheck", 1, 2);
       
    }

    public void ResiterSignin()
    {
        var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }


    public void SetProfileData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "이름", displayName }, { "성별", sex },
            { "생년월일", birth },{ "직업", job },{ "식사", meal },{ "수면", sleep },{ "운동", exercise },
             { "예약여부", "0" },},
        
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패" + error));



        
    }

    public void SetReserData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { 
                { "예약여부", "0" }, { "상담사", "" },{ "상담날짜", "" },{ "상담시간", "" },{ "고민내용", "" }},

            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패" + error));


    }


    public void RegisterBtn()
    {
        MealToggle();
        ExerciseToggle();
        
        var request = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username, DisplayName = displayName };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);

        Invoke("ResiterSignin", 1);

    }


    void OnLoginSuccess(LoginResult result)
    {
        print("로그인 성공");
        SetProfileData();
        //SetReserData();
        SceneManager.LoadScene("LogIn_Scene");

    }

    // 이메일 중복 체크 버튼
    public void EmailCheckBtn()
    {
        email = EmailInput.text;
        var request = new LoginWithEmailAddressRequest { Email = email, Password = "password" };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }


    // 비밀번호 일치 확인
    public void PasswordCheck()
    {
        password = PasswordInput.text;
        passwordCheck = PasswordCheckInput.text;

        if( (password != passwordCheck) || (password == ""))
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
    public void ResiterNext()
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

        birth = BirthInput.text;
        job = JobInput.text;


        if ((password == passwordCheck && password != "") && emailOK == true)
        {
            print("비밀번호가 일치합니다.");

            PasswordOK.SetActive(true);
            info.SetActive(false);
            lifePattern.SetActive(true);

        }else if (emailOK == false)
        {
            print("이미 존재하는 아이디입니다.");
        }
        else
        {
            PasswordOK.SetActive(false);
            print("비밀번호가 일치하지 않습니다.");
        }
     
    }

    public void ResiterBefore()
    {
        info.SetActive(true);
        lifePattern.SetActive(false);

        MealToggle();
        ExerciseToggle();
    }


    // 식사횟수 토글
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


    // 운동횟수 토글
    void ExerciseToggle()
    {
        if (ezero.isOn)
        {
            exercise = ezero.GetComponentInChildren<Text>().text;
        }
        else if (eone.isOn)
        {
            exercise = eone.GetComponentInChildren<Text>().text;
        }else if (ethree.isOn)
        {
            exercise = ethree.GetComponentInChildren<Text>().text;
        } else
        {
            exercise = efive.GetComponentInChildren<Text>().text;
        }
    }

    // 기존에 있던 UI 관련 기능을 모두 삭제하고 슬라이더에 기능을 할당 (초기 UI의 기능을 할당해야 할 타이밍에 삽입)
    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        slider.onValueChanged.AddListener(Function_Slider);

    }

    // 슬라이더를 드래그했을 때 함수 발동, 반환 값은 float 값을 반환
    private void Function_Slider(float time)
    {
        sleepTime.text = time.ToString() + " 시간";
        sleep = time.ToString() + "시간";
        Debug.Log("Slider Dragging!\n" + time);
    }

    //  슬라이더와 버튼 기능 전체 삭제하고 슬라이더의 maxValue(최댓값), minValue(최솟값), wholeNumbers(정수값으로 표시할지 여부) 설정
    private void ResetFunction_UI()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.maxValue = max;
        slider.minValue = min;
        slider.wholeNumbers = true;
    }


    // 아이디 중복체크 (로그인api 호출)
    //void OnLoginFailure(PlayFabError error) => print("로그인 실패" + error);
    void OnLoginFailure(PlayFabError error)
    {
        string errorCheck, userExist;
        //userNotFound = "/Client/LoginWithEmailAddress: User not found";
        userExist = "/Client/LoginWithEmailAddress: Invalid email address or password";

        errorCheck = error.ToString(); 

        if ( errorCheck == userExist)
        {
            emailCheck.text = "이미 존재하는 아이디입니다.";
            print("이미 존재하는 아이디입니다. : " + errorCheck);
            emailOK = false;
        }
        else
        {
            emailCheck.text = "사용가능한 아이디입니다.";
            print("사용가능한 아이디입니다. : " + errorCheck);
            emailOK = true;
        }
    }


    void OnRegisterSuccess(RegisterPlayFabUserResult result) => print("회원가입 성공");
    void OnRegisterFailure(PlayFabError error) => print("회원가입 실패" + error);
}
