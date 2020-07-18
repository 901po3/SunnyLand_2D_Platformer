/*
 * Class: TitleMenu
 * Date: 2020.7.17
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description:  TitleMenu Class (Play, Tutorial, Option, Exit)button
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    //[SerializeField] private Button playButton;
    //[SerializeField] private Button tutorialButton;
    //[SerializeField] private Button optionButton;
    //[SerializeField] private Button exitButton;

    public void PlayerButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Play button clicked");
        SceneLoader.instance.LoadNextScene("Stage1");
    }

    public void TutorialButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Tutorial button clicked");
    }

    public void OptionButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Option button clicked");
    }

    public void ExitButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Exit button clicked");
        Application.Quit();
    }
}
