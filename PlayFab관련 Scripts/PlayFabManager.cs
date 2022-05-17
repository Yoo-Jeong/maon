using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabManager : MonoBehaviour
{

    public InputField EmailInput, PasswordInput, UsernameInput;
    public string password, username;
    public static string email;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void LogOutBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.ForgetAllCredentials();
 
    }


    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        print("로그인 성공" );
        email = result.PlayFabId;

        SceneManager.LoadScene("Home_Scene");

        GetUserData(result.PlayFabId);

    }


    void OnLoginFailure(PlayFabError error) => print("로그인 실패");
    void OnRegisterSuccess(RegisterPlayFabUserResult result) => print("회원가입 성공");
    void OnRegisterFailure(PlayFabError error) => print("회원가입 실패");


 
    public static string reservation, counselor, counselDay, counselTime, concern, displayname;
    public static void GetUserData(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null

        }, result => {
            Debug.Log("데이터 불러오기 성공");
            reservation = result.Data["예약여부"].Value;

            if (result.Data == null) { 
                Debug.Log("데이터 없음.");
                
            }else if (reservation == "1") {
                Debug.Log("예약여부: " + result.Data["예약여부"].Value);
                Debug.Log("상담사: " + result.Data["상담사"].Value);
                Debug.Log("이름: " + result.Data["이름"].Value);
                reservation = result.Data["예약여부"].Value;
                counselor = result.Data["상담사"].Value;
                counselDay = result.Data["상담날짜"].Value;
                counselTime = result.Data["상담시간"].Value;
                concern = result.Data["고민내용"].Value;
                displayname = result.Data["이름"].Value;
            }

            else { 
              
                displayname = result.Data["이름"].Value;

            }
        }, 
        (error) => {
            Debug.Log("데이터 불러오기 실패");
            Debug.Log(error.GenerateErrorReport());
        });
    }


}
