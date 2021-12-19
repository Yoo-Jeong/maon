using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1080, 657, false);
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

    public void SceneChangeToCenter()
    {
        SceneManager.LoadScene("Center_Scene");
    }

}
