using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


// 상담사의 상담소 씬에서 좌측 메뉴 버튼들의 위치를 제어하는 클래스.
public class ButtonControl : MonoBehaviour
{
    public bool[] isClose;

    public Button[] buttons;

    public RectTransform[] pos;

    public GameObject[] contents;


    public void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            isClose[i] = true;
            pos[i] = buttons[i].GetComponent<RectTransform>();   
        }
    }

    public void ClickButton1()
    {
        if (isClose[0])
        {
            pos[1].anchoredPosition = new Vector2(226, pos[0].position.y - 1530 );
            pos[2].anchoredPosition = new Vector2(226, pos[0].position.y - 1630);
            pos[3].anchoredPosition = new Vector2(226, pos[0].position.y - 1730);

            isClose[0] = false;

        }
        else
        {
            pos[1].anchoredPosition = new Vector2(226, -139);
            pos[2].anchoredPosition = new Vector2(226, -233);
            pos[3].anchoredPosition = new Vector2(226, -327);

            isClose[0] = true;
        }
    }

    public void ClickButton2()
    {
        if (isClose[1])
        {
            pos[2].anchoredPosition = new Vector2(226, pos[1].position.y - 1480);
            pos[3].anchoredPosition = new Vector2(226, pos[1].position.y - 1580);

            isClose[1] = false;

        }
        else
        {
            pos[2].anchoredPosition = new Vector2(226, -233);
            pos[3].anchoredPosition = new Vector2(226, -327);

            isClose[1] = true;
        }
    }

    public void ClickButton3()
    {
        if (isClose[2])
        {
            pos[3].anchoredPosition = new Vector2(226, pos[2].position.y - 1480);
            buttons[2].GetComponentInChildren<Text>().color = Color.white;

            isClose[2] = false;

        }
        else
        {
            pos[3].anchoredPosition = new Vector2(226, -327);
            buttons[2].GetComponentInChildren<Text>().color = new Color (134 /255f, 152/255f, 101/255f);

            isClose[2] = true;
        }
    }

    public void ClickButton4()
    {
        if (isClose[3])
        {
            pos[4].anchoredPosition = new Vector2(226, pos[3].position.y - 1480);

            isClose[3] = false;

        }
        else
        {
            pos[4].anchoredPosition = new Vector2(226, -327);

            isClose[3] = true;
        }
    }


}
