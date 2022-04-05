using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
//using Firebase;
//using Firebase.Firestore;
//using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using Object = UnityEngine.Object;


//토큰서버에서 토큰을 가져온다. !!로컬서버라 나(유정)이 토큰서버를 실행하고 있어야 돌아감!!
public class Video_TokenAgora : MonoBehaviour
{
    //FirebaseFirestore db;

    VideoSurface myView;
    VideoSurface remoteView;

    [SerializeField]
    private string APP_ID = "6b17d6a455dc4642a36f39abbba84659";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;
    private static string channelName = "swuniverse";
    private static string channelToken = "";
    //private static string tokenBase = "http://localhost:8080";
    private static string tokenBase = "http://3Bears.iptime.org:18080";
    private CONNECTION_STATE_TYPE state = CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED;

    // Use this for initialization
    void Start()
    {
        //db = FirebaseFirestore.DefaultInstance;

        CheckAppId();
        InitEngine();
        

        JoinChannel();
    }

    

    void RenewOrJoinToken(string newToken)
    {
        Video_TokenAgora.channelToken = newToken;
        if (state == CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED
            || state == CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED
            || state == CONNECTION_STATE_TYPE.CONNECTION_STATE_FAILED
        )
        {
            // 연결되어 있지 않은 경우 채널 참가
            JoinChannel();
        }
        else
        {
            // 이미 연결되어 있는 경우 토큰 업데이트
            UpdateToken();
        }
    }

    ////파이어베이스DB에서 RTC토큰값을 받아와서 채널에 참가
    //void RenewOrJoinToken(string newToken)
    //{
    //    CollectionReference rtcsRef = db.Collection("RTC");
    //    rtcsRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //    {
    //        QuerySnapshot snapshot = task.Result;
    //        foreach (DocumentSnapshot document in snapshot.Documents)
    //        {
    //            Debug.Log(String.Format("rtc: {0}", document.Id));
    //            Dictionary<string, object> documentDictionary = document.ToDictionary();

    //            Debug.Log(String.Format("rtc: {0}", documentDictionary["rtc"] + " 토큰"));
    //            newToken = documentDictionary["rtc"] + "";
    //            Video_TokenAgora.channelToken = newToken;

    //        }
    //        Debug.Log("RTC토큰 읽기 완료");
    //    });

    //    if (state == CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED
    //        || state == CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED
    //        || state == CONNECTION_STATE_TYPE.CONNECTION_STATE_FAILED
    //    )
    //    {
    //        // 연결되어 있지 않은 경우 채널 참가
    //        JoinChannel();
    //    }
    //    else
    //    {
    //        // 이미 연결되어 있는 경우 토큰 업데이트
    //        UpdateToken();
    //    }


    //}



    // Update is called once per frame
    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    void UpdateToken()
    {
        mRtcEngine.RenewToken(Video_TokenAgora.channelToken);
    }

    void CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(APP_ID.Length > 10, "appID를 입력해주세요.");
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetLogFile("log.txt");
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        mRtcEngine.OnWarning += OnSDKWarningHandler;
        mRtcEngine.OnError += OnSDKErrorHandler;
        mRtcEngine.OnConnectionLost += OnConnectionLostHandler;
        mRtcEngine.OnUserJoined += OnUserJoinedHandler;
        mRtcEngine.OnUserOffline += OnUserOfflineHandler;

        //Set the token expiration handler
        mRtcEngine.OnTokenPrivilegeWillExpire += OnTokenPrivilegeWillExpireHandler;
        mRtcEngine.OnConnectionStateChanged += OnConnectionStateChangedHandler;

       
    }

    void JoinChannel()
    {
        if (string.IsNullOrEmpty(channelToken))
        {
            StartCoroutine(HelperClass.FetchToken(tokenBase, channelName, 0, this.RenewOrJoinToken));
            return;
        }
        mRtcEngine.JoinChannelByKey(channelToken, channelName, "", 0);

        //내 웹캠 출력
        GameObject go = GameObject.Find("MyView_RawImage");
        myView = go.AddComponent<VideoSurface>();
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        print("성공2");
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        logger.UpdateLog(string.Format("New Token: {0}", Video_TokenAgora.channelToken));
        // HelperClass.FetchToken(tokenBase, channelName, 0, this.RenewOrJoinToken);

    }

    public void Leave()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideo();
        mRtcEngine.DisableVideoObserver();

    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
        Debug.Log("상담소 퇴장 완료");
        DestroyVideoView(0);
    }

    void OnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
        makeVideoView(uid);
    }

    void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(uid);
    }

    void OnTokenPrivilegeWillExpireHandler(string token)
    {
        StartCoroutine(HelperClass.FetchToken(tokenBase, channelName, 0, this.RenewOrJoinToken));
    }

    void OnConnectionStateChangedHandler(CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason)
    {
        this.state = state;
        logger.UpdateLog(string.Format("ConnectionState changed {0}, reason: ${1}", state, reason));
    }

    void OnSDKWarningHandler(int warn, string msg)
    {
        logger.UpdateLog(string.Format("OnSDKWarning warn: {0}, msg: {1}", warn, msg));
    }

    void OnSDKErrorHandler(int error, string msg)
    {
        logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, msg));
    }

    void OnConnectionLostHandler()
    {
        logger.UpdateLog(string.Format("OnConnectionLost "));
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    private void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }

    private void makeVideoView(uint uid)
    {
        GameObject go = GameObject.Find("RemoteView_RawImage");

        if (remoteView == null)
        {
            remoteView = go.AddComponent<VideoSurface>();
        }

        remoteView.SetForUser(uid);
        remoteView.SetEnable(true);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
    }


}