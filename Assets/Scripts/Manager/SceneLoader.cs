/*
 * Class: SceneLoader
 * Date: 2020.7.16
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: 씬간의 이동을 다루는 클래스로 씬메니저의 역활을 한다.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        Title, Village, Stages, Credit, Tutorial
    }

    //씬 전환시 부드럽게 이동하는 연출에 사용된다.
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private GameObject fadeInPanel;
    //게임이 끝났는지 판별해 Village씬에서 어떤 대사를 사용할지 정한다.
    [SerializeField] private bool isGameFinished = false;

    private Scene curScene = Scene.Title; //현재 씬 (처음은 타이틀 화면)
    private bool isSceneLoading = false; //씬 전환시 클릭및 이동에 제한을 넣는데 사용.
    private bool isSetttingMenuOn = false; //설정청 상태 확인용
    private bool isGameOverMenuOn = false; //게임오버창 상태 확인용
    private bool isTutorialSceneFinished = false; //튜토리얼이 끝났는지 체크

    //Singleton
    public static SceneLoader instance { get; private set; }

    //Setter
    public void SetIsSceneLoading(bool _isSceneLoading) { isSceneLoading = _isSceneLoading; }
    public void SetIsGameFinsihed(bool _isGameFinished) { isGameFinished = _isGameFinished; }
    public void SetIsGameOverMenuOn(bool _isGameOverMenuOn) { isGameOverMenuOn = _isGameOverMenuOn; }
    public void SetIsTutorialSceneFinished(bool _isTutorialSceneFinished) { isTutorialSceneFinished = _isTutorialSceneFinished; }
    public void SetIsSettingMenuOn(bool _isSetttingMenuOn) { isSetttingMenuOn = _isSetttingMenuOn; }
    public void SetCurScene(Scene _curScene) { curScene = _curScene; }

    //Getter
    public bool GetIsSceneLoading() { return isSceneLoading; }
    public bool GetIsGameFinsihed() { return isGameFinished; }
    public bool GetIsGameOverMenuOn() { return isGameOverMenuOn; }
    public bool GetIsTutorialSceneFinished() { return isTutorialSceneFinished; }
    public bool GetIsSettingMenuOn() { return isSetttingMenuOn; }
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

        //타이틀씬에서 처음 생성시 브금 넣는 기능
        if (curScene == Scene.Title)
        {
            AudioManager.instance.BGMSetting(curScene.ToString());
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    //다음씬을 불러오는 함수
    public void LoadNextScene(string stage)
    {
        isSceneLoading = true;
        fadeOutPanel.SetActive(true);
        StartCoroutine(LoadSceneInDelay(stage));
    }

    //더 부드럽고 자연스러운 씬 전환 연출을 위함 코루틴 함수
    IEnumerator LoadSceneInDelay(string stage)
    {
        yield return new WaitForSeconds(0.6f);
        fadeOutPanel.SetActive(false);

        //코루틴 즉 Multi-Thread의 성질을 활용해 다른 씬이 로드된 후에도 뒤에 코드가 문제없이 호출 된다.
        SceneManager.LoadScene(stage); 

        //curSecne값 변경
        switch(stage)
        {
            case "TitleMenuScene":
                curScene = Scene.Title;
                break;
            case "stage0":
                curScene = Scene.Village;
                break;
            case "Stage1":
            case "stage2":
            case "stage3":
                curScene = Scene.Stages;
                break;
            case "TutorialScene":
                curScene = Scene.Tutorial;
                break;
            case "CreditPage":
                curScene = Scene.Credit;
                break;
        }

        //curSecne값에 맞는 브금 재생
        if (AudioManager.instance)
            AudioManager.instance.BGMSetting(stage);

        //Manager역활을 하는 스크립트를 포함한 게임 오브젝트는 다음 씬으로 이동시킨다.
        DontDestroyOnLoad(gameObject);
        if(AudioManager.instance)
        {
            DontDestroyOnLoad(AudioManager.instance.gameObject);
        }
        StartCoroutine(FadeIn());
    }

    //FadeOut으로 자연스런 씬 전환에 쓰인다
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

    //FadeIn으로 자연스런 씬 전환에 쓰인다
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

    //외부에서 부르기위한 용도
    public void PlayFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void PlayFadeIn()
    {
        StartCoroutine(FadeIn());
    }
}
