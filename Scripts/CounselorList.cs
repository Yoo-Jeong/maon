using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;


using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class CounselorList : MonoBehaviour
{

    public static List<string> relationshipUid = new List<string>();

    public static List<string> relationshipUsername = new List<string>();
    public static List<string> relationshipIntro = new List<string>();
    public static List<bool> relationshipMajor = new List<bool>();

    public static List<string> relationshipPic = new List<string>();
    public static List<string> relationshipCareer1 = new List<string>();
    public static List<string> relationshipCareer2 = new List<string>();
    public static List<string> relationshipCareer3 = new List<string>();




    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
     new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        RelationshipCounselorData();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RelationshipCounselorData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("CounselorUsers").Child("대인관계")
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
                            print($"상담사 목록 데이터 레코드 갯수 : {snapshot.ChildrenCount}"); //데이터 건수 출력


                            //반복 시작
                            foreach (DataSnapshot data in snapshot.Children)
                            {

                                // JSON은 사전 형태이기 때문에 딕셔너리 형으로 변환
                                IDictionary relationship = (IDictionary)data.Value;

                                /*    Debug.Log("상담사: " + relationship["userGroup"]
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
                                    );*/


                                relationshipUid.Add((string)relationship["uid"]);
                                relationshipUsername.Add((string)relationship["username"]);
                                relationshipIntro.Add((string)relationship["intro"]);
                                relationshipMajor.Add((bool)relationship["relationship"]);

                                relationshipPic.Add((string)relationship["pic"]);
                                relationshipCareer1.Add((string)relationship["career1"]);
                                relationshipCareer2.Add((string)relationship["career2"]);
                                relationshipCareer3.Add((string)relationship["career3"]);


                            }



                            print("리스트 완료");

                            for (int i = 0; i < relationshipUsername.Count; i++)
                            {
                                print("대인관계 저장 완료1/ "+ " i=" + i + "/ 데이터 : " + relationshipUsername[i]);
                            }


                        }

                    });



    }

} // end.

