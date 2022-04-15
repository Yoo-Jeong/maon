using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public  class MyPageChange : MonoBehaviour
{

    // 입력 정보를 받아올 인풋필드 오브젝트
    public InputField DisplayNameInput, BirthInput, JobInput;

   

    // 입력 정보
    public string email, username, displayName, sex, birth, job, meal, sleep, exercise;

    

    public GameObject info, lifePattern; // 페이지 전환

    public Dropdown years, months, day; // 드롭다운 옵션

    public Slider slider;
    public Text sleepTime; // 슬라이더 변화값 보여주는 텍스트

    private int min = 0;
    private int max = 24;


    public static bool isOpenInfo;

    // Start is called before the first frame update
    void Start()
    {
        SetDropdowonOptions();

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 기본정보 -> 생활패턴 버튼
    public void ChangeToLifepattern()
    {
     
        info.SetActive(false);
        lifePattern.SetActive(true);

        isOpenInfo = false;
      
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
        //sleep = time.ToString() + "시간";
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




}
