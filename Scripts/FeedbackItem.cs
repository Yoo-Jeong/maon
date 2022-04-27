using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


public class FeedbackItem : MonoBehaviour
{

    public GameObject openObj;

    public Image imageBox;
    RectTransform rect;

    public bool isClose;

    public void Start()
    {
        isClose = true;
        openObj.SetActive(false);

    }

    public void ClickOpen()
    {

        if (isClose)
        {
            print("작성 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1454, rect.sizeDelta.y + 130);

            openObj.SetActive(true);


            isClose = false;
        }
        else
        {

            print("닫기 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1454, rect.sizeDelta.y - 130);

            openObj.SetActive(false);


            isClose = true;

        }

    }



    public void ClickSave()
    {
        print("저장 버튼 클릭");

    }



}
