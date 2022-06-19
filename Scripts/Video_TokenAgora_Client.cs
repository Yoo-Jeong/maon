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

using Photon.Pun;
using Photon.Realtime;


//토큰서버에서 토큰을 가져온다. !!로컬서버라 나(유정)이 토큰서버를 실행하고 있어야 돌아감!!
public class Video_TokenAgora_Client : MonoBehaviour
{
    //FirebaseFirestore db;

    VideoSurface myView;
    VideoSurface remoteView;

    [SerializeField]
    private string APP_ID = "";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;
    private static string channelName = "swuniverse";
    private static string channelToken = "";
    private static string tokenBase = "https://maon-server.run.goorm.io";
    private CONNECTION_STATE_TYPE state = CONNECTION_STATE_TYPE.CONNECTION_STATE_DISCONNECTED;
 

    //상담사 캐릭터 애니메이션
    public Animator anim_f, anim_m;
    public GameObject char_f, char_m;

    //상담 레포트 도착안내 팝업
    public GameObject reportPopup;

    public GameObject netPopup;


    public Slider sensitivity;  //볼륨 민감도 슬라이더
    // 상담사 캐릭터가 말하는 애니메이션을 플레이하는 기준인 볼륨 크기
    public float VolumeSensitivity = 30;
    public float max = 200;
    public float min = 30;


    public void Start()
    {
        sensitivity.onValueChanged.AddListener(ChangeVolumeSensitivity);

        netPopup.SetActive(false);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결이 안되었을 때 
            Debug.Log("인터넷 연결 안됨");

            netPopup.SetActive(true);

        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
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


    public void ClickInterior()
    {
        CheckAppId();
        InitEngine();
        JoinChannel();
    }


    void RenewOrJoinToken(string newToken)
    {
        Video_TokenAgora_Client.channelToken = newToken;
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
   

    // Update is called once per frame
    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    void UpdateToken()
    {
        mRtcEngine.RenewToken(Video_TokenAgora_Client.channelToken);
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


        //마이크 볼륨을 확인하기 위함.
        mRtcEngine.EnableAudioVolumeIndication(200, 3);
        mRtcEngine.OnVolumeIndication += VolumeIndicationHandler;

    }

    void JoinChannel()
    {
        if (string.IsNullOrEmpty(channelToken))
        {
            StartCoroutine(HelperClass.FetchToken(tokenBase, channelName, 0, this.RenewOrJoinToken));
            return;
        }
        mRtcEngine.JoinChannelByKey(channelToken, channelName, "", 0);

   
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk 버전: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("채널입장 성공 channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        logger.UpdateLog(string.Format("새로운 Token: {0}", Video_TokenAgora_Client.channelToken));
        // HelperClass.FetchToken(tokenBase, channelName, 0, this.RenewOrJoinToken);      

        //내담자 자신의 웹캠 화면 
        GameObject go = GameObject.Find("MyView_RawImage");
        myView = go.AddComponent<VideoSurface>();

    }

    public void Leave()
    {
        Debug.Log("OnApplicationQuit : 아고라 퇴장");
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }

    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
        Debug.Log("상담소 퇴장 완료");
        DestroyVideoView(0);
    }

    //앱이나 에디터가 닫힐 때 인스턴스를 삭제
    void OnApplicationQuit()
    {
        Debug.Log("에디터 닫음  : OnApplicationQuit");
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    void OnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("다른 유저 입장  uid: ${0} elapsed: ${1}", uid, elapsed));
        makeVideoView(uid);
    }

    void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("다른 유저 퇴장 uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(uid);

        OpenEndReport_popup(); //상담종료 레포트 전달

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
        if (go)
        {
            remoteView = go.AddComponent<VideoSurface>();
            Debug.Log(go.name + " 게임오브젝트 발견");
        }
        else
        {
            Debug.Log("게임오브젝트를 찾을 수 없습니다.");
        }

        remoteView.SetForUser(uid);
        remoteView.SetEnable(true);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
    }



    //마이크 볼륨을 확인해서 말하는 중인지를 알려주는 함수
    protected void VolumeIndicationHandler(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume)
    {
        if (speakerNumber == 0 || speakers == null)
        {
            //Debug.Log(string.Format("onVolumeIndication only local {0}", totalVolume));
        }

        for (int idx = 0; idx < speakerNumber; idx++)
        {
            string volumeIndicationMessage = string.Format("{0} onVolumeIndication {1} {2}",
                speakerNumber, speakers[idx].uid, speakers[idx].volume);
            //Debug.Log(volumeIndicationMessage);

           
            if (speakerNumber != 1 || speakers[0].uid != 0 )
            {
                if (speakers[0].volume > VolumeSensitivity)
                {
                    
                    Debug.Log(speakerNumber + " / " + speakers[0].uid
                        + " 상담사가 말하는 중입니다." + "  볼륨 크기: " + speakers[0].volume);
                    PlayTalk(anim_f, anim_m);
                }
                else
                {
                    //Debug.Log(speakerNumber + " / " + speakers[0].uid + " 상담사가 말하는 중이 아닙니다.");
                    StopTalk(anim_f, anim_m);

                }
            }

            
        }
    }



    //상담사 캐릭터가 말하는 애니메이션을 플레이하는 기준인 볼륨 민감도를 슬라이더로 조절하는 함수
    public void ChangeVolumeSensitivity(float volume)
    {
        sensitivity.value = volume;
        VolumeSensitivity = sensitivity.value;
        Debug.Log("볼륨 조건: " + VolumeSensitivity);
    }


    private void ResetFunction_UI()
    {
        sensitivity.onValueChanged.RemoveAllListeners();
        sensitivity.maxValue = max;
        sensitivity.minValue = min;
        sensitivity.wholeNumbers = true;
    }


    //상담사캐릭터 애니메이션관련 시작
    //포톤 마스터 권한
    public void OwnerTake()
    {
        if (char_f.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("소유자 입니다.");
        }
        else
        {
            char_f.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("소유권을 가져옵니다.");
        }


        if (char_m.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("소유자 입니다.");
        }
        else
        {
            char_m.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("소유권을 가져옵니다.");
        }
    }

    


    public void PlayTalk(Animator animF, Animator animM)
    {
      
        Debug.Log("Talk 애니메이션 플레이");
        animF.SetBool("talk", true);
        animM.SetBool("talk", true);
        Invoke("StopTalk", 0.2f);
    }

    public void StopTalk(Animator animF, Animator animM)
    {
        animF.SetBool("talk", false);
        animM.SetBool("talk", false);
    }



    public void OpenEndReport_popup()
    {
        Debug.Log("상담종료: 상담레포트를 전달합니다.");
        reportPopup.SetActive(true);
        View_Controller.counselPopup.enabled = true;
     
    }

}