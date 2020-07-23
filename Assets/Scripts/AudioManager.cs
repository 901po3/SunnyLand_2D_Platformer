/*
 * Class: AudioManager
 * Date: 2020.7.20
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: Handles BGM and SFX
 *             This will be created on TitleMenu
*/

using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject bgmObj;
    [SerializeField] private GameObject sfxObj;
    //BGM
    [SerializeField] private AudioClip TownBGM;
    [SerializeField] private AudioClip ForestBGM;
    [SerializeField] private AudioClip CreditBGM;
    [SerializeField] private AudioClip TitleBGM;
    //SFX
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip itemSFX;
    [SerializeField] private AudioClip landingSFX;
    [SerializeField] private AudioClip damagedSFX;
    [SerializeField] private AudioClip tocuhSFX;
    [SerializeField] private AudioClip dialogSFX;

    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;

    private float originalBgmVolume;
    private float originalSfxVolume;
    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;

    //Singleton
    public static AudioManager instance { get; private set; }

    //Setter getter
    public void SetBgmVolume(float bgmV) { bgmVolume = bgmV; }
    public void SetSFXVolume(float sfxV) { sfxVolume = sfxV; }
    public void SetOriginalBgmvolume(float obv) { originalBgmVolume = obv; }
    public void SetOriginalSfxVolume(float osv) { originalSfxVolume = osv; }
    public float GetBgmVolume() { return bgmVolume; }
    public float GetSfxVolume() { return sfxVolume; }
    public float GetOriginalBgmVolume() { return originalBgmVolume; }
    public float GetOriginalSfxVolume() { return originalSfxVolume; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        bgmAudioSource = bgmObj.GetComponent<AudioSource>();
        sfxAudioSource = sfxObj.GetComponent<AudioSource>();
        LoadVolumeFromData();
        UpdateBGM(bgmVolume);
        UpdateSFX(sfxVolume);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    //Load bgm, sfx volumes from custom binary file
    public void LoadVolumeFromData()
    {
        AudioData audioData = SaveSystem.LoadVolumeData();
        if(audioData == null)
        {
            bgmVolume = 1f;
            sfxVolume = 1f;
        }
        else
        {
            bgmVolume = audioData.bgmVolume;
            sfxVolume = audioData.sfxVolume;
        }
        originalBgmVolume = bgmVolume;
        originalSfxVolume = sfxVolume;
    }

    //Save bgm, sfx volumes to custom binary file
    public void SaveVolumeToData()
    {
        SaveSystem.SaveVolumeData(this);
    }

    public void BGMSetting(string stage)
    {
        bgmAudioSource.pitch = 1.0f;
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.playOnAwake = true;
        bgmAudioSource.loop = true;

        if (stage == "stage0")
        {
            SceneLoader.instance.SetCurScene(SceneLoader.Scene.Village);
        }
        else if (stage == "Stage1")
        {
            SceneLoader.instance.SetCurScene(SceneLoader.Scene.Stages);
        }
        else if (stage == "CreditPage")
        {
            SceneLoader.instance.SetCurScene(SceneLoader.Scene.Credit);
        }
        else if (stage == "TitleMenuScene")
        {
            SceneLoader.instance.SetCurScene(SceneLoader.Scene.Title);
        }
        else if (stage == "TutorialScene")
        {
            SceneLoader.instance.SetCurScene(SceneLoader.Scene.Tutorial);
        }
        SelectBGM();
        if (stage != "stage2" && stage != "stage3")
            bgmAudioSource.Play();
    }

    private void SelectBGM()
    {
        switch (SceneLoader.instance.GetCurScene())
        {
            case SceneLoader.Scene.Title:
                bgmAudioSource.clip = TitleBGM;
                break;
            case SceneLoader.Scene.Village:
                bgmAudioSource.clip = TownBGM;
                break;
            case SceneLoader.Scene.Stages:
                bgmAudioSource.clip = ForestBGM;
                break;
            case SceneLoader.Scene.Credit:
                bgmAudioSource.clip = CreditBGM;
                break;
            case SceneLoader.Scene.Tutorial:
                bgmAudioSource.clip = ForestBGM;
                break;
        }
    }

    //Called before Setting menu being opened
    public void UpdateOriginalVolume()
    {
        originalBgmVolume = bgmAudioSource.volume;
        originalSfxVolume = sfxAudioSource.volume;
    }

    //Called if cancel button is pressed on setting page
    public void UndoVolume()
    {
        bgmAudioSource.volume = originalBgmVolume;
        sfxAudioSource.volume = originalSfxVolume;
    }

    public void UpdateBGM(float newVolume)
    {
        bgmVolume = newVolume;
        bgmAudioSource.volume = bgmVolume;
    }

    public void UpdateSFX(float newVolume)
    {
        sfxVolume = newVolume;
        sfxAudioSource.volume = sfxVolume;
    }

    public void PlayAttackSFX()
    {
        sfxAudioSource.PlayOneShot(attackSFX);
    }

    public void PlayLandingSFX()
    {
        sfxAudioSource.PlayOneShot(landingSFX);
    }

    public void PlayItemSFX()
    {
        sfxAudioSource.PlayOneShot(itemSFX);
    }

    public void PlayDamagedSFX()
    {
        sfxAudioSource.PlayOneShot(damagedSFX);
    }

    public void PlayJumpSFX()
    {
        sfxAudioSource.PlayOneShot(jumpSFX);
    }

    public void PlayTouchSFX()
    {
        sfxAudioSource.PlayOneShot(tocuhSFX);
    }

    public void PlayDialogSFX()
    {
        sfxAudioSource.PlayOneShot(dialogSFX);
    }
}