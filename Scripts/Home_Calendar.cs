using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Home_Calendar : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _todayText;
    public Text _monthNumText;

    public GameObject _item;
    public Image todayColor;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static Home_Calendar _calendarInstance;

    public Transform parent;
    GameObject item;

    public string year;

    private string today = DateTime.Now.Day.ToString();
    private string thisMonth = DateTime.Now.Month.ToString() + "월";

    public List<Button> dateButton = new List<Button>();  //달력의 날짜 버튼들을 담을 버튼 리스트
    public string seletedDateTime;                        //달력 날짜 버튼을 누르면 그 날짜를 담을 string타입 변수

    public static int dayOfWeek;                          //누른 버튼의 요일정보를 담을 int타입 변수

    void Start()
    {

        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            item = GameObject.Instantiate(_item, parent) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 96 + startPos.x, startPos.y - (i / 7) * 60, startPos.z);

                  
            _dateItems.Add(item);

            dateButton.Add(item.GetComponent<Button>()); //날짜 프리팹에 있는 버튼컴포넌트를 리스트에 추가


        }

        _dateTime = DateTime.Now;

        CreateCalendar();

    }

    //날짜 버튼을 누르면 선택한 날짜의 정보를 저장하는 함수.
    public void GetSeletedDateTime(string year, string month, string date)
    {
        //클릭한 버튼의 날짜가 나오길 원했는데 -1이 되는 문제가 있어 +1을 계산해준다.
        var number = int.Parse(date) + 1;  //string인 date를 int로 바꾸어 +1한 값을 number에 넣는다.
        date = number.ToString();          //number를 string타입으로 바꾸어 date에 넣는다.

        //년도, 월, 날짜가 정상적으로 전달됐는지 확인하기 위함.
        seletedDateTime = year + month + date;
        Debug.Log(seletedDateTime);

        //매개변수로 전달받은 year, month, date값을 가지고 DateTime으로 변환해서 DateTime형인 dateTime에 넣는다.
        DateTime dateTime = Convert.ToDateTime(year + "/" + month + "/" + date);

        dayOfWeek = GetDays(dateTime.DayOfWeek); //요일을 구하기 위한 함수 실행.
        Debug.Log(dayOfWeek); //확인을 위한 출력.

    }


    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);
        _monthNumText.text = _dateTime.Month.ToString() + "월";

        string month = _monthNumText.text;           //달력의 월을 string타입 변수 month에 저장

        // 맨 뒤에 글자 "월"을 자른다 (예: 4월을 4로 바꾼다.)
        month = month.Substring(0, month.Length - 1);

        // month가 1자리수면 앞에 "0"삽입(예:4를 04로 바꾼다.)
        if (month.Length < 2)
        {
            month = month.Insert(0, "0");
        }

     

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            //dateButton[i] 버튼을(달력 날짜버튼) 누르면 GetSeletedDateTime(string year, string month, string date) 함수 실행.
            dateButton[i].onClick.AddListener(() => { GetSeletedDateTime(_dateTime.Year.ToString(), month, label.text); });

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;


                }
            }

            // 오늘 날짜 표시
            if (today == label.text)
            {
                print("오늘날짜: " + today + "일 / 표시날짜: " + label.text);
                today = _dateItems[i].ToString();
                //print(today);
                _dateItems[i].GetComponent<Image>().color = new Color(255f / 255f, 195f / 255f, 43f / 255f);

            }

        }
        _todayText.text = DateTime.Now.ToString("yyyy.MM.dd.");
        //_todayText.text = _dateTime.ToString("yyyy.MM.dd.");
        

        //print("현재 달: " + thisMonth + "   달력 달: " + _monthNumText.text);
    }


    void ReCreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);


            string month = _dateTime.Month.ToString();           //달력의 월을 string타입 변수 month에 저장
            // month가 1자리수면 앞에 "0"삽입(예:4를 04로 바꾼다.)
            if (month.Length < 2)
            {
                month = month.Insert(0, "0");
            }
            //dateButton[i] 버튼을(달력 날짜버튼) 누르면 GetSeletedDateTime(string year, string month, string date) 함수 실행.
            dateButton[i].onClick.AddListener(() => { GetSeletedDateTime(_dateTime.Year.ToString(), month, label.text); });


            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);
                    _dateItems[i].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    //_dateItems[i].GetComponent<Image>().enabled = false;

                    label.text = (date + 1).ToString();
                    date++;

                }
            }

            /*  // 오늘 날짜 표시
              if (_monthNumText.text == thisMonth)
              {
                  print("현재 달: " + thisMonth + "   달력 달: " + _monthNumText.text);
                  _dateItems[i].GetComponent<Image>().enabled = true;

              }*/

        }


        //_todayText.text = _dateTime.ToString("yyyy.MM.dd.");
        _monthNumText.text = _dateTime.Month.ToString() + "월";
    }


    //요일을 구하는 함수.
    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        //print("현재 달: " + thisMonth + "   달력 달: " + _monthNumText.text);
        _dateTime = _dateTime.AddMonths(-1);
        ReCreateCalendar();

    }

    public void MonthNext()
    {
        //print("현재 달: " + thisMonth + "   달력 달: " + _monthNumText.text);
        _dateTime = _dateTime.AddMonths(1);
        ReCreateCalendar();

    }

    public void ShowCalendar(Text target)
    {
        _calendarPanel.SetActive(true);
        _target = target;
        //_calendarPanel.transform.position = new Vector3(965, 475, 0);//Input.mousePosition-new Vector3(0,120,0);
    }


    Text _target;
    //Item 클릭했을 경우 Text에 표시.
    public void OnDateItemClick(string day)
    {
        _target.text = _todayText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");
        _calendarPanel.SetActive(false);
    }


    
  
}


