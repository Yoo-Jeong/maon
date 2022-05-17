using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pwok : MonoBehaviour
{
   public  Image rectimage;
    public void getRect()
    {
        rectimage = GetComponent<Image>();

    }
    // 이미지 받아온 후 비밀번호와 비밀번호 확인의 문자열이 같으면 이미지 띄우기 or 이미지 색 초록색으로 바꾸기
}
