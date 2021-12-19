using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SangdamCtrl : MonoBehaviourPunCallbacks
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void PlayNod()
    {
        Debug.Log("공감하기 애니메이션 플레이");
        //animator.SetTrigger("Nod");  //Trigger 동기화가 느려서 bool로 대체
        animator.SetBool("nod", true);
        Invoke("StopNod", 0.5f);
    }

    public void StopNod()
    {
        Debug.Log("공감하기 애니메이션 중단");
        animator.SetBool("nod", false);
    }

}
