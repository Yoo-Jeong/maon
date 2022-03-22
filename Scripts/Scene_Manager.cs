using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
 

    private void Start()
    {
        Screen.SetResolution(1080, 657, false);
    }

    public void SceneChangeToSelect()
    {
        SceneManager.LoadScene("Select_Scene");
    }

    public void SceneChangeToLogIn()
    {
        SceneManager.LoadScene("LogIn_Scene");
          
    }

    public void SceneChangeToRegister()
    {
        SceneManager.LoadScene("Register_Scene");
    }

    public void SceneChangeToHome()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Home_Scene");

    }

    public void SceneChangeToReservationHome()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("ReservationHome_Scene");

    }

    public void SceneChangeToCenter()
    {
        SceneManager.LoadScene("Center_Scene");
    }

    public void SceneChangeToReservationCounselorProfile()
    {
        SceneManager.LoadScene("ReservationCounselorProfile_Scene");
    }



    public void destory()
    {
        GameObject.Destroy(GameObject.Find("PlayFabManager"));
    }


    public void mypageMenu()
    {
        GameObject menu = GameObject.Find("mypageMenu_Panel");
        menu.SetActive(true);

    }


}
