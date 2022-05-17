using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;

public class EmotionCtrl : MonoBehaviourPunCallbacks //, IPunObservable
{
    public GameObject emotionCard;
    public GameObject re;
    public PhotonView PV;


    public Toggle tgHappy;
    public Toggle tgAnger;
    public Toggle tgSad;
    public Toggle tgSurprise;
    public Toggle tgHate;
    public Toggle tgHorror;

    public string currentEmotion;

    public string otherID;
    public Text stateText;


    // Start is called before the first frame update
    void Start()
    {
        emotionCard.transform.localScale = new Vector3(0, 0, 0);
    }



    public void OnEmotionCard()
    {
        OwnerTake();
        Debug.Log("감정카드 화면 열기");
        emotionCard.transform.localScale = new Vector3(1, 1, 1);
    }


    public void OffEmotionCard()
    {
        SelectEmotion();
        SetEmotionData();

        PV.RPC("CloseEmotion", RpcTarget.All);

        Invoke("GetData", 0.8f);

    }


    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    public void OpenEmotion()
    {
        Debug.Log("감정카드 화면 열기");
        emotionCard.transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void CloseEmotion()
    {
        Debug.Log("감정카드 화면 닫기");
        emotionCard.transform.localScale = new Vector3(0, 0, 0);
    }


    //소유자 양도
    public void OwnerGive()
    {
        Debug.Log("소유자 양도");
        this.photonView.RequestOwnership();
    }

    //마스터 권한 가져오기
    public void OwnerTake()
    {
        if (emotionCard.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("소유자 입니다.");
        }
        else
        {
            emotionCard.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("소유권을 가져옵니다.");
        }
    }

    //감정카드 선택
    public void SelectEmotion()
    {
        // 토글이 켜지면
        if (tgHappy.isOn)
        {
            //currentEmotion = tgHappy.GetComponentInChildren<Text>().text;
            currentEmotion = ("행복");
            Debug.Log("행복");

        }
        else if (tgAnger.isOn)
        {
            //currentEmotion = tgAnger.GetComponentInChildren<Text>().text;
            currentEmotion = ("분노");
            Debug.Log("분노");

        }
        else if (tgSad.isOn)
        {
            //currentEmotion = tgSad.GetComponentInChildren<Text>().text;
            currentEmotion = ("슬픔");
            Debug.Log("슬픔");

        }
        else if (tgSurprise.isOn)
        {
            //currentEmotion = tgSurprise.GetComponentInChildren<Text>().text;
            currentEmotion = ("놀라움");
            Debug.Log("놀라움");

        }
        else if (tgHate.isOn)
        {
            //currentEmotion = tgHate.GetComponentInChildren<Text>().text;
            currentEmotion = ("혐오");
            Debug.Log("혐오");

        }
        else if (tgHorror.isOn)
        {
            //currentEmotion = tgHorror.GetComponentInChildren<Text>().text;
            currentEmotion = ("공포");
            Debug.Log("공포");

        }

    }

    public void SetEmotionData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "감정", currentEmotion } }
        ,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패"));
    }


    public void GetData()
    {
        PV.RPC("GetEmotionData", RpcTarget.MasterClient);
        
    }

    [PunRPC]
    public void GetEmotionData()
    {
        var request = new GetUserDataRequest() { PlayFabId = otherID };

        PlayFabClientAPI.GetUserData(request, (result) =>
           stateText.text = (
             "김내담 님은 현재" + "\n"
             + result.Data["감정"].Value + "을" + "\n"
             + "느끼고 있습니다."),

           (error) => print("데이터 불러오기 실패"));

        Invoke("WaitRe", 0.8f);

    }


    public void WaitRe()
    {
        re.SetActive(true);
    }



}
