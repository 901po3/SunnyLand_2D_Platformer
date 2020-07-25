/*
 * Class: SettingButton
 * Date: 2020.7.20
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: 설정과 나가기(타이틀화면으로) 팝업창으로 가기 "버튼"을 관리
*/

using System.Collections;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuObj; //설정창 오브젝트
    [SerializeField] private GameObject homeMenuObj; //타이틀화면으로 나가기창 오브젝트
        
    private void Awake()
    {
        SettingMenuObj.SetActive(false);
        homeMenuObj.SetActive(false);
        if(SceneLoader.instance)
            SceneLoader.instance.SetIsGameOverMenuOn(false);
    }
    #region 설정창 열기 버튼 관련
    //설정창을 연다
    public void OpenSettingMenu()
    {
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.SetIsSettingMenuOn(true);
        StartCoroutine(OpenSettingMenuInDelay());
    }

    //설정창이 자연스럽게 나오게 한다.
    IEnumerator OpenSettingMenuInDelay()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.UpdateOriginalVolume();
        SettingMenuObj.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
    }
    #endregion

    #region 타이틀로 나가기 버튼 관련

    //타이틀화면으로 나가기창이 불렸을때 호출
    public void OpenHomeMenu()
    {
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.SetIsSettingMenuOn(true);
        StartCoroutine(OpenHomeMenuInDelay());
    }

    //타이틀화면으로 나가기창이 자연스럽게 나온다.
    IEnumerator OpenHomeMenuInDelay()
    {
        yield return new WaitForSeconds(0.1f);
        homeMenuObj.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
    }
    #endregion
}
