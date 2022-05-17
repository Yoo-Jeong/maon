using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;


public class LoginManager : MonoBehaviour
{
    //public Text LogText;

    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    //로그인 필드
    public InputField emailField;
    public InputField passwordField;
    public Button LogInButton;
    //public Button LogOutButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    //인증을 관리할 객체
    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseUser User;


    private void Awake()
    {
        //객체 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void Start()
    {
        if (LogInButton != null)
        {
            LogInButton.interactable = false;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var result = task.Result;

                if (result != DependencyStatus.Available)
                {
                    Debug.LogError(result.ToString());
                    IsFirebaseReady = false;
                }
                else
                {
                    IsFirebaseReady = true;

                    firebaseApp = FirebaseApp.DefaultInstance;
                    firebaseAuth = FirebaseAuth.DefaultInstance;
                }

                LogInButton.interactable = IsFirebaseReady;
            });
        }
    }


    //이메일 로그인
    public void LogIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null) { return; }

        IsSignInOnProgress = true;
        LogInButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text)
            .ContinueWithOnMainThread(task =>
            {
                //Debug.Log($"Sign in status : {task.Status}");

                IsSignInOnProgress = false;
                LogInButton.interactable = true;

                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("로그인 실패");
                   // LogText.text = "취소되었습니다.";
                }
                else
                {
                    User = task.Result;
                    //LogText.text = "UID : " + User.UserId;
                    Debug.Log("로그인 성공! " + "UID: " + User.UserId + "  이메일 : " + User.Email);

                    //로그인에 성공하면 홈 화면으로 이동
                    SceneManager.LoadScene("Counselor_Home_Scene");
                }
            }
            );
    }


    public void LogOut()
    {
        
            firebaseAuth.SignOut();
        //LogText.text = "로그아웃";
        Debug.Log("로그아웃 되었습니다.");
        
    }




    // 테스트용 간편 회원가입
    public void RegisterTest()
    {
        // 이메일과 비밀번호로 회원가입
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task => {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(emailField.text + "로 회원가입\n");
                }
                else
                    Debug.Log("회원가입 실패\n");
            }
            );
    }


}