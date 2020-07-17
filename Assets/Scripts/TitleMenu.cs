/*
 * Class: TitleMenu
 * Date: 2020.7.17
 * Last Modified : 2020.7.17
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
        Debug.Log("Play button clicked");
        SceneLoader.instance.LoadNextScene();
    }

    public void TutorialButtonOnClick()
    {
        Debug.Log("Tutorial button clicked");
    }

    public void OptionButtonOnClick()
    {
        Debug.Log("Option button clicked");
    }

    public void ExitButtonOnClick()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }
}
