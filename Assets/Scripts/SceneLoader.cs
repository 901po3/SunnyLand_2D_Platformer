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
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private GameObject fadeInPanel;

    private bool isSceneLoading = false;

    //Singleton
    public static SceneLoader instance { get; private set; }

    //Setter getter
    public void SetIsSceneLoading(bool sceneLoading) { isSceneLoading = sceneLoading; }

    public bool GetIsSceneLoading() { return isSceneLoading; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isSceneLoading = false;
        fadeOutPanel.SetActive(false);
        fadeInPanel.SetActive(false);
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


}
