/*
 * Class: Dialog
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: Handles dialog system
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private bool pressed = false;
    private int dialogNum = 3;
    private int curDialogNum;

    private void Start()
    {
        
    }

    private void Update()
    {
        TouchScreen();
    }


    private void TouchScreen()
    {

        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Clicked :" + curDialogNum);
            if(curDialogNum < dialogNum)
            {
                curDialogNum++;
                if(curDialogNum >= dialogNum)
                {
                    Debug.Log("Game Start");
                    SceneLoader.instance.LoadNextScene("Stage1");
                }
            }
        }
    }
}
