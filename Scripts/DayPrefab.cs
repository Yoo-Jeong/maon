using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DayPrefab : MonoBehaviour
{
    public static string thisDay;

    public Toggle dayToggle;
   


    public void Start()
    {
        //SetFunction_UI();
    }


    public void ThisDay()
    {
        thisDay = this.GetComponentInChildren<Text>().text;        
    }


    public void Function_dayToggle(bool _bool)
    {

        if (_bool)
        {
            Debug.Log(this.gameObject.GetComponentInChildren<Text>().text);
            this.gameObject.GetComponentInChildren<Text>().color = Color.white;
        
        }
        else
        {
            this.gameObject.GetComponentInChildren<Text>().color = Color.black;
           
        }


    }


    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        dayToggle.onValueChanged.AddListener(Function_dayToggle);

    }

    private void ResetFunction_UI()
    {
        if (dayToggle.group != null)
        {
            dayToggle.onValueChanged.RemoveAllListeners();
        }
     
    }


}
