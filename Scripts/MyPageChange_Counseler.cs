using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class MyPageChange_Counseler : MonoBehaviour
{
    public DatabaseReference reference { get; set; }


    // 입력 정보를 받아올 인풋필드 오브젝트
    public InputField DisplayNameInput, IntroInput,CareerInput1,CareerInput2,CareerInput3;

    public Toggle Male, Female;
    

    // 입력 정보
    public string displayName, sex, major, intro, career1, career2, career3;

    public bool isInfoOk = false;

    
    public Toggle T1, T2, T3, T4, T5, T6, T7, T8, T9, T10; // 상담가능 시간
    public Toggle mon, tue, wed, thur, fri, sat, sun; // 상담가능 요일
    public Toggle course, job, love, relationship, myself, family; // 상담 전문 분야
  
    public static bool isOpenInfo;

    // Start is called before the first frame update
    void Start()
    {
       
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    // Update is called once per frame
    void Update()
    {

    }


    // 비밀번호 관련 변수들
    public InputField PasswordInput, PasswordCheckInput;
    public string password, passwordCheck;
    public GameObject PasswordOK; //비밀번호가 일치하면 체크표시 보여줄 오브젝트


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


    //기본정보 업데이트 저장 버튼
    public void ClickinfoSaveBtn()
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
        
        intro = IntroInput.text;
        
        if (displayName != "" && intro != "" && sex !="" )
        {
            //내담자 기본정보 업데이트
            Dictionary<string, object> updateCUserInfo = new Dictionary<string, object>();
            updateCUserInfo["username"] = displayName;
            updateCUserInfo["sex"] = sex;
            updateCUserInfo["intro"] = intro;
            

            reference.Child("CounselorUsers").Child(Auth_Manager.user.UserId).UpdateChildrenAsync(updateCUserInfo);


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

    // 전문분야 , 경력 업데이트 저장 버튼
    public void ClickSaveMajorBtn()
    {
        career1 = CareerInput1.text;
        career2 = CareerInput2.text;
        career3 = CareerInput3.text;

        MajorToggle(); //전문분야


        if (career1 != "" && career2 != "" && career3 != "" && major != "")
        {
            //상담사 전문분야, 경력 업데이트
            Dictionary<string, object> updateCUserMajor = new Dictionary<string, object>();
            updateCUserMajor["career1"] = career1;
            updateCUserMajor["career2"] = career2;
            updateCUserMajor["career3"] = career3;
            updateCUserMajor["major"] = major;

            reference.Child("CounselorUsers").Child(Auth_Manager.user.UserId).UpdateChildrenAsync(updateCUserMajor);

            Debug.Log("상담사 경력,전문분야 업데이트 완료");
        }
        else
        {
            Debug.Log("상담사 경력,전문분야 업데이트 실패...");
        }
    }

    // 상담시간, 요일 업데이트 저장 버튼
    public void ClickSaveTimeBtn()
    {

    }

    // 전문분야 토글 텍스트 값을 받아오고, 전공별로 데이터 전송(TODO!)
    void MajorToggle()
    {
        if (course.isOn)
        {
            major = course.GetComponentInChildren<Text>().text;
        }
        else if (job.isOn)
        {
            major = job.GetComponentInChildren<Text>().text;
        }
        else if (love.isOn)
        {
            major = love.GetComponentInChildren<Text>().text;
        }
        else if (relationship.isOn)
        {
            major =relationship.GetComponentInChildren<Text>().text;
        }
        else if (myself.isOn)
        {
            major = myself.GetComponentInChildren<Text>().text;
        }
        else
        {
            major = family.GetComponentInChildren<Text>().text;
        }
    }



    //  -> 기본정보 버튼
    public void ChangeToInfo()
    {
        isOpenInfo = true;

        if (isOpenInfo == true)
        {
            InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크
        }

    }


    //  -> 전공/경력 버튼
    public void ChangeToMajor()
    {
        MajorToggle();
    }


}
