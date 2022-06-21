using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEvent : MonoBehaviour
{
    public GameObject detail;

    void Start()
    {
        detail.SetActive(false);
    }
    public void OnMouseOver()
    { 
        detail.SetActive(true);
    }

    public void OnMouseExit()
    {
        detail.SetActive(false);
    }
}
