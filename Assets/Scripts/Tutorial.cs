/*
 * Class: Tutorial
 * Date: 2020.7.21
 * Last Modified : 2020.7.21
 * Author: Hyukin Kwon 
 * Description:  Tutorial Script (Handles all tutorial process)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] uncompletedTutorialObjs;
    [SerializeField] private GameObject[] completedTutorialObjs;

    private int curTutorialNum = 0;
    private float timeMesurement = 0.0f;
    private bool isCurTotorialCompleted = false;
    private bool isLoadingNextTutorial = false;


    private void Start()
    {
        SetTutorialObjsToCurrentState();
    }

    private void Update()
    {
        PlayTutorial();
    }

    private void PlayTutorial()
    {
        PlayFirstTutorial();
    }

    private void PlayFirstTutorial()
    {
        if (curTutorialNum == 0)
        {
            if (!isCurTotorialCompleted)
            {
                if (PlayerController.instance.GetIsRightButtonPressed() || PlayerController.instance.GetIsLeftButtonPressed())
                {
                    timeMesurement += Time.deltaTime;
                    if (timeMesurement > 2.5f)
                    {
                        LoadNextTutorialObj();
                        timeMesurement = 0.0f;
                    }
                }
            }
            else
            {
                if(!isLoadingNextTutorial)
                {
                    StartCoroutine(MoveToNextTutorial());
                }
            }
        }
    }

    //This function takes player to the next tutorial;
    IEnumerator MoveToNextTutorial()
    {
        isLoadingNextTutorial = true;
        yield return new WaitForSeconds(3f);
        isLoadingNextTutorial = false;
        LoadNextTutorialObj();
    }


    //Load Next Tutorial GameObject(Panel)
    private void LoadNextTutorialObj()
    {
        if(!isCurTotorialCompleted)
        {
            isCurTotorialCompleted = true;
        }
        else
        {
            isCurTotorialCompleted = false;
            curTutorialNum++;
            if(curTutorialNum >= uncompletedTutorialObjs.Length)
            {
                //All tutorials are finished
            }
        }
        SetTutorialObjsToCurrentState();
    }

    private void SetTutorialObjsToCurrentState()
    {
        for (int i = 0; i < uncompletedTutorialObjs.Length; i++)
        {
            if(!isCurTotorialCompleted)
            {
                if (i == curTutorialNum)
                {
                    uncompletedTutorialObjs[i].SetActive(true);
                }
                else
                {
                    uncompletedTutorialObjs[i].SetActive(false);
                }
                completedTutorialObjs[i].SetActive(false);
            }
            else
            {
                if (i == curTutorialNum)
                {
                    completedTutorialObjs[i].SetActive(true);
                }
                else
                {
                    completedTutorialObjs[i].SetActive(false);
                }
                uncompletedTutorialObjs[i].SetActive(false);
            }
        }
    }

}
