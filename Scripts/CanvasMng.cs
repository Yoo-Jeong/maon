using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMng : MonoBehaviour
{
    public GameObject popup;


    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Popup()
    {
        popup.SetActive(true);
    }

}
