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
    [SerializeField] private GameObject FriendIcon;
    [SerializeField] private GameObject PlayerIcon;
    [SerializeField] private AudioClip tocuhSound;

    private bool pressed = false;
    private int dialogNum = 3;
    private int curDialogNum;
    private bool isFriendTalking = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SceneLoader.instance.GetSfxVolume();
    }

    private void Update()
    {
        if(!SceneLoader.instance.GetIsSettingMenuOn())
        {
            if (!SceneLoader.instance.GetIsGameFinsihed())
            {
                Prolouge();
            }
            else
            {
                Epilogue();
            }
        }
        ChangeIcon();
    }


    private void Prolouge()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Clicked :" + curDialogNum);
            audioSource.PlayOneShot(tocuhSound);
            if (curDialogNum < dialogNum)
            {
                curDialogNum++;
                isFriendTalking = !isFriendTalking;
                if (curDialogNum >= dialogNum)
                {
                    Debug.Log("Game Start");
                    SceneLoader.instance.LoadNextScene("Stage1");
                }
            }
        }
    }

    private void Epilogue()
    {
        if (Input.GetMouseButtonUp(0))
        {
            audioSource.PlayOneShot(tocuhSound);
            Debug.Log("Mouse Clicked :" + curDialogNum);
            if (curDialogNum < dialogNum)
            {
                curDialogNum++;
                isFriendTalking = !isFriendTalking;
                if (curDialogNum >= dialogNum)
                {
                    Debug.Log("Game Start");
                    SceneLoader.instance.LoadNextScene("CreditPage");
                }
            }
        }
    }

    private void ChangeIcon()
    {
        if(isFriendTalking)
        {
            FriendIcon.SetActive(true);
            PlayerIcon.SetActive(false);
        }
        else
        {
            FriendIcon.SetActive(false);
            PlayerIcon.SetActive(true);
        }
    }
}
