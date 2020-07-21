/*
 * Class: Tutorial
 * Date: 2020.7.21
 * Last Modified : 2020.7.21
 * Author: Hyukin Kwon 
 * Description:  Tutorial Script (Handles all tutorial process)
*/

using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] uncompletedTutorialObjs;
    [SerializeField] private GameObject[] completedTutorialObjs;
    [SerializeField] private Transform[] positions;

    private int curTutorialNum = 0;
    private int jumpCnt = 0;
    private float timeMesurement = 0.0f;
    private bool isCurTotorialCompleted = false;
    private bool isLoadingNextTutorial = false;


    private void Start()
    {
        MoveToPosition();
        SetTutorialObjsToCurrentState();
    }

    private void Update()
    {
        PlayTutorial();
    }

    private void PlayTutorial()
    {
        switch(curTutorialNum)
        {
            case 0:
                PlayFirstTutorial();
                break;
            case 1:
                PlayeSecondTutorial();
                break;
        }
    }

    private void PlayFirstTutorial()
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
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial());
            }
        }
    }

    private void PlayeSecondTutorial()
    {
        if (!isCurTotorialCompleted)
        {          
            if (!PlayerController.instance.GetWasJumpButtonPressed() && PlayerController.instance.GetIsJumpButtonPressed())
            {
                jumpCnt++;
                if (jumpCnt >= 3)
                {
                    LoadNextTutorialObj();
                    jumpCnt = 0;
                }
            }
        }
        else
        {
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial());
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
            MoveToPosition();
            if (curTutorialNum >= uncompletedTutorialObjs.Length)
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

    //Move player to next tutorial zone
    private void MoveToPosition()
    {
        PlayerController.instance.transform.position = positions[curTutorialNum].position; 
    }
}
