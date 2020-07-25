/*
 * Class: Tutorial
 * Date: 2020.7.21
 * Last Modified : 2020.7.21
 * Author: Hyukin Kwon 
 * Description:  튜토리얼(학습)씬을 관리한다.
*/

using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] uncompletedTutorialObjs; //각 튜토리얼에서 완료되지 않은 상태에 나오는 HUD를 갖고있다.
    [SerializeField] private GameObject[] completedTutorialObjs; //각 튜토리얼에서 완료된 상태에 나오는 HUD를 갖고있다.
    [SerializeField] private Transform[] positions; //각 튜토리얼의 시작 위치를 담음.
    [SerializeField] private GameObject JumpButton;

    private int curTutorialNum = 0; //현재 진행중인 튜토리얼
    private bool isCurTotorialCompleted = false; 
    private bool isLoadingNextTutorial = false;

    //튜토일얼 진행에 필요한 변수들
    private int cnt = 0;
    private float timeMesurement = 0.0f;
    private bool callFunctionOnce = false;


    private void Start()
    {
        MoveToPosition();
        SetTutorialObjsToCurrentState(); //현재 진행중인 튜토리얼에 맞게 HUD를 불러온다.
        SceneLoader.instance.SetIsTutorialSceneFinished(false);
    }

    private void Update()
    {
        PlayTutorial();
    }

    //튜토리얼을 진행한다.
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

    //첫번째 튜토리얼을 진행하는 함수
    private void PlayFirstTutorial()
    {
        JumpButton.SetActive(false);
        if (!isCurTotorialCompleted)
        {
            //좌우로 움직이면 시간을 계산해서 충분히 움직였으면 통과
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

    //두번째 튜토리얼을 진행하는 함수
    private void PlayeSecondTutorial()
    {
        JumpButton.SetActive(true);
        if (!isCurTotorialCompleted)
        {
            //점프를 했으면 잠시후 다음 씬으로 넘어감
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

    //세번째 튜토리얼을 진행하는 함수
    private void PlayThirdTutorial()
    {
        //현재 튜토리얼에 있는 적을 모두 죽이면 통과
        if (!isCurTotorialCompleted)
        {
            if (timeMesurement != 0)
            {
                timeMesurement -= Time.deltaTime;
                if (timeMesurement < 0)
                {
                    timeMesurement = 0;
                }
            }
            else if (PlayerController.instance.GetEnemyBelow() != null && timeMesurement == 0)
            {
                cnt++;
                timeMesurement = 0.25f;
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

    //네번째 튜토리얼을 진행하는 함수
    private void PlayFourthTutorial()
    {
        //조건없이 시간이 지나면 다음 튜토리얼로 이동
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

    //다섯번째 튜토리얼을 진행하는 함수
    private void PlayFifthTutorial()
    {
        //마지막 튜토리얼로 도착지에 도달하면 통과
        if (!isCurTotorialCompleted)
        {
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

    //다음 튜토리얼을 진행한다.
    IEnumerator MoveToNextTutorial(float time)
    {
        isLoadingNextTutorial = true;
        yield return new WaitForSeconds(time);
        isLoadingNextTutorial = false;
        LoadNextTutorialObj(); //다음 튜토리얼을 진행
    }


    //다음으로 진행
    private void LoadNextTutorialObj()
    {
        if(!isCurTotorialCompleted) //방금 조건을 충족시 충족으로 상태를 바꿈. (충족 했을시 나오는 HUD로 바뀜)
        {
            isCurTotorialCompleted = true;
        }
        else //이미 충족했을시 다음 튜토리얼로 이동
        {
            curTutorialNum++;
            if (curTutorialNum >= uncompletedTutorialObjs.Length) //마직막 튜토리얼을 깼을시 타이틀 화면으로 이동
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

    //현재 진행중인 튜토리얼의 상태에 맞게 HUD를 불러온다.
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

    //진행중인 튜토리얼 위치로 이동
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
