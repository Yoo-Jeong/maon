using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMng_Home : MonoBehaviour
{

    public Canvas home, counsel;

    public Canvas Cprofile, Reservation, popup;

    public Image checkPopup, completePopup;
    public GameObject checkPopupObj;

    public GameObject ReservationObj, CprofileObj;

    // Start is called before the first frame update
    void Start()
    {
        home.enabled = true;
        counsel.enabled = false;
        Reservation.enabled = false;
        Cprofile.enabled = false;
        popup.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openCounselCan()
    {
        home.enabled = false;
        counsel.enabled = true;
        Reservation.enabled = false;
        Cprofile.enabled = false;
        popup.enabled = false;
    }

    public void openHomeCan()
    {
        home.enabled = true;
        counsel.enabled = false;
        Reservation.enabled = false;

        CprofileObj.SetActive(false);
        Cprofile.enabled = false;
        popup.enabled = false;
    }

    public void Popup()
    {
        home.enabled = false;
        counsel.enabled = false;

        Reservation.enabled = false;
        Cprofile.enabled = true;

        popup.enabled = true;

        checkPopupObj.SetActive(true);
        checkPopup.enabled = true;
        completePopup.enabled = false;

    }

    public void ReserBtn()
    {
     
        checkPopup.enabled = false;
        checkPopupObj.SetActive(false);
        completePopup.enabled = true;

    }

 

    public void CprofileCan()
    {
        home.enabled = false;
        counsel.enabled = false;


        CprofileObj.SetActive(true);
        Cprofile.enabled = true;
        ReservationObj.SetActive(false);
        Reservation.enabled = false;
        popup.enabled = false;

    }

    public void ReservationCan()
    {
        home.enabled = false;
        counsel.enabled = false;

        ReservationObj.SetActive(true);
        Reservation.enabled = true;
        Cprofile.enabled = false;
        popup.enabled = false;
    }

}
