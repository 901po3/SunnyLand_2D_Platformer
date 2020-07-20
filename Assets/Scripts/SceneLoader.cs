/*
 * Class: SceneLoader
 * Date: 2020.7.16
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: Managing Scene interactions.
 *              This will be created on TitleMenu
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        Title, Village, Stages, Credit
    }

    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private GameObject fadeInPanel;
    [SerializeField] private bool isGameFinished = false;

    private Scene curScene = Scene.Title;
    private bool isSceneLoading = false;
    private bool isSetttingMenuOn = false;

    //Singleton
    public static SceneLoader instance { get; private set; }

    //Setter getter
    public void SetIsSceneLoading(bool sceneLoading) { isSceneLoading = sceneLoading; }
    public void SetIsGameFinsihed(bool gameFinished) { isGameFinished = gameFinished; }
    public void SetCurScene(Scene cS) { curScene = cS; }
    public void SetIsSettingMenuOn(bool sm) { isSetttingMenuOn = sm; }
    public bool GetIsSceneLoading() { return isSceneLoading; }
    public bool GetIsGameFinsihed() { return isGameFinished; }
    public Scene GetCurScene() { return curScene; }
    public bool GetIsSettingMenuOn() {  return isSetttingMenuOn; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isSceneLoading = false;
        fadeOutPanel.SetActive(false);
        fadeInPanel.SetActive(false);

        if(curScene == Scene.Title)
        {
            AudioManager.instance.BGMSetting("TitleMenuScene");
        }
    }


    private void OnDestroy()
    {
        instance = null;
    }

    public void LoadNextScene(string stage)
    {
        isSceneLoading = true;
        fadeOutPanel.SetActive(true);
        StartCoroutine(LoadSceneInDelay(stage));
    }

    IEnumerator LoadSceneInDelay(string stage)
    {
        yield return new WaitForSeconds(0.6f);
        fadeOutPanel.SetActive(false);
        SceneManager.LoadScene(stage);
        if(AudioManager.instance)
            AudioManager.instance.BGMSetting(stage);

        DontDestroyOnLoad(gameObject);
        if(AudioManager.instance)
        {
            DontDestroyOnLoad(AudioManager.instance.gameObject);
        }
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        fadeOutPanel.SetActive(true);
        if(PlayerController.instance)
            PlayerController.instance.SetIsFronze(true);
        yield return new WaitForSeconds(0.6f);
        if (PlayerController.instance)
            PlayerController.instance.SetIsFronze(false);
        fadeOutPanel.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        fadeInPanel.SetActive(true);
        if (PlayerController.instance)
            PlayerController.instance.SetIsFronze(true);
        yield return new WaitForSeconds(0.6f);
        if (PlayerController.instance)
            PlayerController.instance.SetIsFronze(false);
        fadeInPanel.SetActive(false);
    }

    public void PlayFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void PlayFadeIn()
    {
        StartCoroutine(FadeIn());
    }
}
