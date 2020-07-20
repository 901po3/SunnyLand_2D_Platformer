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
    [SerializeField] private AudioClip tocuhSound;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private float originalBGMVolume;
    private float originalSFXVolume;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(SceneLoader.instance)
        {
            audioSource.volume = SceneLoader.instance.GetSfxVolume();
            originalBGMVolume = SceneLoader.instance.GetBgmVolume();
            originalSFXVolume = SceneLoader.instance.GetSfxVolume();
        }
    }

    public void CancelButtonPressed()
    {
        Debug.Log("Cancel Button Pressed");
        SettingMenuBgObj.SetActive(false);
        Time.timeScale = 1.0f;
        audioSource.PlayOneShot(tocuhSound);
        StartCoroutine(CancelButtonPressedInDelay());
    }

    IEnumerator CancelButtonPressedInDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SceneLoader.instance.UpdateBGM(originalBGMVolume);
        SceneLoader.instance.UpdateSFX(originalSFXVolume);
        bgmSlider.value = originalBGMVolume;
        sfxSlider.value = originalSFXVolume;
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }

    public void BGMSlider()
    {
        if(SceneLoader.instance)
        {
            SceneLoader.instance.UpdateBGM(bgmSlider.value);
            Debug.Log("value changed");
        }
    }

    public void SFXSlider()
    {
        if (SceneLoader.instance)
        {
            SceneLoader.instance.UpdateSFX(sfxSlider.value);
            Debug.Log("value changed");
        }
    }
}
