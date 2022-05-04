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

    public GameObject camRawImage;
    public GameObject mycamRawImage;
    public GameObject panel;
    public GameObject openButton;
    public GameObject closeButton;

    public bool isMyCamClose = true;



    public void Awake()
    {
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
    }


    //마음의 온기 인테리어 선택 버튼 클릭
    public void ClickInterior_warm()
    {
        interior_green.SetActive(false);   //휴식 인테리어 비활성화
        interior_warm.SetActive(true);     //온기 인테리어 활성화

        interior.enabled = false;          //인테리어 선택 캔버스 비활성화

        CreateCharacter();
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



    public void CamButtonOpen()
    {
        
        CamOpen();
        openButton.SetActive(false);
        closeButton.SetActive(true);   
    }

    public void CamButtonClose()
    {
        CamClose();
        closeButton.SetActive(false);
        openButton.SetActive(true);
    }

    public void CamOpen()
    {
        //상담사 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = camRawImage.GetComponent<RectTransform>();

        //상담사 캠 화면 사이즈를 280,140으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 280);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140);
        Debug.Log("상담사 화면 활성화  " + camTran.rect);
    }

    public void CamClose()
    {
        //상담사 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = camRawImage.GetComponent<RectTransform>();

        //상담사 캠 화면 사이즈를 0,0으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        Debug.Log("상담사 화면 비활성화  " + camTran.rect);
    }


    public void MyCamClose()
    {
        //내 캠 화면의 RectTransform 컴포넌트 추출
        RectTransform camTran = mycamRawImage.GetComponent<RectTransform>();

        //내 캠 화면 사이즈를 0,0으로 설정
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        Debug.Log("내 화면 비활성화  " + camTran.rect);
    }

    public void MyCamOpen()
    {

        if (isMyCamClose)
        {

            //내 캠 화면의 RectTransform 컴포넌트 추출
            RectTransform camTran = mycamRawImage.GetComponent<RectTransform>();

            //상담사 내 화면 사이즈를 280,140으로 설정
            camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 280);
            camTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140);
            Debug.Log("내 화면 활성화  " + camTran.rect);

            isMyCamClose = false;
        }
        else
        {
            MyCamClose();
            isMyCamClose = true;
        }
    }


    public void PanelButton()
    {
        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

   

}