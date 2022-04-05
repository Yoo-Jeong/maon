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


    GameObject item;
    public string year;

    private string today = DateTime.Now.Day.ToString();
    private string thisMonth = DateTime.Now.Month.ToString() + "월";

    void Start()
    {

        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 96 + startPos.x, startPos.y - (i / 7) * 60, startPos.z);

            

            _dateItems.Add(item);


        }

        _dateTime = DateTime.Now;

        CreateCalendar();

    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

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
        _monthNumText.text = _dateTime.Month.ToString() + "월";

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


