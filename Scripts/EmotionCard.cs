using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class EmotionCard : MonoBehaviour
{

    public Toggle anger;
    public Toggle blue;
    public Toggle sad;
    public Toggle jealousy;

    private string emotion;
    private string emotion2;

    public string myID;

   public void emotionBtn()
    {
       if (anger.isOn)
        {
            emotion = anger.GetComponentInChildren<Text>().text;
            Debug.Log("분노");
        }

        if (blue.isOn)
        {
            emotion2 = blue.GetComponentInChildren<Text>().text;
            Debug.Log("우울");
        }


    }


    public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "감정", emotion }, { "감정2", emotion2 } }
        , Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패"));
    }

    //public void GetData()
    //{
    //    var request = new GetUserDataRequest() { PlayFabId = myID };
    //    PlayFabClientAPI.GetUserData(request, (result) => 
    //    { foreach (var eachData in result.Data) LogText.text += eachData.Key + " : " + eachData.Value.Value + "\n"; }, (error) => print("데이터 불러오기 실패"));
    //}
}
