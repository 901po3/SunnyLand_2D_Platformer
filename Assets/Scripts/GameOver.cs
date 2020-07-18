/*
 * Class: GameOver
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: GameOver script
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private string curSceneName;
    [SerializeField] private GameObject Panel;

    //Singleton
    public static GameOver instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }

    public void TurnOnGameOver()
    {
        StartCoroutine(TurnOn());
    }

    public void RetryButtonPressed()
    {
        Time.timeScale = 1;
        SceneLoader.instance.LoadNextScene(curSceneName);
    }

    public void ExitButtonPressed()
    {
        Time.timeScale = 1;
        SceneLoader.instance.LoadNextScene("TitleMenuScene");
    }

    IEnumerator TurnOn()
    {
        Panel.SetActive(false);
        SceneLoader.instance.PlayFadeOut();

        yield return new WaitForSeconds(0.6f);
        Panel.SetActive(true);
        Time.timeScale = 0;
    }
}
