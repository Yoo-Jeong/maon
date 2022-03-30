using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;



public class CounselorLoad : MonoBehaviour
{
    public GameObject item;

    public Toggle relationshipT;
    public RawImage profileImg;

    public Text nameText, introduce, major;

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
       new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");


        SetFunction_UI();
        item.SetActive(false);

    }

    private void Function_Toggle(bool _bool)
    {
        Debug.Log("대인관계 선택 : " + _bool);


        if (_bool == true)
        {
            FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child("relationship")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {


                    if (task.IsFaulted)
                    {
                        // Handle the error...
                        print("실패...");
                    }

                    // 성공적으로 데이터를 가져왔으면
                    if (task.IsCompleted)
                    {
                        // 데이터를 출력하고자 할때는 Snapshot 객체 사용함
                        DataSnapshot snapshot = task.Result;
                        print($"데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                        foreach (DataSnapshot data in snapshot.Children)
                        {
                            // JSON은 사전 형태이기 때문에 딕셔너리 형으로 변환
                            IDictionary relationship = (IDictionary)data.Value;
                            Debug.Log("상담사: " + relationship["userGroup"] + ", email: " + relationship["email"]
                                + ", pic: " + relationship["pic"] + ", username: " + relationship["username"]
                                + ", sex: " + relationship["sex"] + ", intro: " + relationship["intro"]
                                + ", family: " + relationship["family"] + ", myself: " + relationship["myself"]
                                + ", relationship: " + relationship["relationship"] + ", romance: " + relationship["romance"]
                                + ", work: " + relationship["work"] + ", career: " + relationship["career"]
                                + ", career1: " + relationship["career1"] + ", career2: " + relationship["career2"]
                                + ", career3: " + relationship["career3"] + ", day: " + relationship["day"]
                                + ", time: " + relationship["time"] + ", appointment: " + relationship["appointment"]
                                + ", patient: " + relationship["patient"] + ", appDay: " + relationship["appDay"]
                                + ", appTime: " + relationship["appTime"] + ", worry: " + relationship["worry"]
                                );

                            nameText.text = (string)relationship["username"];
                            introduce.text = (string)relationship["intro"];
                            major.text = "대인관계";

                            // 이미지 적용
                            StartCoroutine(GetTexture((string)relationship["pic"]));

                            item.SetActive(true);


                        }
                    }

                });
        }
        else
        {
            item.SetActive(false);
        }


    }


    // 웹url에서 이미지를 가져와 RawImage에 적용시키는 함수.
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            profileImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }


    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        relationshipT.onValueChanged.AddListener(Function_Toggle);
    }

    private void ResetFunction_UI()
    {
        relationshipT.onValueChanged.RemoveAllListeners();
    }



}





