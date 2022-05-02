using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SangdamCtrl_counselor : MonoBehaviourPunCallbacks
{

    public GameObject bg;
    bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    //공감하기 버튼을 누르면 실행되는 함수. 하위버튼이 들어있는 게임 오브젝트를 켠다.
    public void ClickAniMenuBtn()
    {
        if (open)
        {
            open = false;
            bg.SetActive(true);

        }
        else
        {
            open = true;
            bg.SetActive(false);

        }


    }


}
