using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_Controller : MonoBehaviour
{
    public GameObject camRawImage;
    public GameObject panel;
    public GameObject openButton;
    public GameObject closeButton;

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

    public void Start()
    {
        CamClose();
        closeButton.SetActive(false);
        openButton.SetActive(true);
    }

}