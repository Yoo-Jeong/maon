using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharAni : MonoBehaviourPunCallbacks
{

    //상담사 캐릭터 애니메이션
    public Animator animator;

    [PunRPC]
    public void clickNod()
    {
        Debug.Log("그렇군요 애니메이션 플레이");
        //animator.SetTrigger("Nod");  //Trigger 동기화가 느려서 bool로 대체
        animator.SetBool("nod", true);
        Invoke("StopNod", 0.5f);
    }


    [PunRPC]
    public void PlayNod()
    {

        photonView.RPC("clickNod", RpcTarget.All);
    }

    [PunRPC]
    public void StopNod()
    {
        //Debug.Log("그렇군요 애니메이션 중단");
        animator.SetBool("nod", false);
    }

    [PunRPC]
    public void PlayThumbup()
    {
        Debug.Log("잘하셨어요 애니메이션 플레이");
        animator.SetBool("thumbup", true);
        Invoke("StopThumbup", 0.2f);
    }

    [PunRPC]
    public void StopThumbup()
    {
        //Debug.Log("잘하셨어요 애니메이션 중단");
        animator.SetBool("thumbup", false);
    }

    [PunRPC]
    public void PlayFace_sad()
    {
        Debug.Log("속상해요 애니메이션 플레이");
        animator.SetBool("face_sad", true);
        Invoke("StopFace_sad", 0.2f);
    }

    [PunRPC]
    public void StopFace_sad()
    {
        //Debug.Log("속상해요 애니메이션 중단");
        animator.SetBool("face_sad", false);
    }

    [PunRPC]
    public void PlayTalk()
    {
        Debug.Log("Talk 애니메이션 플레이");
        animator.SetBool("talk", true);
        Invoke("StopTalk", 0.2f);
    }

    [PunRPC]
    public void StopTalk()
    {
        animator.SetBool("talk", false);
    }

}
