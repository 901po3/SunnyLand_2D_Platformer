/*
 * Class: SettingMenu
 * Date: 2020.7.20
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: Setting Menu
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuBgObj;
    [SerializeField] private GameObject homeMenuBgObj;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] GameObject InputMenu;

    private void Awake()
    {
        if(AudioManager.instance)
        {
            bgmSlider.value = AudioManager.instance.GetOriginalBgmVolume();
            sfxSlider.value = AudioManager.instance.GetOriginalSfxVolume();
        }
    }

    public void ApplyButtonPressed()
    {
        Debug.Log("Apply Button Pressed");
        SettingMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(ApplyButtonPressedInDelay());
    }

    IEnumerator ApplyButtonPressedInDelay()
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.UpdateOriginalVolume();
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }

    public void CancelButtonPressed()
    {
        Debug.Log("Cancel Button Pressed");
        SettingMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayTouchSFX();
        StartCoroutine(CancelButtonPressedInDelay());
    }

    IEnumerator CancelButtonPressedInDelay()
    {
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


    //home menu buttons
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
}
