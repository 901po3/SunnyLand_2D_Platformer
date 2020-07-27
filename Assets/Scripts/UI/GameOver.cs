/*
 * Class: GameOver
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: 게임을 패배했을때 상황을 관리한다.
*/

using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private string curSceneName; //현재 진행중인 씬
    [SerializeField] private GameObject Panel; //게임오버 팝업 창

    //Singleton
    public static GameOver instance { get; private set; }

    private void Awake()
    {
        instance = this;
        Panel.SetActive(false);
    }
    private void OnDestroy()
    {
        instance = null;
    }

    //외부에서 플레이어가 죽었을시 불리는 함수
    public void TurnOnGameOver()
    {
        StartCoroutine(TurnOn());
    }

    //재시도 버튼
    public void RetryButtonPressed()
    {
        Time.timeScale = 1;
        SceneLoader.instance.SetIsGameOverMenuOn(false);
        SceneLoader.instance.LoadNextScene(curSceneName); //현재씬을 다시 로드한다.
    }

    //나가기 버튼
    public void ExitButtonPressed()
    {
        Time.timeScale = 1;
        Panel.SetActive(false);
        SceneLoader.instance.SetIsGameOverMenuOn(false);
        SceneLoader.instance.LoadNextScene("TitleMenuScene"); //타이틀 화면으로 나간다.
    }

    //게임오버 팝업창을 자연스럽게 나오게 한다.
    IEnumerator TurnOn()
    {
        SceneLoader.instance.PlayFadeOut();

        yield return new WaitForSeconds(0.6f);
        Panel.SetActive(true);
        SceneLoader.instance.SetIsGameOverMenuOn(true);
        Time.timeScale = 0;
    }
}
