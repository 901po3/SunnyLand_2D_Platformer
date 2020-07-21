/*
 * Class: Dialog
 * Date: 2020.7.18
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: Handles dialog system
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
        public DialogStruct(bool b, string s)
        {
            isFriendTalking = b;
            dialogText = s;
        }
    }

    private int idx = 0;
    private string str;
    private bool isLoadingStr = false;
    private float curWordDelay = 0.0f; //delay between words
    private int curDialogNum;
    private bool isFriendTalking = false;

    private Queue<DialogStruct> dialog = new Queue<DialogStruct>();

    private void Start()
    {
        SetDialog();
        LoadNextText(false);
        dialogText.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!SceneLoader.instance.GetIsSettingMenuOn())
            {
                LoadNextText(true);
            }
        }
        ChangeIcon();

        LoadCurrentStr();
    }
    private void LoadNextText(bool playSound)
    {
        if (isLoadingStr)
        {
            if(str != "")
            {
                dialogText.text = str;
                idx = 0;
                curWordDelay = 0f;
                isLoadingStr = false;
            }
            return;
        }

        if (dialog.Count <= 0)
        {
            Debug.Log("Game Start");
            if (!SceneLoader.instance.GetIsGameFinsihed())
                SceneLoader.instance.LoadNextScene("Stage1");
            else
                SceneLoader.instance.LoadNextScene("CreditPage");
            return;
        }
        Debug.Log("Mouse Clicked :" + curDialogNum);
        if(playSound)
            AudioManager.instance.PlayTouchSFX();

        DialogStruct dialogStruct = dialog.Dequeue();
        isFriendTalking = dialogStruct.isFriendTalking;
        dialogText.text = "";
        str = dialogStruct.dialogText;
        isLoadingStr = true;
    }

    private void LoadCurrentStr()
    {
        if(isLoadingStr)
        {
            if(idx >= str.Length)
            {
                idx = 0;
                isLoadingStr = false;
                //StartCoroutine(AutoLoadNextStr());
            }
            else
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

    IEnumerator AutoLoadNextStr()
    {
        yield return new WaitForSeconds(1.5f);
        LoadNextText(false);
    }

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
            dialog.Enqueue(new DialogStruct(false, "그 열매라면 너도 다시 건강해 질거야."));
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
}
