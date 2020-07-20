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
    [SerializeField] private GameObject SceneManagerPrefab;
    [SerializeField] private AudioClip tocuhSound;

    private AudioSource audioSource;

    private void Start()
    {
        if(SceneLoader.instance == null)
        {
            Instantiate(SceneManagerPrefab);
        }
        SceneLoader.instance.SetIsSceneLoading(false);
        SceneLoader.instance.SetIsGameFinsihed(false);
        SettingMenu.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SceneLoader.instance.GetSfxVolume();
    }
    public void PlayerButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        audioSource.PlayOneShot(tocuhSound);
        Debug.Log("Play button clicked");
        SceneLoader.instance.LoadNextScene("stage0");
    }

    public void TutorialButtonOnClick()
    {
        audioSource.PlayOneShot(tocuhSound);
        Debug.Log("Tutorial button clicked");
    }

    public void OptionButtonOnClick()
    {
        audioSource.PlayOneShot(tocuhSound);
        Debug.Log("Option button clicked");
        StartCoroutine(OpenSettingMenu());
    }

    public void ExitButtonOnClick()
    {
        audioSource.PlayOneShot(tocuhSound);
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    IEnumerator OpenSettingMenu()
    {
        yield return new WaitForSeconds(0.25f);
        SettingMenu.SetActive(true);
    }
}
