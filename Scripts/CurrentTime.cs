using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CurrentTime : MonoBehaviour
{
    public Text Time_Text, Time_Text2;
    public Text Date_Text, Date_Text2;

    // Update is called once per frame
    void Update()
    {
        Time_Text.text = DateTime.Now.ToString("hh : mm tt");
        Date_Text.text = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");

        Time_Text2.text = DateTime.Now.ToString("hh : mm tt");
        Date_Text2.text = DateTime.Now.ToString("yyyy년 MM월 dd일 dddd");
    }
}
