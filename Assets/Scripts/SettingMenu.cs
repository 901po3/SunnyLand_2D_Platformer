/*
 * Class: SettingMenu
 * Date: 2020.7.20
 * Last Modified : 2020.7.20
 * Author: Hyukin Kwon 
 * Description: Setting Menu
*/

using System.Collections;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenuBgObj;
    [SerializeField] private AudioClip tocuhSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SceneLoader.instance.GetSfxVolume();
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
        SceneLoader.instance.SetIsSettingMenuOn(false);
        Debug.Log("Back to Normal state");
    }
}
