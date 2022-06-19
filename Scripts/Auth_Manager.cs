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
    public static FirebaseAuth auth;         //인증을 관리할 객체
    public static FirebaseUser user;  //전역 인증 객체

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

   
    //로그인 UI
    public Canvas authCanvas;
    public InputField emailField;
    public InputField passwordField;

    public Button loginBtn;
    public Button logoutBtn;

    public Button joinBtn;

    private static Auth_Manager instance = null;


    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 Auth_Manager 인스턴스가 담겨있지 않다면, 자신을 넣어준다
            instance = this;

            Debug.Log("인증객체 생성");
            
            //씬 전환이 되더라도 파괴되지 않게 한다
            DontDestroyOnLoad(this.gameObject);

            
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 Auth_Manager가 존재할 수도 있다
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해준다
            //이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 Auth_Manager)을 삭제해준다
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {          
        instance.emailField.Select();   //처음은 이메일 Input Field를 선택하도록 한다.

        authCanvas.enabled = true;
        InitializeFirebase();           //파이어베이스 인증관련 초기화
        InitUI();                       //로그인관련 UI 초기화
    }

    public void Update()
    {
        ClickKey();
    }


    //Auth_Manager 인스턴스에 접근할 수 있는 프로퍼티
    public static Auth_Manager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    void InitializeFirebase()
    {       
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;    //인증 객체 초기화
        auth.StateChanged += AuthStateChanged;                //유저의 로그인 정보에 변경이 생기면 실행되도록 이벤트를 걸어준다(이벤트 핸들러)
        

        Debug.Log("파이어베이스 인증관련 초기화 완료");
    }


    //게임오브젝트가 다시 생성됐을때 인풋필드와 버튼 찾아서 넣기
    public void InitUI()
    {
        authCanvas = instance.authCanvas;
        emailField = instance.emailField;
        passwordField = instance.passwordField;
        loginBtn = instance.loginBtn;
        loginBtn.onClick.AddListener(() => { LogInWithEmail(emailField.text, passwordField.text); });
        logoutBtn.onClick.AddListener(Logout);

        joinBtn.onClick.AddListener(GoJoin);

        Debug.Log("Auth_Manager 관련 인풋필드 버튼 초기화 완료");
    }


    //유저의 로그인 정보에 변경이 생기면 실행되는 이벤트 핸들러 구현
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("로그아웃 성공 " + user.UserId);
                SceneManager.LoadScene("LogIn_Scene");

                emailField.text = null;
                passwordField.text = null;

                authCanvas.enabled = true;

                instance.emailField.Select();   //이메일 Input Field를 선택하도록 한다
          
            }

            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("로그인 성공! : " + user.Email + " / Uid: " + user.UserId);
                CheckUserGroup(); //내담자면 내담자의 홈 씬을, 상담사면 상담사의 홈 씬을 불러온다.

                authCanvas.enabled = false;
            }
        }
    }



    //이메일 로그인 함수
    public void LogInWithEmail(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {             
                if (task.IsFaulted)
                {
                    Debug.LogError("로그인 실패: " + task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("로그인 실패");             
                }
                else
                {
                    user = task.Result;          

                }
            }
            );
    }



    //로그아웃 함수
    public void Logout()
    {
        auth.SignOut();
    }





    //유저 그룹 확인하는 함수.
    public void CheckUserGroup()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        string client = "내담자";

        // RDB에서 데이터 읽기.
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(user.UserId)
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



    //tap키 누르면 패스워드 필드로 이동, enter키 누르면 로그인버튼이 클릭되는 함수
    public void ClickKey() {

        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 emailField 객체를 선택
            instance.emailField.Select();
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab키는 아래의 passwordField 객체를 선택
            instance.passwordField.Select();
            
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 엔터키를 치면 로그인 버튼을 클릭
            loginBtn.onClick.Invoke();
            Debug.Log("로그인 버튼 눌림!");
        }
     
    }



    public void OpenAuthCanvas()
    {
        authCanvas.enabled = true;
    }

    public void OnAuthCanvas()
    {
        emailField.text = null;
        passwordField.text = null;

        authCanvas.enabled = true;

        instance.emailField.Select();   //이메일 Input Field를 선택하도록 한다
    }

   
    public void OffAuthCanvas()
    {
        authCanvas.enabled = false;
    }


    public void GoJoin()
    {
        authCanvas.enabled = false;
        SceneManager.LoadScene("Select_Scene");
    }


    // 테스트용 간편 회원가입(실제 사용x)
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

