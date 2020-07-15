/*
 * Class: CameraSize
 * Date: 2020.7.14
 * Last Modified : 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Adjusting Camera Size repect to screen size.
*/

using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] float size = 15; //size of orthographic camera
    void Start()
    {
        Camera.main.orthographicSize = (float)(size * Screen.height / Screen.width * 0.5);
    }
}
