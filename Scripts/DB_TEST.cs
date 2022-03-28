using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;


//RDB 데이터 저장 테스트
public class DB_TEST : MonoBehaviour
{
    public DatabaseReference reference { get; set; }
    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용

    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
        new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // 데이터베이스 경로를 설정해 인스턴스를 초기화
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
    }


    // 내담자 정보를 담는 ClientUser 클래스
    class ClientUser
    {
        public string userGroup, email, username, sex, birth, job, meal, sleep, exercise;
        public bool appointment;
        public string counselorInCharge, appDay, appTime, worry;


        // 내담자 생성자
        public ClientUser(string userGroup, string email, string username, string sex, string birth , string job, 
            string meal, string sleep, string exercise,
            bool appointment, string counselorInCharge, string appDay, string appTime, string worry)
        {
            this.userGroup = userGroup;
            this.email = email;
            this.username = username;
            this.sex = sex;
            this.birth = birth;
            this.job = job;
            this.meal = meal;
            this.sleep = sleep;
            this.exercise = exercise;

            this.appointment = appointment;
            this.counselorInCharge = counselorInCharge;
            this.appDay = appDay;
            this.appTime = appTime;
            this.worry = worry;
        }
    }

   
    // 내담자 임시유저 저장 버튼
    public void TestBtn()
    {
        ClientUser clientUser = new ClientUser("내담자","asdf@gmail.com","김내담", "여","19950202", "학생","3끼", "7시간", "0회",
            false, "","","","");
        string json = JsonUtility.ToJson(clientUser);
        // 데이터를 json형태로 반환

        string key = reference.Child("ClientUser").Push().Key;
        // root의 자식 clientUser key 값을 추가해주는 것

        reference.Child("ClientUser").Child(key).SetRawJsonValueAsync(json);
        // 생성된 키의 자식으로 json데이터를 삽입
    }


    //


    // 상담사 정보를 담는 CounselorUser 클래스
    class CounselorUser
    {
        public string userGroup, email, pic, username, sex, intro, major, career1, career2, career3, day, time;
        public bool appointment;
        public string patient, appDay, appTime, worry;


        // 상담사 생성자
        public CounselorUser(string userGroup, string email, string pic, string username, string sex, string intro, string major,
            string career1, string career2, string career3, string day, string time,
            bool appointment, string patient, string appDay, string appTime, string worry)
        {
            this.userGroup = userGroup;
            this.email = email;
            this.pic = pic;
            this.username = username;
            this.sex = sex;
            this.intro = intro;
            this.major = major;
            this.career1 = career1;
            this.career2 = career2;
            this.career3 = career3;
            this.day = day;
            this.time = time;

            this.appointment = appointment;
            this.patient = patient;
            this.appDay = appDay;
            this.appTime = appTime;
            this.worry = worry;
        }
    }



    // 상담사 임시유저 저장 버튼
    public void TestCBtn()
    {
        CounselorUser counselorUser = new CounselorUser("상담사", "asdf@gmail.com", "이미지 경로", "김상담", "여", "안녕하세요. 김상담입니다.",
            "대인관계","경력1","경력2","경력3","월수금","", false, "", "", "", "");
        string json = JsonUtility.ToJson(counselorUser);
        // 데이터를 json형태로 반환

        string key = reference.Child("CounselorUser").Push().Key;
        // root의 자식 clientUser key 값을 추가해주는 것

        reference.Child("CounselorUser").Child(key).SetRawJsonValueAsync(json);
        // 생성된 키의 자식으로 json데이터를 삽입
    }



}
