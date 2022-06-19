using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_Controller : MonoBehaviour
{
    public Canvas interior;            //인테리어 선택 캔버스
    public GameObject interior_green;  //휴식 인테리어
    public GameObject interior_warm;   //온기 인테리어

    //public Transform characterParent;  //characterParent의 자식으로 캐릭터 프리팹 생성을 위함
    public GameObject femalePrefab;    //여자 상담사 프리팹
    public GameObject malePrefab;      //남자 상담사 프리팹
    public GameObject TestPrefab;      //초기 상담사 프리팹

    public GameObject camRawImage;     //상담사 얼굴 화면
    public GameObject mycamRawImage;   //나(내담자) 얼굴 화면
    public GameObject openButton;     //상담사 얼굴 보기 버튼
    public GameObject closeButton;    //상담사 얼굴 닫기 버튼

    public GameObject myOpenButton;   //내 얼굴 보기 버튼
    public GameObject myCloseButton;  //내 얼굴닫기 버튼
     
    public Image overImage;           //마우스 오버시 나올 설명 이미지

    public static Canvas counselPopup, reportCanvas, endCanvas;  //상담 레포트, 종료화면 캔버스
    public GameObject enterPopup;       //상담소 입장 팝업
    public GameObject reportPopup;     //상담 레포트 도착 안내 팝업


    public void Awake()
    {
        counselPopup = GameObject.FindWithTag("Center_popup_Client").GetComponent<Canvas>();
        reportCanvas = GameObject.FindWithTag("Center_report_Client").GetComponent<Canvas>();
        endCanvas = GameObject.FindWithTag("Center_CounselEnd_Client").GetComponent<Canvas>();
        reportPopup.SetActive(false);

        counselPopup.enabled = false;
        reportCanvas.enabled = false;
        endCanvas.enabled = false;

        femalePrefab.SetActive(true);      //여자 상담사 프리팹 활성화
        malePrefab.SetActive(true);        //남자 상담사 프리팹 활성화
        TestPrefab.SetActive(true);        //초기 상담사 프리팹 활성화

        CamClose();
        MyCamClose();
        closeButton.SetActive(false);
        openButton.SetActive(true);

    }


    public void Start()
    {
        interior.enabled = true;            //인테리어 선택 캔버스 활성화
        interior_green.SetActive(false);    //휴식 인테리어 비활성화
        interior_warm.SetActive(false);     //온기 인테리어 비활성화

        femalePrefab.SetActive(false);      //여자 상담사 프리팹 비활성화
        malePrefab.SetActive(false);        //남자 상담사 프리팹 비활성화
        TestPrefab.SetActive(false);        //초기 상담사 프리팹 비활성화
 
    }


    //마음의 휴식 인테리어 선택 버튼 클릭
    public void ClickInterior_green()
    {
        interior_green.SetActive(true);  //휴식 인테리어 활성화
        interior_warm.SetActive(false);  //온기 인테리어 비활성화

        interior.enabled = false;        //인테리어 선택 캔버스 비활성화

        CreateCharacter();

        //상담소 입장 팝업 보이기
        counselPopup.enabled = true;
        enterPopup.SetActive(true);
        Invoke("DisableEnterPopup", 3f);
    }


    //마음의 온기 인테리어 선택 버튼 클릭
    public void ClickInterior_warm()
    {
        interior_green.SetActive(false);   //휴식 인테리어 비활성화
        interior_warm.SetActive(true);     //온기 인테리어 활성화

        interior.enabled = false;          //인테리어 선택 캔버스 비활성화

        CreateCharacter();

        //상담소 입장 팝업 보이기
        counselPopup.enabled = true;
        enterPopup.SetActive(true);
        Invoke("DisableEnterPopup", 5f);
    }


    //상담사 성별에 따른 캐릭터 생성
    public void CreateCharacter()
    {
        //오늘 상담사의 성별이 여자라면 여자 상담사 프리팹 생성
        if(User_Data.todayCounsel[2] == "여")
        {
            //GameObject myInstance = Instantiate(femalePrefab);
            femalePrefab.SetActive(true);
            malePrefab.SetActive(false);
            //Destroy(malePrefab);
            Debug.Log(User_Data.todayCounsel[2] + " 상담사 캐릭터 생성");
        }
        else         //오늘 상담사의 성별이 남자라면 남자 상담사 프리팹 생성
        {
            malePrefab.SetActive(true);
            femalePrefab.SetActive(false);
            //Destroy(femalePrefab);
            //GameObject myInstance = Instantiate(malePrefab);
            Debug.Log(User_Data.todayCounsel[2] + " 상담사 캐릭터 생성");
        }

    }



    //입장팝업  사라지게 하는 함수
    public void DisableEnterPopup()
    {
        counselPopup.enabled = false;
        enterPopup.SetActive(false);

    }


    //상담사 얼굴 보기 버튼
    public void CamButtonOpen()
    {
        
        CamOpen(); //상담사 화면 활성화
        openButton.SetActive(false);
        closeButton.SetActive(true);   
    }

    //상담사 얼굴 닫기 버튼
    public void CamButtonClose()
    {
        CamClose(); //상담사 화면 비활성화
        closeButton.SetActive(false);
        openButton.SetActive(true);
    }


   
    //상담사 화면 활성화
    public void CamOpen()
    {
        //상담사 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = camRawImage.GetComponent<RectTransform>();

        CloseMouseOverImage(); //마우스 오버 설명이미지 끄기

        //상담사 캠 화면 사이즈를 280,140으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 280);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 180);
        Debug.Log("상담사 화면 활성화  " + camTran.rect);
    }


    //상담사 화면 비활성화
    public void CamClose()
    {
        //상담사 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = camRawImage.GetComponent<RectTransform>();

        CloseMouseOverImage(); //마우스 오버 설명이미지 끄기

        //상담사 캠 화면 사이즈를 0,0으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        Debug.Log("상담사 화면 비활성화  " + camTran.rect);
    }


    //내 얼굴 닫기 버튼
    public void MyCamClose()
    {

        //내 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = mycamRawImage.GetComponent<RectTransform>();

        //내 캠 화면 사이즈를 0,0으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        Debug.Log("내 화면 비활성화  " + camTran.rect);


        myOpenButton.SetActive(true);
        myCloseButton.SetActive(false);

    }


    //내 얼굴 보기 버튼
    public void MyCamOpen()
    {

        //내 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = mycamRawImage.GetComponent<RectTransform>();

        //상담사 내 화면 사이즈를 280,140으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 280);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 180);
        Debug.Log("내 화면 활성화  " + camTran.rect);


        myOpenButton.SetActive(false);
        myCloseButton.SetActive(true);

    }


    //마우스 오버이미지 켜기
    public void OpenMouseOverImage()
    {
        overImage.enabled = true;
    }

    //마우스 오버이미지 끄기
    public void CloseMouseOverImage()
    {
        overImage.enabled = false;
    }



   public void OpenReport()
    {
        counselPopup.enabled = false;
        reportCanvas.enabled = true;
        endCanvas.enabled = false;

    }

    public void OpenCounselEnd()
    {
        counselPopup.enabled = false;
        reportCanvas.enabled = false;
        endCanvas.enabled = true;
    }

}