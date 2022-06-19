using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationEffect : MonoBehaviour
{
    // 네비게이션 버튼 목록
    public Image home, reservation, counsel, client, homeB, reservationB, counselB, clientB;
  

    // Start is called before the first frame update
    void Start()
    {
        home.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        homeB.enabled = true ;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;

    }
    public void HomeSelect()
    {
            home.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
            homeB.enabled = true;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
            counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;
    }

    public void ReservationSelect()
    {
       
            home.color = new Color(1f, 1f, 1f);
        homeB.enabled = false;
        reservation.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        reservationB.enabled = true;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(1f, 1f, 1f);
        clientB.enabled = false;

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

    public void clientSelect()
    {
     
            home.color = new Color(1f, 1f, 1f);
        homeB.enabled = false;
        reservation.color = new Color(1f, 1f, 1f);
        reservationB.enabled = false;
        counsel.color = new Color(1f, 1f, 1f);
        counselB.enabled = false;
        client.color = new Color(0.5294118f, 0.5960785f, 0.3843138f);
        clientB.enabled = true;

    }


}
