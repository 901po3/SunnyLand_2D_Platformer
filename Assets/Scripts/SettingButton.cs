/*
 * Class: SettingButton
 * Date: 2020.7.20
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: Open Setting Menu
*/

using System.Collections;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuObj;
    [SerializeField] private GameObject homeMenuObj;
        
    private void Awake()
    {
        SettingMenuObj.SetActive(false);
        homeMenuObj.SetActive(false);
        if(SceneLoader.instance)
            SceneLoader.instance.SetIsGameOverMenuOn(false);
    }  

    public void OpenSettingMenu()
    {
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.SetIsSettingMenuOn(true);
        StartCoroutine(OpenSettingMenuInDelay());
    }

    IEnumerator OpenSettingMenuInDelay()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.UpdateOriginalVolume();
        SettingMenuObj.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
    }

    public void OpenHomeMenu()
    {
        AudioManager.instance.PlayTouchSFX();
        SceneLoader.instance.SetIsSettingMenuOn(true);
        StartCoroutine(OpenHomeMenuInDelay());
    }

    IEnumerator OpenHomeMenuInDelay()
    {
        yield return new WaitForSeconds(0.1f);
        homeMenuObj.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
    }
}
