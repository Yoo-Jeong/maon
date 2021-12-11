using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Controller : MonoBehaviour
{
    public GameObject camRawImage;
    public GameObject panel;

    public void CamButton()
    {
        if (camRawImage.activeSelf == true)
        {
            camRawImage.SetActive(false);
        }
        else
        {
            camRawImage.SetActive(true);
        }
    }


    public void PanelButton()
    {
        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }


}