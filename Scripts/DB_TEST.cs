using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;


//RDB 데이터 저장 테스트
public class DB_TEST : MonoBehaviour
{

    // 라이브러리를 통해 불러온 FirebaseDatabase 관련객체를 선언해서 사용
    public DatabaseReference reference { get; set; }
    
    
    void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
        new System.Uri("https://swuniverse-d9641-default-rtdb.firebaseio.com/");


        // 데이터베이스 경로를 설정해 인스턴스를 초기화
        // Database의 특정지점을 가리킬 수 있다, 그 중 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }


    // 내담자 정보를 담는 ClientUser 클래스.
    class ClientUser
    {

        // 기본 정보 : 내담자그룹, 이메일, 이름, 성별, 생년월일, 직업, 식사횟수, 수면시간, 운동횟수
        public string userGroup, email, username, sex, birth, job, meal, sleep, exercise;

        public bool appointment; // 예약여부

        // 예약이 있다면 : 예약한 상담사이름, 예약날짜, 예약시간, 고민내용
        public string counselorInCharge, appDay, appTime, worry;


        // 내담자 생성자.
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

   
    // 내담자 임시유저 저장 버튼.
    public void TestBtn()
    {
        ClientUser clientUser = new ClientUser("내담자","asdf@gmail.com","김내담", "여","19950202", "학생","3끼", "7시간", "0회",
            false, "","","","");

        // 데이터를 json형태로 반환
        string json = JsonUtility.ToJson(clientUser);

        // root의 자식 clientUser key 값을 추가.
        string key = reference.Child("ClientUser").Push().Key;

        // 생성된 키의 자식으로 json데이터를 삽입
        reference.Child("ClientUser").Child(key).SetRawJsonValueAsync(json);
        
    }


    //
    //
    //

    // 상담사 정보를 담는 CounselorUser 클래스.
    class CounselorUser
    {
        // 기본 정보 : 상담사그룹, 이메일, 프로필이미지 경로, 이름, 성별, 한줄소개, 경력사항1, 경력사항2, 경력사항3, 상담가능요일, 상담가능시간
        public string userGroup, email, pic, username, sex, intro, career1, career2, career3, day, time;

        // 전문 분야 : 가족, 나 자신, 대인관계, 연애, 직장, 진로/취직
        public bool family, myself, relationship, romance, work, career;

        public bool appointment;  // 예약여부

        // 예약이 있다면 : 내담자이름, 예약날짜, 예약시간, 고민내용
        public string patient, appDay, appTime, worry;


        // 상담사 생성자.
        public CounselorUser(string userGroup, string email, string pic, string username, string sex, string intro,
            bool family, bool myself, bool relationship, bool romance, bool work, bool career,
            string career1, string career2, string career3, string day, string time,
            bool appointment, string patient, string appDay, string appTime, string worry)
        {
            this.userGroup = userGroup;
            this.email = email;
            this.pic = pic;
            this.username = username;
            this.sex = sex;
            this.intro = intro;

            this.family = family;
            this.myself = myself;
            this.relationship = relationship;
            this.romance = romance;
            this.work = work;
            this.career = career;


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


    // 상담사 임시유저 저장 버튼.
    public void TestCBtn()
    {
        CounselorUser counselorUser = new CounselorUser("상담사", "asdf@gmail.com", "이미지 경로", "김상담", "여", 
            "책임감과 헌신적인 자세로 내담자의 마음을 치유하는 심리학 박사입니다.",
            false, false, true, false, false, false,
            "상담심리사 1급 (한국심리학회)", "정신건강임상심리사 1급 (보건복지부)", "전) 한양대병원 정신건강의학과 심리평가 및 상담", 
            "월수금","", false, "", "", "", "");


        // 데이터를 json형태로 반환.
        string json = JsonUtility.ToJson(counselorUser);

        // root의 자식 clientUser key 값을 추가.
        string key = reference.Child("CounselorUsers").Push().Key;



        // 해당 전문 분야 하위에 상담사 데이터 저장. (전문 분야로 쿼리하기 위해..?)
        if (counselorUser.family)
        {
            reference.Child("CounselorUsers").Child("family").Child(key).SetRawJsonValueAsync(json);

        }
        else if (counselorUser.myself)
        {
            reference.Child("CounselorUsers").Child("myself").Child(key).SetRawJsonValueAsync(json);
        }
        else if (counselorUser.relationship)
        {
            reference.Child("CounselorUsers").Child("relationship").Child(key).SetRawJsonValueAsync(json);
        }
        else if (counselorUser.romance)
        {
            reference.Child("CounselorUsers").Child("romance").Child(key).SetRawJsonValueAsync(json);
        }
        else if (counselorUser.work)
        {
            reference.Child("CounselorUsers").Child("work").Child(key).SetRawJsonValueAsync(json);
        }
        else
        {
            reference.Child("CounselorUsers").Child("career").Child(key).SetRawJsonValueAsync(json);
        }


    }



}
