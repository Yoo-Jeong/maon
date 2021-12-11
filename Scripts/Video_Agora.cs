using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class Video_Agora : MonoBehaviour
{
    VideoSurface myView;
    VideoSurface remoteView;

    [SerializeField]
    private string APP_ID = "";

    [SerializeField]
    private string TOKEN = "";

    [SerializeField]
    private string CHANNEL_NAME = "swuniverse";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;


    // Use this for initialization
    void Start()
    {
        CheckAppId();
        InitEngine();
        JoinChannel();
    }

    // Update is called once per frame
    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    void CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(APP_ID.Length > 10, "Please fill in your appId in VideoCanvas!!!!!");
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
    }


    void JoinChannel()
    {
        mRtcEngine.JoinChannelByKey(TOKEN, CHANNEL_NAME, "", 0);
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess!! channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        //makeVideoView(0);
        GameObject go = GameObject.Find("MyView_RawImage");
        myView = go.AddComponent<VideoSurface>();

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
