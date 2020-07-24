/*
 * Class: AudioManager
 * Date: 2020.7.20
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: 게임내의 모든 소리를 컨트롤하는 메니져 클래스
*/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //에디터에서 다른 두개의 EmptyObject에서 오디오 소스를 갖고와 BGM과 SFX로 나눠서 볼륨을 설정한다.
    [SerializeField] private GameObject bgmObj;
    [SerializeField] private GameObject sfxObj;
    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;
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

    //소리 설정에서 취소 버튼이 눌렸을때 필요한 원래 볼륨값을 담아두는 변수들
    private float originalBgmVolume; 
    private float originalSfxVolume;

    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;

    //Singleton
    public static AudioManager instance { get; private set; }

    //Setter
    public void SetBgmVolume(float _bgmVolume) { bgmVolume = _bgmVolume; }
    public void SetSFXVolume(float _sfxVolume) { sfxVolume = _sfxVolume; }
    public void SetOriginalBgmvolume(float _originalBgmVolume) { originalBgmVolume = _originalBgmVolume; }
    public void SetOriginalSfxVolume(float _originalSfxVolume) { originalSfxVolume = _originalSfxVolume; }

    //Getter
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

        //처음 생성시 데이터에서 볼륨값 받아와 적용한다.
        LoadVolumeFromData();
        UpdateBGM(bgmVolume);
        UpdateSFX(sfxVolume);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    //소리값를 받아오는 함수
    public void LoadVolumeFromData()
    {
        AudioData audioData = SaveSystem.LoadVolumeData(); //로컬 디바이스에서 데이터를 불러온다
        if(audioData == null) //데이터가 없으면 1로 초기값 설정
        {
            bgmVolume = 1f;
            sfxVolume = 1f;
        }
        else //데이터를 받아왔으면 데이터값을 적용.
        {
            bgmVolume = audioData.bgmVolume;
            sfxVolume = audioData.sfxVolume;
        }
        originalBgmVolume = bgmVolume;
        originalSfxVolume = sfxVolume;
    }

    //볼륨값을 로컬 디바이스에 저장한다. (인스턴스 종류 후 다시 실행해도 최근의 설정값을 기억함)
    public void SaveVolumeToData()
    {
        SaveSystem.SaveVolumeData(this);
    }

    //현재 스테이지에 맞는 BGM 설정
    public void BGMSetting(string stage)
    {
        bgmAudioSource.pitch = 1.0f;
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.playOnAwake = true;
        bgmAudioSource.loop = true;

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

        //스테이지 1->2->3으로 이어질땐 BGM이 계속 이어진다
        if (stage != "stage2" && stage != "stage3")
            bgmAudioSource.Play();
    }

    //설정창이 뜨기전에 기존의 소리값 저장하는 용도
    public void UpdateOriginalVolume()
    {
        originalBgmVolume = bgmAudioSource.volume;
        originalSfxVolume = sfxAudioSource.volume;
    }

    //설정창에서 취소 버튼이 눌렸을때 호출되는 함수
    public void UndoVolume()
    {
        bgmAudioSource.volume = originalBgmVolume;
        sfxAudioSource.volume = originalSfxVolume;
    }

    //BGM 볼륨이 변했을때 호출되는 함수
    public void UpdateBGM(float newVolume)
    {
        bgmVolume = newVolume;
        bgmAudioSource.volume = bgmVolume;
    }

    //SFX 볼륨이 변했을때 호출되는 함수
    public void UpdateSFX(float newVolume)
    {
        sfxVolume = newVolume;
        sfxAudioSource.volume = sfxVolume;
    }

    //외부에서 SFX를 실행할때 불리는 함수들
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