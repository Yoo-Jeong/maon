using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class ProfileCheck : MonoBehaviour
{
    public string otherID;
    public Text LogText;

    public void GetProfileData()
    {
        var request = new GetUserDataRequest() { PlayFabId = otherID };

        //플레이어 데이터 전부 불러오기
        //PlayFabClientAPI.GetUserData(request, (result) =>
        //{ foreach (var eachData in result.Data) LogText.text += eachData.Key + " : " + eachData.Value.Value + "\n"; },

        PlayFabClientAPI.GetUserData(request, (result) =>
         LogText.text = (
           "● 이름: " + result.Data["이름"].Value + "\n"
         + "● 성별: " + result.Data["성별"].Value + "\n"
         + "● 생년월일: " + result.Data["생년월일"].Value + "\n"
         + "● 직업: " + result.Data["직업"].Value + "\n"

         + "● 생활 패턴" + "\n"
         + "   -식사           " + result.Data["식사"].Value + "\n"
         + "   -수면           " + result.Data["수면"].Value + "\n"
         + "   -운동           " + result.Data["운동"].Value + "\n"),

         (error) => print("데이터 불러오기 실패"));
    }


    public void SetProfileData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "이름", "김내담" }, { "성별", "여" },
            { "생년월일", "2000.01.01" },{ "직업", "학생" },{ "식사", "3 끼" },{ "수면", "7 시간" },{ "운동", "안함" },}
        ,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패"));
    }

}
