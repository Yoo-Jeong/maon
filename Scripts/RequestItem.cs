using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System;

public class RequestItem : MonoBehaviour
{
    public GameObject openObj;

    public Image imageBox;
    RectTransform rect;

    public bool isClose;


    // Start is called before the first frame update
    void Start()
    {
        isClose = true;
        openObj.SetActive(false);

    }

    public void ClickOpen()
    {

        if (isClose)
        {
            print("더보기 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1333, rect.sizeDelta.y + 85);

            openObj.SetActive(true);

            isClose = false;
        }
        else
        {

            print("접기 버튼 클릭");

            rect = (RectTransform)imageBox.transform;
            rect.sizeDelta = new Vector2(1333, rect.sizeDelta.y - 85);

            openObj.SetActive(false);

            isClose = true;

        }

    }
}
