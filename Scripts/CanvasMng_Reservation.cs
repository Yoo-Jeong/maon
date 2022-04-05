using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMng_Reservation : MonoBehaviour
{

 
    public Canvas Cprofile, Reservation, popup;

    public Image checkPopup, completePopup;
    public GameObject checkPopupObj;


    // Start is called before the first frame update
    void Start()
    {
         
        Reservation.enabled = true;
        Cprofile.enabled = false;
        popup.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Popup()
    {
        Reservation.enabled = false;
        Cprofile.enabled = true;

        popup.enabled = true;
        checkPopup.enabled = true;
        completePopup.enabled = false;
        
    }

    public void ReserBtn()
    {
        checkPopupObj.SetActive(false);
        completePopup.enabled = true;
        
    }


    public void CprofileCan()
    {
        Cprofile.enabled = true;
        Reservation.enabled = false;
        popup.enabled = false;

    }

    public void ReservationCan()
    {
        Reservation.enabled = true;
        Cprofile.enabled = false;
        popup.enabled = false;
    }

}
