using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 캔버스를 제어하기 위한 CanvasMng_Home 클래스.
public class CanvasMng_Home : MonoBehaviour
{
    // 캔버스 변수들.
    public Canvas home, Reservation, ReservationProfile, popup, counsel, myPage, myPageChange;
    public GameObject confirm, complete;


    // 비밀번호 일치 체크 확인을 위한 변수들.
    public InputField PasswordInput, PasswordCheckInput;
    public string password, passwordCheck;
    public GameObject PasswordOK; //비밀번호가 일치하면 체크표시를 보여줄 오브젝트

    public GameObject miniMenu;  //우측상단 이름 옆 버튼을 누르면 나오는 패널
    public bool isOpenMini;      

    // Start is called before the first frame update
    void Start()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);

        OpenHomeCanvas();

        confirm.SetActive(false);
        complete.SetActive(false);

        MyPageChange.isOpenInfo = false;

    }

    //우측상단 이름 옆 버튼 클릭
    public void ClickNameBtn()
    {
        if (isOpenMini)
        {
            miniMenu.SetActive(false);
            isOpenMini = false;
        }
        else
        {
            miniMenu.SetActive(true);
            isOpenMini = true;
        }
    }



    // 홈 화면 캔버스를 켜는 함수.
    public void OpenHomeCanvas()
    {
      
        home.enabled = true;
        counsel.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = false;
        popup.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
        Debug.Log("홈 화면 오픈");
    }


    // 상담메뉴 화면 캔버스를 켜는 함수.
    public void OpenCounselCanvs()
    {
        home.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = false;
        popup.enabled = false;
        counsel.enabled = true;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }

    
  

    // 예약메뉴 화면 캔버스를 켜는 함수.
    public void OpenReservationCanvs()
    {
        home.enabled = false;
        Reservation.enabled = true;
        ReservationProfile.enabled = false;
        popup.enabled = false;
        counsel.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }


    // 예약메뉴 상담사 프로필 화면 캔버스를 켜는 함수.
    public void OpenReservationProfileCanvas()
    {
        Debug.Log("예약메뉴 상담사 프로필 화면 오픈");

        home.enabled = false;
        ReservationProfile.enabled = true;
        Reservation.enabled = false;
        popup.enabled = false;
        counsel.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }



    // 팝업창 화면 캔버스를 켜는 함수.
    public void OpenPopupCanvas()
    {
        home.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = true;
        popup.enabled = true;
        counsel.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;

        SetActiveConfirm();

        MyPageChange.isOpenInfo = false;
    }

    // 예약신청 내용 확인 팝업창을 켜는 함수.
    public void SetActiveConfirm()
    {
        confirm.SetActive(true);
        complete.SetActive(false);
    }

    // 예약신청 내용 확인 팝업창을 켜는 함수.
    public void SetActiveComplete()
    {
        confirm.SetActive(false);
        complete.SetActive(true);
    }


    // 팝업창 화면 캔버스를 끄는 함수.
    public void ClosePopupCanvas()
    {
        home.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = true;
        popup.enabled = false;
        counsel.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = false;


        MyPageChange.isOpenInfo = false;
    }



    // 내 정보메뉴 화면 캔버스를 켜는 함수.
    public void OpenMypageCanvas()
    {
       
        home.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = false;
        popup.enabled = false;
        counsel.enabled = false;
        myPage.enabled = true;
        myPageChange.enabled = false;

        MyPageChange.isOpenInfo = false;
    }



   
    // 내 정보메뉴 프로필 정보 변경 화면 캔버스를 켜는 함수.
    public void OpneMypageChangeCanvas()
    {

        home.enabled = false;
        Reservation.enabled = false;
        ReservationProfile.enabled = false;
        popup.enabled = false;
        counsel.enabled = false;
        myPage.enabled = false;
        myPageChange.enabled = true;
        

        // 마이페이지 내 정보 변경 화면 켜짐(비밀번호 일치 체크 시작)
        MyPageChange.isOpenInfo = true;

        if (MyPageChange.isOpenInfo == true)
        {
            InvokeRepeating("PasswordCheck", 1, 2); //비밀번호 일치 체크
        }


    }



    // 비밀번호 일치 확인 함수.
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
