using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;


public class Char_Ani_Counselor : MonoBehaviour
{

    //상담사 캐릭터 애니메이션
    public Animator animator;

    public void Start()
    {
        OwnerTake();
    }

    //상담사캐릭터 애니메이션관련 시작
    //포톤 마스터 권한
    public void OwnerTake()
    {
        if (this.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("소유자 입니다.");
        }
        else
        {
            this.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("소유권을 가져옵니다.");
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

        //Debug.Log("그렇군요 애니메이션 중단");
        animator.SetBool("nod", false);
    }

    public void PlayThumbup()
    {

        Debug.Log("잘하셨어요 애니메이션 플레이");
        animator.SetBool("thumbup", true);
        Invoke("StopThumbup", 0.2f);
    }


    public void StopThumbup()
    {

        //Debug.Log("잘하셨어요 애니메이션 중단");
        animator.SetBool("thumbup", false);
    }


    public void PlayFace_sad()
    {

        Debug.Log("속상해요 애니메이션 플레이");
        animator.SetBool("face_sad", true);
        Invoke("StopFace_sad", 0.2f);
    }

    public void StopFace_sad()
    {

        //Debug.Log("속상해요 애니메이션 중단");
        animator.SetBool("face_sad", false);
    }


    public void PlayTalk()
    {

        Debug.Log("Talk 애니메이션 플레이");
        animator.SetBool("talk", true);
        Invoke("StopTalk", 0.2f);
    }


    public void StopTalk()
    {

        animator.SetBool("talk", false);
    }


}




