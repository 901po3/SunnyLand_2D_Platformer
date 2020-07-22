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
    [SerializeField] private GameObject JumpButton;

    private int curTutorialNum = 0;
    private int cnt = 0;
    private float timeMesurement = 0.0f;
    private bool callFunctionOnce = false;
    private bool isCurTotorialCompleted = false;
    private bool isLoadingNextTutorial = false;


    private void Start()
    {
        MoveToPosition();
        SetTutorialObjsToCurrentState();
        SceneLoader.instance.SetIsTutorialSceneFinished(false);
    }

    private void Update()
    {
        PlayTutorial();
    }

    private void PlayTutorial()
    {
        switch (curTutorialNum)
        {
            case 0:
                PlayFirstTutorial();
                break;
            case 1:
                PlayeSecondTutorial();
                break;
            case 2:
                PlayThirdTutorial();
                break;
            case 3:
                PlayFourthTutorial();
                break;
            case 4:
                PlayFifthTutorial();
                break;
        }
    }

    private void PlayFirstTutorial()
    {
        JumpButton.SetActive(false);
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
                StartCoroutine(MoveToNextTutorial(3));
            }
        }
    }

    private void PlayeSecondTutorial()
    {
        JumpButton.SetActive(true);
        if (!isCurTotorialCompleted)
        {
            if (PlayerController.instance.GetIsJumpButtonPressed())
            {
                if (!callFunctionOnce)
                {
                    callFunctionOnce = true;
                    StartCoroutine(MoveToNextTutorialInDelay(3));
                }
            }
        }
        else
        {
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial(3));
            }
        }
    }

    private void PlayThirdTutorial()
    {
        if (!isCurTotorialCompleted)
        {
            if (PlayerController.instance.GetEnemyBelow() != null)
            {
                cnt++;
                if (cnt >= 3)
                {
                    LoadNextTutorialObj();
                    cnt = 0;
                }
            }
        }
        else
        {
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial(3));
            }
        }
    }

    private void PlayFourthTutorial()
    {
        JumpButton.SetActive(true);
        if (!isCurTotorialCompleted)
        {
            if (!callFunctionOnce)
            {
                callFunctionOnce = true;
                LoadNextTutorialObj();
            }
        }
        else
        {
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial(12f));
            }
        }
    }

    private void PlayFifthTutorial()
    {
        if (!isCurTotorialCompleted)
        {
            PlayerController.instance.SetEnemyBelow(null);
            if(SceneLoader.instance.GetIsTutorialSceneFinished())
            {
                LoadNextTutorialObj();
            }
        }
        else
        {
            if (!isLoadingNextTutorial)
            {
                StartCoroutine(MoveToNextTutorial(4.5f));
            }
        }
    }

    //This function takes player to the next tutorial;
    IEnumerator MoveToNextTutorial(float time)
    {
        isLoadingNextTutorial = true;
        yield return new WaitForSeconds(time);
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
            curTutorialNum++;
            if (curTutorialNum >= uncompletedTutorialObjs.Length)
            {
                SceneLoader.instance.LoadNextScene("TitleMenuScene");
                return;
            }
            isCurTotorialCompleted = false;
            callFunctionOnce = false;
            cnt = 0;
            timeMesurement = 0;
            MoveToPosition();
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

    IEnumerator MoveToNextTutorialInDelay(float time)
    {
        yield return new WaitForSeconds(time);
        LoadNextTutorialObj();
    }

}
