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
        LoadNextText();
        dialogText.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!SceneLoader.instance.GetIsSettingMenuOn())
            {
                LoadNextText();
            }
        }
        ChangeIcon();

        LoadCurrentStr();
    }
    private void LoadNextText()
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
        LoadNextText();
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
            dialog.Enqueue(new DialogStruct(false, "안녕 근처에 오는길에 들렸어..."));
            dialog.Enqueue(new DialogStruct(false, "잘지내지?"));
            dialog.Enqueue(new DialogStruct(true, "콜록 콜록"));
            dialog.Enqueue(new DialogStruct(false, "!!"));
            dialog.Enqueue(new DialogStruct(true, "미안 몸이 많이 아픈 것 같아..."));
            dialog.Enqueue(new DialogStruct(false, "..."));
            dialog.Enqueue(new DialogStruct(false, "내가 어떻게 도와줄수 있을까?"));
            dialog.Enqueue(new DialogStruct(true, "..."));
            dialog.Enqueue(new DialogStruct(true, "혹시 마법의 열매를 구해다 줄 수 있을까?"));
            dialog.Enqueue(new DialogStruct(false, "물론이지"));
            dialog.Enqueue(new DialogStruct(true, "정말 고마워...콜록 콜록"));
            dialog.Enqueue(new DialogStruct(false, "난 이만 숲으로 출발해볼게"));
            dialog.Enqueue(new DialogStruct(false, "몸조리 잘하고 있어"));
        }
        else
        {
            dialog.Enqueue(new DialogStruct(false, "열매 구해왔어"));
            dialog.Enqueue(new DialogStruct(true, "고마워"));
        }
    }
}
