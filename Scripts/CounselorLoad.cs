using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;



public class CounselorLoad : MonoBehaviour
{
    public GameObject item;              // 리스트 상담사 버튼

    public Toggle familyT, relationshipT; // 상담사 전문분야 선택 토글
    public RawImage profileImg, profileImg2;   // 상담사 프로필 이미지

    public Transform parent;
    public GameObject prefab;
    public int num;

    public Text nameText, introduce, major; // 리스트 상담사 간략정보를 담을 변수

    public string selectDay;                // 선택 날짜를 담을 변수
    public Text month, selectDayT;          // 달력의 월, 선택한 날짜 텍스트 변수

    public Text selectTime;                 // 선택한 시간을 담을 텍스트 변수

    // 시간 선택 토글
    public Toggle nine, ten, eleven, twelve, thirteen, fourteen, fifteen, sixteen, seventeen;


    // 상담사 상세 프로필을 담을 변수
    public Text Cname1, Cname2, Cname3, Cmajor, Cintro, career1, career2, career3;

    public Text worry, worry2; // 고민내용을 담을 변수


    // 예약 신청내용 확인 팝업창 안의 텍스트
    public Text Cname4, selectDayT2, selectTime2;


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

                        num = (int)snapshot.ChildrenCount;
                        

                        foreach (DataSnapshot data in snapshot.Children)
                        {

                            // JSON은 사전 형태이기 때문에 딕셔너리 형으로 변환
                            IDictionary relationship = (IDictionary)data.Value;

                            Debug.Log("상담사: " + relationship["userGroup"] 
                                + "\n uid: " + relationship["uid"]
                                + "\n email: " + relationship["email"] 
                                + "\n pic: " + relationship["pic"] 
                                + "\n username: " + relationship["username"]
                                + "\n sex: " + relationship["sex"] 
                                + "\n intro: " + relationship["intro"]
                                + "\n family: " + relationship["family"] 
                                + "\n myself: " + relationship["myself"]
                                + "\n relationship: " + relationship["relationship"] 
                                + "\n romance: " + relationship["romance"]
                                + "\n work: " + relationship["work"] 
                                + "\n career: " + relationship["career"]
                                + "\n career1: " + relationship["career1"] 
                                + "\n career2: " + relationship["career2"]
                                + "\n career3: " + relationship["career3"] 
                                + "\n appointment: " + relationship["appointment"]
                                );

                            nameText.text = (string)relationship["username"];
                            introduce.text = (string)relationship["intro"];
                            major.text = "대인관계";

                            // 이미지 적용
                            StartCoroutine(GetTexture((string)relationship["pic"]));

                            item.SetActive(true);

                            Cname1.text = (string)relationship["username"];
                            Cname2.text = (string)relationship["username"];
                            Cname3.text = (string)relationship["username"];
                            Cmajor.text = "대인관계";
                            Cintro.text =  (string)relationship["intro"];
                            career1.text = (string)relationship["career1"];
                            career2.text = (string)relationship["career2"];
                            career3.text = (string)relationship["career3"];

                            
                                prefab = Instantiate(item, parent);
                                prefab.transform.position = new Vector3(1450, 367, 0);
                                print("생성");
                            

                            
                        }
                    }

                });
        }
        else
        {
            item.SetActive(false);

            Destroy(item);
        }

    

    }


    private void Function_fToggle(bool _bool)
    {
        Debug.Log("가족 선택 : " + _bool);


        if (_bool == true)
        {
            FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child("family")
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
                            IDictionary family = (IDictionary)data.Value;

                            Debug.Log("상담사: " + family["userGroup"]
                                + "\n uid: " + family["uid"]
                                + "\n email: " + family["email"]
                                + "\n pic: " + family["pic"]
                                + "\n username: " + family["username"]
                                + "\n sex: " + family["sex"]
                                + "\n intro: " + family["intro"]
                                + "\n family: " + family["family"]
                                + "\n myself: " + family["myself"]
                                + "\n relationship: " + family["relationship"]
                                + "\n romance: " + family["romance"]
                                + "\n work: " + family["work"]
                                + "\n career: " + family["career"]
                                + "\n career1: " + family["career1"]
                                + "\n career2: " + family["career2"]
                                + "\n career3: " + family["career3"]
                                + "\n appointment: " + family["appointment"]
                                );

                            nameText.text = (string)family["username"];
                            introduce.text = (string)family["intro"];
                            major.text = "가족";

                            // 이미지 적용
                            StartCoroutine(GetTexture((string)family["pic"]));

                            item.SetActive(true);

                            Cname1.text = (string)family["username"];
                            Cname2.text = (string)family["username"];
                            Cname3.text = (string)family["username"];
                            Cmajor.text = "가족";
                            Cintro.text = (string)family["intro"];
                            career1.text = (string)family["career1"];
                            career2.text = (string)family["career2"];
                            career3.text = (string)family["career3"];


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
            profileImg2.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }


    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        relationshipT.onValueChanged.AddListener(Function_Toggle);
        familyT.onValueChanged.AddListener(Function_fToggle);
        nine.onValueChanged.AddListener(nine_Toggle);

        ten.onValueChanged.AddListener(ten_Toggle);
        eleven.onValueChanged.AddListener(eleven_Toggle);
        twelve.onValueChanged.AddListener(twelve_Toggle);
        thirteen.onValueChanged.AddListener(thirteen_Toggle);
        fourteen.onValueChanged.AddListener(fourteen_Toggle);
        fifteen.onValueChanged.AddListener(fifteen_Toggle);
        sixteen.onValueChanged.AddListener(sixteen_Toggle);
        seventeen.onValueChanged.AddListener(seventeen_Toggle);
    }

    private void ResetFunction_UI()
    {
        relationshipT.onValueChanged.RemoveAllListeners();
        familyT.onValueChanged.RemoveAllListeners();
        nine.onValueChanged.RemoveAllListeners();

        ten.onValueChanged.RemoveAllListeners();
        eleven.onValueChanged.RemoveAllListeners();
        twelve.onValueChanged.RemoveAllListeners();
        thirteen.onValueChanged.RemoveAllListeners();
        fourteen.onValueChanged.RemoveAllListeners();
        fifteen.onValueChanged.RemoveAllListeners();
        sixteen.onValueChanged.RemoveAllListeners();
        seventeen.onValueChanged.RemoveAllListeners();
    }


    public void SelectDay()
    {
        if (DayBtn.thisDay == null)
        {
            DayBtn.thisDay = DateTime.Now.Day.ToString();
            print(DayBtn.thisDay);
        }
        else
        {
            print(DayBtn.thisDay);
            
        }
        string monthSub = month.text;
        selectDay = "2022." + monthSub.Substring(0, monthSub.Length - 1) + "." + DayBtn.thisDay + ".";
        selectDayT.text = selectDay;
    }

    private void nine_Toggle(bool _bool)
    {
        Debug.Log("9시 - 10시 : " + _bool);

        if (_bool == true)
        {
            print(nine.GetComponentInChildren<Text>().text);
            selectTime.text = nine.GetComponentInChildren<Text>().text;
        }
    }

    private void ten_Toggle(bool _bool)
    {
        Debug.Log("10시 - 11시 : " + _bool);

        if (_bool == true)
        {
            print(ten.GetComponentInChildren<Text>().text);
            selectTime.text = ten.GetComponentInChildren<Text>().text;
        }
    }

    private void eleven_Toggle(bool _bool)
    {
        Debug.Log("11시 - 12시 : " + _bool);

        if (_bool == true)
        {
            print(eleven.GetComponentInChildren<Text>().text);
            selectTime.text = eleven.GetComponentInChildren<Text>().text;
        }
    }

    private void twelve_Toggle(bool _bool)
    {
        Debug.Log("12시 - 13시 : " + _bool);

        if (_bool == true)
        {
            print(twelve.GetComponentInChildren<Text>().text);
            selectTime.text = twelve.GetComponentInChildren<Text>().text;
        }
    }

    private void thirteen_Toggle(bool _bool)
    {
        Debug.Log("13시 - 14시 : " + _bool);

        if (_bool == true)
        {
            print(thirteen.GetComponentInChildren<Text>().text);
            selectTime.text = thirteen.GetComponentInChildren<Text>().text;
        }
    }

    private void fourteen_Toggle(bool _bool)
    {
        Debug.Log("14시 - 15시 : " + _bool);

        if (_bool == true)
        {
            print(fourteen.GetComponentInChildren<Text>().text);
            selectTime.text = fourteen.GetComponentInChildren<Text>().text;
        }
    }


    private void fifteen_Toggle(bool _bool)
    {
        Debug.Log("15시 - 16시 : " + _bool);

        if (_bool == true)
        {
            print(fifteen.GetComponentInChildren<Text>().text);
            selectTime.text = fifteen.GetComponentInChildren<Text>().text;
        }
    }

    private void sixteen_Toggle(bool _bool)
    {
        Debug.Log("16시 - 17시 : " + _bool);

        if (_bool == true)
        {
            print(sixteen.GetComponentInChildren<Text>().text);
            selectTime.text = sixteen.GetComponentInChildren<Text>().text;
        }
    }

    private void seventeen_Toggle(bool _bool)
    {
        Debug.Log("17시 - 18시 : " + _bool);

        if (_bool == true)
        {
            print(seventeen.GetComponentInChildren<Text>().text);
            selectTime.text = seventeen.GetComponentInChildren<Text>().text;
        }
    }


    public void ReserBtn()
    {
        Cname4.text = Cname2.text;
        selectDayT2.text = selectDay;
        selectTime2.text = selectTime.text;
        worry2.text = worry.text;

    }

    
}



    





