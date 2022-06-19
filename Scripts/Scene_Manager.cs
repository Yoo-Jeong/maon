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

    public void GoFirst()
    {
        SceneManager.LoadScene("First_Scene");
    }

    public void GoSelect()
    {
        SceneManager.LoadScene("Select_Scene");
    }

    public void GoLogIn()
    {
        SceneManager.LoadScene("LogIn_Scene");
          
    }


    public void GoClient_Join()
    {
        SceneManager.LoadScene("Client_Join_Scene");
    }

    public void GoCounselor_Join()
    {
        SceneManager.LoadScene("Counselor_Join_Scene");
    }

    public void GoCounselor_JoinFinish()
    {
        SceneManager.LoadScene("Counselor_JoinFinish_Scene");
    }

    public void GoClient_Home()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Client_Home_Scene");
        
        Debug.Log("상담소 퇴장");
        PhotonNetwork.Disconnect();
       
        Debug.Log("내담자 홈으로 이동");
        

    }


    public void GoCounselor_Home()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Counselor_Home_Scene");

        Debug.Log("상담소 퇴장");
        PhotonNetwork.Disconnect();

        Debug.Log("내담자 홈으로 이동");


    }


    public void GoClient_Center()
    {
        SceneManager.LoadScene("Client_Center_Scene");

        Debug.Log("내담자 상담소 씬 이동");
    }


}
