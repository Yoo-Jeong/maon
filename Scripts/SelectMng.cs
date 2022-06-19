using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


//회원가입 시 계정유형(상담사,내담자) 선택화면 관련
public class SelectMng : MonoBehaviour
{

    public Toggle counselorToggle, clientToggle;
    public bool isClient;

    public Button nextBtn, loginBtn;

    // Start is called before the first frame update
    void Start()
    {
        isClient = true;
        SetFunction_UI();

        loginBtn.onClick.AddListener(Auth_Manager.Instance.OnAuthCanvas);
    }

    public void ClickNextBtn()
    {
        if(isClient == true)
        {
            SceneManager.LoadScene("Client_Join_Scene");
        }
        else
        {
            SceneManager.LoadScene("Counselor_Join_Scene");
        }
    }


    private void ResetFunction_UI()
    {
        counselorToggle.onValueChanged.RemoveAllListeners();
        clientToggle.onValueChanged.RemoveAllListeners();

    }

    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();

        counselorToggle.onValueChanged.AddListener(Function_counselorToggle);
        clientToggle.onValueChanged.AddListener(Function_clientToggleToggle);
    
    }


    // 상담사 토글On
    public void Function_counselorToggle(bool _bool)
    {
        if (_bool == true)
        {
            isClient = false;
            nextBtn.interactable = false;

        }
        else
        {

            isClient = true;
            nextBtn.interactable = true;
        }


    } // Function_counselorToggle(bool _bool).


    // 내담자 토글On
    public void Function_clientToggleToggle(bool _bool)
    {
        if (_bool == true)
        {
            isClient = true;
            nextBtn.interactable = true;

        }
        else
        {

            isClient = false;
            nextBtn.interactable = false;
        }


    } // Function_counselorToggle(bool _bool).


}
