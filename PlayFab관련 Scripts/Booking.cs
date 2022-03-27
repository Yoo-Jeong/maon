using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Booking : MonoBehaviour
{
    public Text counselorName, counselDay, counselTime, concern, displayname;
    public Text counselorName2, counselDay2, counselTime2, concern2;

    public GameObject reserOK, reserNO;

    public string reservationCheck = "1";

    public void Start()
    {
        reserOK = GameObject.Find("예약있음");
        reserNO = GameObject.Find("예약없음");

        reserNO.SetActive(false);
        reserOK.SetActive(false);

        Invoke("get", 1);
    }

    public void get()
    {
        displayname = GameObject.Find("displayname").GetComponent<Text>();
        displayname.text = PlayFabManager.displayname;

        if ( PlayFabManager.reservation == reservationCheck)
        {
            print(PlayFabManager.reservation);
            reserOK.SetActive(true);
            reserNO.SetActive(false);

            print(PlayFabManager.counselor);
            counselorName = GameObject.Find("counselorName").GetComponent<Text>();
            counselorName.text = PlayFabManager.counselor;
            counselorName2 = GameObject.Find("counselorName2").GetComponent<Text>();
            counselorName2.text = PlayFabManager.counselor;

            counselDay = GameObject.Find("counselDay").GetComponent<Text>();
            counselDay.text = PlayFabManager.counselDay;
            counselDay2 = GameObject.Find("counselDay2").GetComponent<Text>();
            counselDay2.text = PlayFabManager.counselDay;

            counselTime = GameObject.Find("counselTime").GetComponent<Text>();
            counselTime.text = PlayFabManager.counselTime;
            counselTime2 = GameObject.Find("counselTime2").GetComponent<Text>();
            counselTime2.text = PlayFabManager.counselTime;

            concern = GameObject.Find("concern").GetComponent<Text>();
            concern.text = PlayFabManager.concern;

        }
        else
        {
            reserOK.SetActive(false);
            reserNO.SetActive(true);
        }

    }

    public void SetBookingData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "예약여부", "1" }, { "상담사", "김상담" },
            { "상담날짜", "2022.03.24" },{ "상담시간", "13:00 - 14:00" },{ "고민내용", "우울" }}
        ,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("예약 데이터 저장 성공"), (error) => print("예약 데이터 저장 실패"));

        //PlayFabManager.GetUserData(PlayFabManager.email);
        //counselorName.text = PlayFabManager.counselor;
    }

    public static void SetDelBookingData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "예약여부", "0" }, { "상담사", "" },
            { "상담날짜", "" },{ "상담시간", "" },{ "고민내용", "" }}
        ,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("예약 데이터 삭제 성공"), (error) => print("예약 데이터 삭제 실패"));

        //PlayFabManager.GetUserData(PlayFabManager.email);
        //counselorName.text = PlayFabManager.counselor;
    }


}
