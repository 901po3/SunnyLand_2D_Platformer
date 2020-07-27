/*
 * Class: AudioData
 * Date: 2020.7.22
 * Last Modified : 2020.7.24
 * Author: Hyukin Kwon 
 * Description: 오디오 볼륨 데이터 스토리지
*/

using UnityEngine;

[System.Serializable]
public class AudioData
{
    public float bgmVolume;
    public float sfxVolume;

    //편의를 위한 구조체 생성
    public AudioData(AudioManager audioManager)
    {
        bgmVolume = audioManager.GetBgmVolume();
        sfxVolume = audioManager.GetSfxVolume();
    }
}
