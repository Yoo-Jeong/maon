using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounselSceneMng : MonoBehaviour
{
    public GameObject miniMenu;  //우측상단 이름 옆 버튼을 누르면 나오는 패널
    public bool isOpenMini;
    void Start()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);
    }
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
}
