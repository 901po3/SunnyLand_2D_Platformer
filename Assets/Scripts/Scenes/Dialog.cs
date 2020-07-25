/*
 * Class: Dialog
 * Date: 2020.7.18
 * Last Modified : 2020.7.23
 * Author: Hyukin Kwon 
 * Description: Queue를 사용해 대사를 관리한다.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject FriendIcon;
    [SerializeField] private GameObject PlayerIcon;
    [SerializeField] private Text dialogText;

    struct DialogStruct
    {
        public bool isFriendTalking;
        public string dialogText;

        //말하는 대상과 내용
        public DialogStruct(bool _isFriendTalking, string _dialogText)
        {
            isFriendTalking = _isFriendTalking;
            dialogText = _dialogText;
        }
    }

    private int idx = 0; //문자 잠초하는 인덱스
    private string str; //문장
    private bool isLoadingStr = false;
    private float curWordDelay = 0.0f; //문자 나오는 주기
    private bool isFriendTalking = false;
    private bool isSkipButtonPressed = false;

    private Queue<DialogStruct> dialog = new Queue<DialogStruct>();

    private void Start()
    {
        SetDialog(); //대사들을 불러온다
        LoadNextText(false);
        dialogText.text = "";
    }

    private void Update()
    {
        if (isSkipButtonPressed) return;

        if (Input.GetMouseButtonUp(0)) //클릭하면 대사를 넘김
        {
            if (!SceneLoader.instance.GetIsSettingMenuOn())
            {
                LoadNextText(true);
            }
        }
        ChangeIcon();

        LoadCurrentStr();
    }
    
    //다음 대사를 불러오는 함수
    private void LoadNextText(bool playSound)
    {
        if (isLoadingStr)
        {
            if(str != "") //초기화 
            {
                dialogText.text = str; 
                idx = 0;
                curWordDelay = 0f;
                isLoadingStr = false;
            }
            return;
        }

        if (dialog.Count <= 0) //큐에 남은 대사가 없으면 다음 씬으로 넘어간다.
        {
            GoToNextScene();
            return;
        }
        if(playSound)
            AudioManager.instance.PlayTouchSFX();

        //큐에서 문장과 말하는이 정보를 불러온다.
        DialogStruct dialogStruct = dialog.Dequeue();
        isFriendTalking = dialogStruct.isFriendTalking;
        dialogText.text = "";
        str = dialogStruct.dialogText;
        isLoadingStr = true;
    }

    //현재 참조하는 문장에서 한글자씩 불러온다.
    private void LoadCurrentStr()
    {
        if(isLoadingStr)
        {
            if(idx >= str.Length) //문장이 끝남
            {
                idx = 0;
                isLoadingStr = false;
            }
            else //주기마다 한글자씩 불러옴
            {
                if(curWordDelay < 0.1f)
                {
                    curWordDelay += Time.deltaTime;
                    if(curWordDelay >= 0.1f)
                    {
                        AudioManager.instance.PlayDialogSFX();
                        dialogText.text += str[idx]; 
                        curWordDelay = 0f;
                        idx++;
                    }
                }
            }
        }
    }

    //자동 넘김용 (현재 사용X)
    IEnumerator AutoLoadNextStr()
    {
        yield return new WaitForSeconds(1.5f);
        LoadNextText(false);
    }

    //말하는이에 따라 아이콘을 변경한다.
    private void ChangeIcon()
    {
        if(isFriendTalking)
        {
            FriendIcon.SetActive(true);
            PlayerIcon.SetActive(false);
            dialogText.alignment = TextAnchor.MiddleRight;
        }
        else
        {
            FriendIcon.SetActive(false);
            PlayerIcon.SetActive(true);
            dialogText.alignment = TextAnchor.MiddleLeft;
        }
    }

    //대사를 큐에 넣는다
    private void SetDialog()
    {
        if (!SceneLoader.instance.GetIsGameFinsihed())
        {
            dialog.Enqueue(new DialogStruct(false, "안녕..."));
            dialog.Enqueue(new DialogStruct(false, "아프다는 소문을 들었는데 괜찮아?"));
            dialog.Enqueue(new DialogStruct(true, "콜록 콜록"));
            dialog.Enqueue(new DialogStruct(false, "!!!"));
            dialog.Enqueue(new DialogStruct(true, "하얀토끼야 와줘서 고마워..."));
            dialog.Enqueue(new DialogStruct(true, "몸이 점점 안좋아지네..."));
            dialog.Enqueue(new DialogStruct(false, "..."));
            dialog.Enqueue(new DialogStruct(false, "내가 어떻게 도와줄까?"));
            dialog.Enqueue(new DialogStruct(true, "..."));
            dialog.Enqueue(new DialogStruct(true, "마법의 열매를 구할 수 있니?"));
            dialog.Enqueue(new DialogStruct(false, "!!!"));
            dialog.Enqueue(new DialogStruct(false, "당연하지."));
            dialog.Enqueue(new DialogStruct(false, "그 열매라면 다시 건강해 질거야."));
            dialog.Enqueue(new DialogStruct(true, "정말 고마워... 콜록 콜록"));
            dialog.Enqueue(new DialogStruct(false, "나만 믿어."));
            dialog.Enqueue(new DialogStruct(true, "응 고마워 하얀토끼야..."));
            dialog.Enqueue(new DialogStruct(false, "(지금 숲으로 출발해야겠어)"));
        }
        else
        {
            dialog.Enqueue(new DialogStruct(false, "(열매를 전해주고 며칠 후...)"));
            dialog.Enqueue(new DialogStruct(false, "건강을 되찾아서 다행이야."));
            dialog.Enqueue(new DialogStruct(true, "정말 고마워 하얀토끼야."));
            dialog.Enqueue(new DialogStruct(true, "오늘 저녁은 맛있는 당근을 준비했어."));
            dialog.Enqueue(new DialogStruct(true, "어서 가자."));
            dialog.Enqueue(new DialogStruct(false, "그래."));
        }
    }


    //함수가 불리면 대화를 생략하고 바로 다음 씬으로 이동
    public void SkipButton()
    {
        isSkipButtonPressed = true;
        GoToNextScene();
    }

    //게임 진행도에 따라 다음 씬을 로드
    private void GoToNextScene()
    {
        if (!SceneLoader.instance.GetIsGameFinsihed())
            SceneLoader.instance.LoadNextScene("Stage1");
        else
            SceneLoader.instance.LoadNextScene("CreditPage");
    }
}
