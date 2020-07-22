/*
 * Class: TitleMenu
 * Date: 2020.7.17
 * Last Modified : 2020.7.19
 * Author: Hyukin Kwon 
 * Description:  TitleMenu Class (Play, Tutorial, Option, Exit)button
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenu;
    [SerializeField] private GameObject AudioManagerPrefab;
    [SerializeField] private GameObject SceneManagerPrefab;

    private void Start()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(AudioManagerPrefab);
        }
        if (SceneLoader.instance == null)
        {
            Instantiate(SceneManagerPrefab);
        }
        SceneLoader.instance.SetIsSceneLoading(false);
        SceneLoader.instance.SetIsGameFinsihed(false);
        SettingMenu.SetActive(false);
    }
    public void PlayerButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Play button clicked");
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.LoadNextScene("stage0");
    }

    public void TutorialButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Tutorial button clicked");
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.LoadNextScene("TutorialScene");
    }

    public void OptionButtonOnClick()
    {
        AudioManager.instance.PlayTouchSFX();
        Debug.Log("Option button clicked");
        StartCoroutine(OpenSettingMenu());
    }

    public void ExitButtonOnClick()
    {
        AudioManager.instance.PlayTouchSFX();
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    IEnumerator OpenSettingMenu()
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.UpdateOriginalVolume();
        SettingMenu.SetActive(true);
    }
}
