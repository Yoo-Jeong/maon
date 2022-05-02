using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


// 상담사의 상담소 씬에서 좌측 메뉴 버튼들의 위치를 제어하는 클래스.
public class ButtonControl : MonoBehaviour
{
    public bool isClose1 = true;
    public bool isClose2 = true;
    public bool isClose3 = true;

    public Button button1, button2, button3;

    public RectTransform pos1 ,pos2, pos3;


    public void Start()
    {
        pos1 = button1.GetComponent<RectTransform>();
        pos2 = button2.GetComponent<RectTransform>();
        pos3 = button3.GetComponent<RectTransform>();
    }

    public void ClickButton1()
    {
        if (isClose1)
        {         
            pos2.anchoredPosition = new Vector2(226, pos1.position.y - 1530 );
            pos3.anchoredPosition = new Vector2(226, pos1.position.y - 1630);

            isClose1 = false;

        }
        else
        {          
            pos2.anchoredPosition = new Vector2(226, -139);
            pos3.anchoredPosition = new Vector2(226, -237);

            isClose1 = true;
        }
    }

    public void ClickButton2()
    {
        if (isClose2)
        {  
            pos3.anchoredPosition = new Vector2(226, pos2.position.y - 1480);

            isClose2 = false;

        }
        else
        {           
            pos3.anchoredPosition = new Vector2(226, -237);

            isClose2 = true;
        }
    }

    public void ClickButton3()
    {

    }


}
