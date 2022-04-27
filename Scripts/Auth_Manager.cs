using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;


public class Auth_Manager : MonoBehaviour
{
    //public Text LogText;

    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    //로그인 필드
    public InputField emailField;
    public InputField passwordField;
    public Button LogInButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    //인증을 관리할 객체
    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseUser User;

    string client = "내담자";


    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    public void Start()
    {

        //객체 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
      new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");


        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;




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
        //if (!IsFirebaseReady || IsSignInOnProgress || User != null) { return; }

        //IsSignInOnProgress = true;
        //LogInButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text)
            .ContinueWithOnMainThread(task =>
            {
                //Debug.Log($"Sign in status : {task.Status}");

                // IsSignInOnProgress = false;
                //LogInButton.interactable = true;

                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("로그인 실패");
                    //LogText.text = "취소되었습니다.";
                }
                else
                {
                    User = task.Result;
                    //LogText.text = "UID : " + User.UserId;
                    Debug.Log("로그인 성공! " + "UID: " + User.UserId + "  이메일 : " + User.Email);

                    CheckUserGroup(); //내담자면 내담자의 홈 씬을, 상담사면 상담사의 홈 씬을 불러온다.


                }
            }
            );
    }


    //유저 그룹 확인하는 함수.
    public void CheckUserGroup()
    {
        // RDB에서 데이터 읽기.
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(User.UserId)
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error...
                        print("데이터베이스 읽기 실패...");
                    }

                    // 성공적으로 데이터를 가져왔으면
                    if (task.IsCompleted)
                    {
                        // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                        DataSnapshot snapshot = task.Result;
                        print($"유저 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력

                        // 유저 그룹 확인.
                        print(snapshot.Child("userGroup").Value);

                        //내담자 로그인에 성공하면 내담자 홈 화면으로 이동
                        if ((string)snapshot.Child("userGroup").Value == client)
                        {
                            SceneManager.LoadScene("Client_Home_Scene");
                        }
                        else
                        {
                            //상담사 로그인에 성공하면 상담사 홈 화면으로 이동
                            SceneManager.LoadScene("Counselor_Home_Scene");
                        }


                    }
                });
    }





    public void LogOut()
    {
        firebaseAuth.SignOut();

        //LogText.text = "로그아웃";
        Debug.Log("로그아웃 되었습니다.");
        Debug.Log(User.UserId);
        //User = null;
        //Debug.Log(User.UserId);
    }




    // 테스트용 간편 회원가입
    public void RegisterTest()
    {
        // 이메일과 비밀번호로 회원가입
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(
            task =>
            {
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

