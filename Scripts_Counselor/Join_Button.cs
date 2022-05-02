using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Join_Button : MonoBehaviour
{
   public void SceneChange()
    {
        SceneManager.LoadScene("Join_Scene");  
    }
}
