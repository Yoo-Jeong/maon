using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SangdamCtrl : MonoBehaviourPunCallbacks
{
    public Animator animator;

    public GameObject bg;
    bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    //공감하기 버튼을 누르면 실행되는 함수. 하위버튼이 들어있는 게임 오브젝트를 켠다. (상담사씬에서 필요)
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


    public void PlayNod()
    {
        Debug.Log("그렇군요 애니메이션 플레이");
        //animator.SetTrigger("Nod");  //Trigger 동기화가 느려서 bool로 대체
        animator.SetBool("nod", true);
        Invoke("StopNod", 0.5f);
    }

    public void StopNod()
    {
        Debug.Log("그렇군요 애니메이션 중단");
        animator.SetBool("nod", false);

    }

    public void PlayThumbup()
    {
        Debug.Log("잘하셨어요 애니메이션 플레이");
        animator.SetBool("thumbup", true);
        Invoke("StopThumbup", 0.5f);
    }


    public void StopThumbup()
    {
        Debug.Log("잘하셨어요 애니메이션 중단");
        animator.SetBool("thumbup", false);

    }


    public void PlayFace_sad()
    {
        Debug.Log("속상해요 애니메이션 플레이");
        animator.SetBool("face_sad", true);
        Invoke("StopFace_sad", 0.5f);
    }

    public void StopFace_sad()
    {
        Debug.Log("속상해요 애니메이션 중단");
        animator.SetBool("face_sad", false);
    }



}
