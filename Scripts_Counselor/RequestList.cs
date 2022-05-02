using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestList : MonoBehaviour
{

    public Transform parent;                        // 신청 온 예약 프리팹이 생성될 위치의 부모객체의 위치
    public GameObject requestPrefab, requestClone;  // 신청 온 예약 프리팹, 복제된 신청 온 예약 프리팹

    public List<GameObject> requestCloneList = new List<GameObject>();    //생성된 프리팹을 담을 리스트.
    public Text[] newRequestData;                                         // 프리팹 안의 텍스트타입 게임오브젝트를 담을 배열.
    public List<Button> btnList = new List<Button>();                     // 생성된 프리팹들의 버튼들을 담을 리스트.


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
