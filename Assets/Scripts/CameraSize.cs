/*
 * Class: CameraSize
 * Date: 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Adjusting Camera Size repect to screen size.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] float m_size = 15;
    void Start()
    {
        Camera.main.orthographicSize = (float)(m_size * Screen.height / Screen.width * 0.5);
    }
}
