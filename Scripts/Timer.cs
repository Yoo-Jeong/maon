using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    bool CounselActive;
    public Text[] ClockText; // 텍스트
    float time; // 시간계산


    // 상담시간 측정
    // 상담 시작버튼을 누르면 측정 시작 , 상담 종료 버튼 누르면 멈춤
    // 텍스트는 상담종료 버튼을 눌렀을때 보이도록

    // Start is called before the first frame update
    void Start()
    {
        CounselActive = true; // 상담씬에 접속하면 상담 활성화
    }
    public void ClickEnd()
    {
        CounselActive = false;  // 상담 종료 버튼을 누르면 타이머 멈춤
    }
    // Update is called once per frame
    void Update()
    {
        if (CounselActive == true)
        {
            time += Time.deltaTime;
            ClockText[0].text = ((int)time / 60 % 60).ToString();
            ClockText[1].text = ((int)time % 60).ToString();
        }
    }
}
