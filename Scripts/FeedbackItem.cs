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
            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1327, rect.sizeDelta.y + 105);

            openObj.SetActive(true);

            isClose = false;
        }
        else
        {
            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1327, rect.sizeDelta.y - 105);

            openObj.SetActive(false);

            isClose = true;

        }

    }


    public void ClickSave()
    {
        print("저장 버튼 클릭");

    }



}
