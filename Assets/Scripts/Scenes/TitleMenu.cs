/*
 * Class: TitleMenu
 * Date: 2020.7.17
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description:  타이틀 메뉴에서 버튼 관련 이벤트를 관리한다.
*/

using System.Collections;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenu; //설정창
    [SerializeField] private GameObject AudioManagerPrefab; 
    [SerializeField] private GameObject SceneManagerPrefab;

    private void Awake()
    {
        //게임 접속후 처음으로 타이틀 화면에 들어오면 메니저 오브젝트들을 생성한다.
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

    //시작 버튼 이벤트
    public void PlayerButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Play button clicked");
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.LoadNextScene("stage0");
    }

    //튜토리얼(학습) 버튼 이벤트
    public void TutorialButtonOnClick()
    {
        if (SceneLoader.instance.GetIsSceneLoading()) return;
        Debug.Log("Tutorial button clicked");
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.LoadNextScene("TutorialScene");
    }

    //설정창 버튼 이벤트
    public void OptionButtonOnClick()
    {
        AudioManager.instance.PlayTouchSFX();
        Debug.Log("Option button clicked");
        StartCoroutine(OpenSettingMenu()); //설정창 전환을 자연스럽게 하기위해 코루틴 사용
    }

    //나가기 버튼 이벤트 (현재 사용X)
    public void ExitButtonOnClick()
    {
        AudioManager.instance.PlayTouchSFX();
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    //설정창 전환을 자연스럽게 하기위해 코루틴 사용
    IEnumerator OpenSettingMenu()
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.UpdateOriginalVolume();
        SettingMenu.SetActive(true);
        //BGM SFX 슬라이더를 현재 볼륨값에 맞게 설정한다.
        SettingMenu.transform.parent.GetComponent<SettingMenu>().UpdateSoundSliders(); 
    }
}
