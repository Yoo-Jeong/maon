using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMng_Home : MonoBehaviour
{

    public Canvas home, counsel;
    public Canvas Cprofile, Reservation, popup, myPage, myPageChange;

    public Image checkPopup, completePopup;
    public GameObject checkPopupObj;

    public GameObject ReservationObj, CprofileObj;

    // Start is called before the first frame update
    void Start()
    {
        home.enabled = true;
        counsel.enabled = false;
        Reservation.enabled = false;
        Cprofile.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openCounselCan()
    {
        home.enabled = false;
        counsel.enabled = true;
        Reservation.enabled = false;
        Cprofile.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }

    public void openHomeCan()
    {
        home.enabled = true;
        counsel.enabled = false;
        Reservation.enabled = false;

        CprofileObj.SetActive(false);
        Cprofile.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }

    public void Popup()
    {
        home.enabled = false;
        counsel.enabled = false;

        Reservation.enabled = false;
        Cprofile.enabled = true;

        popup.enabled = true;
        myPage.enabled = false;
        myPageChange.enabled = false;

        checkPopupObj.SetActive(true);
        checkPopup.enabled = true;
        completePopup.enabled = false;

        MyPageChange.isOpenInfo = false;
    }

    public void ReserBtn()
    {
     
        checkPopup.enabled = false;
        checkPopupObj.SetActive(false);
        completePopup.enabled = true;

        MyPageChange.isOpenInfo = false;
    }

 

    public void CprofileCan()
    {
        home.enabled = false;
        counsel.enabled = false;


        CprofileObj.SetActive(true);
        Cprofile.enabled = true;
        ReservationObj.SetActive(false);
        Reservation.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }

    public void ReservationCan()
    {
        home.enabled = false;
        counsel.enabled = false;

        ReservationObj.SetActive(true);
        Reservation.enabled = true;
        Cprofile.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }


    public void OpenMypageCanvas()
    {
        myPage.enabled = true;

        home.enabled = false;
        counsel.enabled = false;
        Reservation.enabled = false;

        Cprofile.enabled = false;
        popup.enabled = false; 
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }



    public  InputField PasswordInput, PasswordCheckInput;
    public  string password, passwordCheck;
    public  GameObject PasswordOK; //비밀번호가 일치하면 체크표시 보여줄 오브젝트
    public void OpneMypageChangeCanvas()
    {

        myPageChange.enabled = true;
        myPage.enabled = false;

        home.enabled = false;
        counsel.enabled = false;
        Reservation.enabled = false;

        Cprofile.enabled = false;
        popup.enabled = false;



        // 마이페이지 내 정보 변경 화면 켜짐(비밀번호 일치 체크 시작)
        MyPageChange.isOpenInfo = true;

        if (MyPageChange.isOpenInfo == true)
        {
            InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크
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
            print("비밀번호가 일치합니다.");
        }
    }




}
