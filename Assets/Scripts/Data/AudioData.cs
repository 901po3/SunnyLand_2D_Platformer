/*
 * Class: AudioData
 * Date: 2020.7.22
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: save audio volume data
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public float bgmVolume;
    public float sfxVolume;

    public AudioData(AudioManager audioManager)
    {
        bgmVolume = audioManager.GetBgmVolume();
        sfxVolume = audioManager.GetSfxVolume();
    }
}
