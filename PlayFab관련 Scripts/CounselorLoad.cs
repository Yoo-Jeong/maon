using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class CounselorLoad : MonoBehaviour
{

    public string otherID;
    public Text nameText, introduce, major;
   

    // Start is called before the first frame update
    void Start()
    {
        GetCounselorData(otherID);
        GetPlayerCombinedInfo();
        GetOtherID();
        //AddFriend(, otherID);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GetCounselorData(string PlayFabeId)
    {
        /* var request = new GetUserDataRequest() { PlayFabId = otherID };
         PlayFabClientAPI.GetUserData(request, (result) =>
         nameText.text = (
           result.Data["이름"].Value ),

         (error) => print("데이터 불러오기 실패"));*/

        //상담사 정보 불러오기.
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = otherID,
            Keys = null
        }, result => {
            Debug.Log("데이터 불러오기 성공:");
            if (result.Data == null || !result.Data.ContainsKey("이름")) Debug.Log("데이터 없음.");
            else { 
                Debug.Log("이름: " + result.Data["이름"].Value);
                nameText.text = result.Data["이름"].Value;
                introduce.text = result.Data["한 줄 소개"].Value;
                major.text = result.Data["전문분야"].Value;
            }
        }, (error) => {
            Debug.Log("데이터 불러오기 실패:");
            Debug.Log(error.GenerateErrorReport());
        });

    }


    public void GetPlayerCombinedInfo()
    {

        GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
        {
            PlayFabId = this.otherID,

            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {

                GetUserAccountInfo = true,
                GetUserInventory = true,
                GetUserVirtualCurrency = true,
                GetUserData = true,
                GetUserReadOnlyData = true,
                GetCharacterInventories = true,
                GetTitleData = true,
                GetPlayerStatistics = true

            }
        };

        PlayFabClientAPI.GetPlayerCombinedInfo(request, (result) => {
            if (result.InfoResultPayload == null)
            {
                Debug.Log("No result!");

            }
            else if (result.InfoResultPayload.UserVirtualCurrency == null)
            {

                Debug.Log("UserVirtualCurrency missing for PlayFabID: " + result.InfoResultPayload.AccountInfo.PlayFabId);


            }
            else
            {
                foreach (var item in result.InfoResultPayload.UserVirtualCurrency)
                {
                    Debug.Log(" " + item.Key + " =================== " + item.Value);
                }
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.ErrorMessage);
        }, null);
    }



    public void GetOtherID()
    {

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = nameText.text,
            //AuthenticationContext = Login.login.AuthenticationContext
        },
            result =>
            {
                Debug.Log(result.AccountInfo.PlayFabId);
                PlayFabClientAPI.GetUserData(new GetUserDataRequest
                {
                    PlayFabId = result.AccountInfo.PlayFabId,
                    //AuthenticationContext = Login.login.AuthenticationContext
                },
                result2 =>
                {
                    if (result2.Data != null && result2.Data.ContainsKey("전문분야"))
                    {
                        major.text = result2.Data["전문분야"].Value;
                        //networkLobbyManager.networkAddress = ipAddress;
                        //networkLobbyManager.StartClient();
                    }
                },
                error => { Debug.LogError(error.GenerateErrorReport()); });
            },
            error => { Debug.LogError(error.GenerateErrorReport()); });



        PlayFabClientAPI.GetPlayerSegments(new GetPlayerSegmentsRequest(),
    (result) =>
    {
        foreach (var segment in result.Segments)
        {
            Debug.Log(segment.Id + " : " + segment.Name);
            
        }
    },
    (error) =>
    {
        Debug.LogError(error.GenerateErrorReport());
    });

    }

    
    //친구
    void DisplayFriends(List<FriendInfo> friendsCache) { friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId)); }
    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }


    List<FriendInfo> _friends = null;

    void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }




    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }




}
