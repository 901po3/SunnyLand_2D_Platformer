using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class: CameraSize
 * Date: 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Adjusting Camera Size repect to screen size.
*/

public class CameraSize : MonoBehaviour
{
    [SerializeField] float size = 15;
    void Start()
    {
        Camera.main.orthographicSize = (float)(size * Screen.height / Screen.width * 0.5);
    }
}
