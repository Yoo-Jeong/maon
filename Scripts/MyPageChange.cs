using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public  class MyPageChange : MonoBehaviour
{
    public DatabaseReference reference { get; set; }


    // 입력 정보를 받아올 인풋필드 오브젝트
    public InputField DisplayNameInput, JobInput;

    public Toggle Male, Female;

    // 입력 정보
    public string displayName, sex, birth, job, meal, sleep, exercise;

    public bool isInfoOk = false;

    public GameObject info, lifePattern; // 페이지 전환

    public Dropdown years, months, day; // 드롭다운 옵션


    public Toggle mzero, mone, mtwo, mthree, mfour, mfive; // 식사 횟수
    public Toggle ezero, eone, ethree, efive; // 운동 횟수

    public Slider slider;
    public Text sleepTime; // 슬라이더 변화값 보여주는 텍스트

    private int min = 0;
    private int max = 24;


    public static bool isOpenInfo;

    // Start is called before the first frame update
    void Start()
    {
        SetDropdowonOptions();
        SetFunction_UI();
        sleep = "7 시간";

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    // Update is called once per frame
    void Update()
    {
        
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


  


    // 비밀번호 관련 변수들
    public  InputField PasswordInput, PasswordCheckInput;
    public  string password, passwordCheck;
    public  GameObject PasswordOK; //비밀번호가 일치하면 체크표시 보여줄 오브젝트


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


    //비밀번호 업데이트 함수
    public void UpdatePassword(string newPassword)
    {

        if (Auth_Manager.auth.CurrentUser != null)
        {
            Auth_Manager.auth.CurrentUser.UpdatePasswordAsync(newPassword).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("비밀번호 업데이트가 취소되었습니다.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("비밀번호 업데이트 중 오류가 발생했습니다. :  " + task.Exception);
                    return;
                }

                Debug.Log("비밀번호가 업데이트 성공");
            });
        }
    }


    //기본정보 저장 버튼
    public void ClickInfoSaveBtn()
    {
        isInfoOk = false;
        password = PasswordInput.text;
        passwordCheck = PasswordCheckInput.text;
        displayName = DisplayNameInput.text;
   

        if (Male.isOn)
        {
            sex = Male.GetComponentInChildren<Text>().text;
        }
        else
        {
            sex = Female.GetComponentInChildren<Text>().text;
        }

        birth = years.options[years.value].text + months.options[months.value].text + day.options[day.value].text;
        job = JobInput.text;


        if (displayName != "" && birth != "" && job != "") {
            //내담자 기본정보 업데이트
            Dictionary<string, object> updateUserInfo = new Dictionary<string, object>();
            updateUserInfo["username"] = displayName;
            updateUserInfo["sex"] = sex;
            updateUserInfo["birth"] = birth;
            updateUserInfo["job"] = job;

            reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).UpdateChildrenAsync(updateUserInfo);


            //비밀번호 업데이트
            if (passwordCheck != "")
            {
                isInfoOk = false;
                UpdatePassword(passwordCheck);
            }
            else
            {
                Debug.Log("변경할 비밀번호를 입력해주세요.");
            }

            isInfoOk = true;
            Debug.Log("내담자 기본정보 업데이트 완료");
                   
        }
        else
        {
            isInfoOk = false;
            Debug.Log("모든 정보를 입력해주세요");
        }

        if (isInfoOk)
        {
            //CanvasMng_Home.OpenMypageCanvas();
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

    //생활패턴 저장 버튼
    public void ClickPatternSaveBtn()
    {
        MealToggle(); //식사 횟수
        ExerciseToggle(); //운동 횟수


        if (meal != "" && sleep != "" && exercise != "")
        {
            //내담자 기본정보 업데이트
            Dictionary<string, object> updateUserPattern = new Dictionary<string, object>();
            updateUserPattern["meal"] = meal;
            updateUserPattern["sleep"] = sleep;
            updateUserPattern["exercise"] = exercise;
       
            reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).UpdateChildrenAsync(updateUserPattern);

            Debug.Log("내담자 생활패턴 업데이트 완료");
        }
        else
        {
            Debug.Log("내담자 생활패턴 업데이트 실패...");
        }
    }


    // 생활패턴 -> 기본정보 버튼
    public void ChangeToInfo()
    {
        info.SetActive(true);
        lifePattern.SetActive(false);

        isOpenInfo = true;

        if (isOpenInfo == true)
        {
            InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크
        }

    }


    // 기본정보 -> 생활패턴 버튼
    public void ChangeToPattern()
    {
        info.SetActive(false);
        lifePattern.SetActive(true);

        //MealToggle();
        //ExerciseToggle();
    }


}
