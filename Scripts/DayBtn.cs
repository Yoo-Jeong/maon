using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DayBtn : MonoBehaviour
{
    public static string thisDay;

    public void ThisDay()
    {
        thisDay = this.GetComponentInChildren<Text>().text;
        print(thisDay);
    }





}
