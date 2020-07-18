/*
 * Class: SceneLoader
 * Date: 2020.7.16
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: Managing Scene interactions.
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

    [SerializeField] private AudioClip TownBGM;
    [SerializeField] private AudioClip ForestBGM;
    [SerializeField] private AudioClip CreditBGM;
    [SerializeField] private AudioClip TitleBGM;
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private GameObject fadeInPanel;
    [SerializeField] private bool isGameFinished = false;
    [SerializeField] private float volume = 1f;

    private Scene curScene = Scene.Title;
    private bool isSceneLoading = false;
    private AudioSource audioSource;

    //Singleton
    public static SceneLoader instance { get; private set; }

    //Setter getter
    public void SetIsSceneLoading(bool sceneLoading) { isSceneLoading = sceneLoading; }
    public void SetIsGameFinsihed(bool gameFinished) { isGameFinished = gameFinished; }
    public void SetCurScene(Scene cS) { curScene = cS; }
    public bool GetIsSceneLoading() { return isSceneLoading; }
    public bool GetIsGameFinsihed() { return isGameFinished; }
    public Scene GetCurScene() { return curScene; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isSceneLoading = false;
        fadeOutPanel.SetActive(false);
        fadeInPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        BGMSetting("TitleMenuScene");
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
        BGMSetting(stage);

        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        fadeOutPanel.SetActive(true);
        PlayerController.instance.SetIsFronze(true);
        yield return new WaitForSeconds(0.6f);
        PlayerController.instance.SetIsFronze(false);
        fadeOutPanel.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        fadeInPanel.SetActive(true);
        PlayerController.instance.SetIsFronze(true);
        yield return new WaitForSeconds(0.6f);
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

    private void BGMSetting(string stage)
    {
        audioSource.pitch = 1.0f;
        audioSource.volume = volume;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        if (stage == "stage0")
        {
            curScene = Scene.Village;
        }
        else if (stage == "Stage1")
        {
            curScene = Scene.Stages;
        }
        else if (stage == "CreditPage")
        {
            curScene = Scene.Credit;
        }
        else if (stage == "TitleMenuScene")
        {
            curScene = Scene.Title;
        }
        SelectBGM();
        if(stage != "stage2" && stage != "stage3")
            audioSource.Play();
    }

    private void SelectBGM()
    {
        switch (curScene)
        {
            case Scene.Title:
                audioSource.clip = TitleBGM;
                break;
            case Scene.Village:
                audioSource.clip = TownBGM;
                break;
            case Scene.Stages:
                audioSource.clip = ForestBGM;
                break;
            case Scene.Credit:
                audioSource.clip = CreditBGM;
                break;
        }
    }

}
