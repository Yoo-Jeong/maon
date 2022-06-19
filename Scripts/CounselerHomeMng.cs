using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounselerHomeMng : MonoBehaviour
{
    public Image home, reservation, counsel, client, homeB, reservationB, counselB, clientB;
    public Canvas homeC, reservationC, clientC, mypageC;
    public GameObject miniMenu;  //우측상단 이름 옆 버튼을 누르면 나오는 패널
    public bool isOpenMini;
    // Start is called before the first frame update
    void Start()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);
        OpenHomeCanvs();


        home.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        homeB.enabled = true;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;
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
    public void OpenHomeCanvs()
    {

        mypageC.enabled = false;
        homeC.enabled = true;
        reservationC.enabled = false;
        clientC.enabled = false;


        home.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        homeB.enabled = true;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;
    }
    public void OpenReservationCanvs()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);
        mypageC.enabled = false;
        homeC.enabled = false;
        reservationC.enabled = true;
        clientC.enabled = false;


        home.color = new Color(1f, 1f, 1f);
        homeB.enabled = false;
        reservation.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        reservationB.enabled = true;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;
    }
    public void OpenClientCanvs()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);
        mypageC.enabled = false;
        homeC.enabled = false;
        reservationC.enabled = false;
        clientC.enabled = true;


        home.color = new Color(1f, 1f, 1f);
        homeB.enabled = false;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        clientB.enabled = true;

    }
    public void OpenMypageCanvs()
    {
        isOpenMini = false;
        miniMenu.SetActive(false);
        mypageC.enabled = true;
        homeC.enabled = false;
        reservationC.enabled = false;
        clientC.enabled = false;
    }



    

    public void counselSelect()
    {

        home.color = new Color(1f, 1f, 1f);
        homeB.enabled = false;
        reservation.color = new Color(1f, 1f, 1f); ;
        reservationB.enabled = false;
        counsel.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        counselB.enabled = true;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;

    }

   

}
