using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class EmotionCtrl : MonoBehaviourPunCallbacks //, IPunObservable
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


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


    public Text ClientDisplyName;
    public string todayClientName;
    public string todayClientUid;


    public string currentCard;


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;

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
    {/*
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "감정", currentEmotion } }
        ,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패"));*/

        // 감정카드 선택하면 업데이트
        Dictionary<string, object> emotionCard = new Dictionary<string, object>();
        emotionCard["emotionCard"] = currentEmotion;
        reference.Child("ClientUsers").Child(Auth_Manager.user.UserId).UpdateChildrenAsync(emotionCard);
       


    }


    public void GetData()
    {
        PV.RPC("GetEmotionData", RpcTarget.MasterClient);
        
    }

    [PunRPC]
    public void GetEmotionData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("ClientUsers").Child(todayClientUid)
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
                            print($"내담자 정보 레코드 개수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                           /*            //좌측 상단 내담자 이름 표시.
                                       ClientDisplyName.text = snapshot.Child("username").Value.ToString();


                                       Debug.Log("내담자: " + snapshot.Child("userGroup").Value
                                             + "\n uid: " + snapshot.Child("uid").Value
                                             + "\n email: " + snapshot.Child("email").Value
                                             + "\n username: " + snapshot.Child("username").Value
                                             + "\n sex: " + snapshot.Child("sex").Value
                                             + "\n birth: " + snapshot.Child("birth").Value
                                             + "\n job: " + snapshot.Child("job").Value
                                             + "\n meal: " + snapshot.Child("meal").Value
                                             + "\n sleep: " + snapshot.Child("sleep").Value
                                             + "\n exercise: " + snapshot.Child("exercise").Value
                                             + "\n emotionCard: " + snapshot.Child("emotionCard").Value
                                             );*/


                            currentCard = (string)(snapshot.Child("emotionCard").Value);

                            stateText.text = (todayClientName + " 님은 현재" + "\n" + "\n"
                   + currentCard + " 의 감정을" + "\n" + "\n"
                   + "느끼고 있습니다.");

                            print("내담자 기본정보 불러오기 완료");
                        }

                    });



        

        WaitRe();


    }


    public void WaitRe()
    {
        re.SetActive(true);
    }

    // 방 퇴장
    public void OutRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("방 퇴장");
    }

}
