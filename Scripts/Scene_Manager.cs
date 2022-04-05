using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;

public class Scene_Manager : MonoBehaviourPunCallbacks
{

    public void click()
    {
        print("클릭");
    }

    private void Start()
    {
        Screen.SetResolution(1778, 1080, false);

        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결이 안되었을 때 
            Debug.Log("인터넷 연결 안됨");


        }else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // 데이터로 연결이 되었을 때
            Debug.Log("데이터로 연결됨");
        }
        else
        {
            // 와이파이로 연결이 되었을 때
            Debug.Log("와이파이로 연결됨");

        }


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
        
        Debug.Log("상담소 퇴장");
        PhotonNetwork.Disconnect();
       
        Debug.Log("홈으로 이동");
        

    }


    public void SceneChangeToCounsel()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Home_Scene");

        Debug.Log("상담소 퇴장");
        PhotonNetwork.Disconnect();

        Debug.Log("홈으로 이동");


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
