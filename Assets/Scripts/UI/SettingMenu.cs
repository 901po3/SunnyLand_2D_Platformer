/*
 * Class: SettingMenu
 * Date: 2020.7.20
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: 버튼이아닌 실질적인 설정창과 타이틀화면으로 나가기 창을 관리한다.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuBgObj; //설정창 오브젝트
    [SerializeField] private GameObject homeMenuBgObj; //타이틀화면으로 나가기창 오브젝트
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;


    private void Awake()
    {
        UpdateSoundSliders();
    }

    #region 설정창 관련
    public void UpdateSoundSliders() //볼륨값에 맞게 슬라이이값 설정
    {
        if (AudioManager.instance)
        {
            bgmSlider.value = AudioManager.instance.GetOriginalBgmVolume();
            sfxSlider.value = AudioManager.instance.GetOriginalSfxVolume();
        }
    }

    public void ApplyButtonPressed() //볼륨 변화를 적용시키고 나가는 버튼
    {
        Debug.Log("Apply Button Pressed");
        SettingMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(ApplyButtonPressedInDelay());
    }

    IEnumerator ApplyButtonPressedInDelay() //자연스라운 연출을 위한 코루틴
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.UpdateOriginalVolume();
        AudioManager.instance.SaveVolumeToData(); //0.2초뒤에 볼륨 적용을 한다.
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }

    public void CancelButtonPressed() //볼륨 변화를 취소하고 나가는 버튼
    {
        Debug.Log("Cancel Button Pressed");
        SettingMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(CancelButtonPressedInDelay());
    }

    IEnumerator CancelButtonPressedInDelay()  //자연스라운 연출을 위한 코루틴
    {
        //취소했기때문에 원래 볼륨으로 되돌린다.
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.UndoVolume();
        bgmSlider.value = AudioManager.instance.GetOriginalBgmVolume(); 
        sfxSlider.value = AudioManager.instance.GetOriginalSfxVolume();
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }


    public void BGMSlider()
    {
        if(SceneLoader.instance)
        {
            AudioManager.instance.UpdateBGM(bgmSlider.value);
            Debug.Log("value changed");
        }
    }

    public void SFXSlider()
    {
        if (SceneLoader.instance)
        {
            AudioManager.instance.UpdateSFX(sfxSlider.value);
            Debug.Log("value changed");
        }
    }

    #endregion

    #region 타이틀화면으로 나가기 창 관련

    //나가기 버튼이 눌렸을때
    public void YesButtonPressed()
    {
        Debug.Log("Yes Button Pressed");
        homeMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(YesButtonPressedInDelay());
    }

    IEnumerator YesButtonPressedInDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SceneLoader.instance.SetIsSettingMenuOn(false);
        SceneLoader.instance.LoadNextScene("TitleMenuScene"); 
    }

    //취소 버튼이 눌렸을때 다시 게임을 진행
    public void NoButtonPressed()
    {
        Debug.Log("Yes Button Pressed");
        homeMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(NoButtonPressedInDelay());
    }

    IEnumerator NoButtonPressedInDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }
    #endregion
}
