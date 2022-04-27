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

        public bool appointmentcheck; // 예약여부

      


        // 내담자 생성자.
        public ClientUser(string userGroup, string email, string username, string sex, string birth , string job, 
            string meal, string sleep, string exercise,
            bool appointmentcheck )
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

            this.appointmentcheck = appointmentcheck;
       
        }
    }

   
    // 내담자 임시유저 저장 버튼.
    public void TestBtn()
    {
        ClientUser clientUser = new ClientUser("내담자","asdf@gmail.com","김내담", "여","19950202", "학생","3끼", "7시간", "0회",
            false );

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
        public string userGroup, uid, email, pic, username, sex, intro, career1, career2, career3;

        // 전문 분야 : 가족, 나 자신, 대인관계, 연애, 직장, 진로/취직
        public bool family, myself, relationship, romance, work, career;

        public bool appointment;  // 예약여부


        public int careersCount;
        public string[] careers;


        // 상담사 생성자.
        public CounselorUser(string userGroup, string uid, string email, string pic, string username, string sex, string intro,
            bool family, bool myself, bool relationship, bool romance, bool work, bool career,
            string career1, string career2, string career3,
            bool appointment)
        {
            this.userGroup = userGroup;
            this.uid = uid;
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

            //this.appointment = appointment;

           // this.careersCount = careersCount;
           // this.careers = new careers[careersCount];
           
        }

    }



    // 상담사 레코드 하위에 위치한 예약 레코드(appointment)
    class CounselorAppo
    {
        // 내담자uid, 거절사유, 고민내용, 내담자 후기, 상담날짜, 상담시간, 신청인(내담자)이름
        public string clientUid, refuse, worry, feedback, appDay1, appDay2, appTime, client;

        // 수락상태, 0:무반응 1:수락 2:거절
        public int progress;

        public CounselorAppo(string clientUid, string refuse, string worry, string feedback,
            string appDay1, string appDay2, string appTime, string client, int progress)
        {
            this.clientUid = clientUid;
            this.refuse = refuse;
            this.worry = worry;
            this.feedback = feedback;
            this.appDay1 = appDay1;
            this.appDay2 = appDay2;
            this.appTime = appTime;
            this.client = client;
            this.progress = progress;

        }

    }


    // 상담사 레코드 하위에 위치한 가능예약 시간레코드(time)
    class CounselorTime
    {
        public string nine, ten, eleven, twelve, thirteen, fourteen
            , fifteen, sixteen, seventeen;

        public CounselorTime(string nine, string ten, string eleven, string twelve, string thirteen, string fourteen
            , string fifteen, string sixteen, string seventeen)
        {
            this.nine = nine;
            this.ten = ten;
            this.eleven = eleven;
            this.twelve = twelve;
            this.thirteen = thirteen;
            this.fourteen = fourteen;
            this.fifteen = fifteen;
            this.sixteen = sixteen;
            this.seventeen = seventeen;
 
        }

    }

    // 상담사 레코드 하위에 위치한 가능예약 요일레코드(day)
    class CounselorDay
    {
        public bool Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday;

        public CounselorDay(bool Monday, bool Tuesday, bool Wednesday
            , bool Thursday, bool Friday, bool Saturday, bool Sunday)
        {
            this.Monday = Monday;
            this.Tuesday = Tuesday;
            this.Wednesday = Wednesday;
            this.Thursday = Thursday;
            this.Friday = Friday;
            this.Saturday = Saturday;
            this.Sunday = Sunday;

        }

    }




    // 상담사 임시유저 저장 버튼.
    public void TestCBtn()
    {
        // 상담사 기본정보
        CounselorUser counselorUser = new CounselorUser("상담사", "-MzpTIrk43SkRf1yXmjd","asdf@gmail.com", "이미지 경로", "박상담", "여", 
            "책임감과 헌신적인 자세로 내담자의 마음을 치유하는 심리학 박사입니다.",
            false, false, true, false, false, false,
            "상담심리사 1급 (한국심리학회)", "정신건강임상심리사 1급 (보건복지부)", "전) 한양대병원 정신건강의학과 심리평가 및 상담", 
             false);

        // 상담사 예약정보
        CounselorAppo counselorAppo = new CounselorAppo("uid", "", "", "", "", "", "", "", 0);

        // 상담사 가능 시간
        CounselorTime counselorTime = new CounselorTime("9:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00"
           , "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00", "17:00-18:00");

        // 상담사 가능 요일
        CounselorDay counselorDay = new CounselorDay(true, true, true, true, true, true, true);


        // 데이터를 json형태로 반환.
        string json = JsonUtility.ToJson(counselorUser);
        string json2 = JsonUtility.ToJson(counselorAppo);
        string json3 = JsonUtility.ToJson(counselorTime);
        string json4 = JsonUtility.ToJson(counselorDay);

        // root의 자식 clientUser key 값을 추가.
        string key = reference.Child("CounselorUsers").Push().Key;

   


        // 해당 전문 분야 하위에 상담사 데이터 저장. (전문 분야로 쿼리하기 위해..?)
        if (counselorUser.family)
        {
            reference.Child("CounselorUsers").Child("가족").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("가족").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("가족").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("가족").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);


        }
        else if (counselorUser.myself)
        {
            reference.Child("CounselorUsers").Child("나 자신").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("나 자신").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("나 자신").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("나 자신").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);
        }
        else if (counselorUser.relationship)
        {
            reference.Child("CounselorUsers").Child("대인관계").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("대인관계").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("대인관계").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("대인관계").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);

        }
        else if (counselorUser.romance)
        {
            reference.Child("CounselorUsers").Child("연애").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("연애").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("연애").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("연애").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);
        }
        else if (counselorUser.work)
        {
            reference.Child("CounselorUsers").Child("직장").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("직장").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("직장").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("직장").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);
        }
        else
        {
            reference.Child("CounselorUsers").Child("진로/취업").Child(key).SetRawJsonValueAsync(json);

            reference.Child("CounselorUsers").Child("진로/취업").Child(key).Child("appointment").Push().Child(key).SetRawJsonValueAsync(json2);

            reference.Child("CounselorUsers").Child("진로/취업").Child(key).Child("counselorTime").SetRawJsonValueAsync(json3);

            reference.Child("CounselorUsers").Child("진로/취업").Child(key).Child("counselorDay").SetRawJsonValueAsync(json4);
        }


    }



}



// 예약 수락이 되면 -> 채널 이름을 파이어베이스에 저장(내담자 uid로?)
// 상담소에 입장할 때 파이어베이스에 저장한 채널이름으로 토큰 생성
// 예약 날짜 당일,시간이 아니면 상담입장은 비활성화 되어야함