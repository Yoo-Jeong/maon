using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 회전
public class PlayerCamCtrl : MonoBehaviour
{

    public Transform follow;
    Vector2 m_Input;
    float m_Speed = 0.5f;


    // Update is called once per frame
    void Update()
    {
        Rotate();
    }


    void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            m_Input.x = Input.GetAxis("Mouse X");

            if (m_Input.magnitude != 0)
            {
                Quaternion q = follow.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x + m_Input.y * m_Speed,
                    q.eulerAngles.y + m_Input.x * m_Speed,
                    q.eulerAngles.z);

                follow.rotation = q;

            }
        }
    }


}
