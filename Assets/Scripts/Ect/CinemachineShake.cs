/*
 * Class: CinemachineShake
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Handles CinemaMachine Camera shake.
*/

using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float totalShakeTimer;
    private float startingIntensity;

    //Singleton
    public static CinemachineShake instance { get; private set; }

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0.0f, shakeTimer / totalShakeTimer);
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.0f;
            }
        }
    }

    public void CameraShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        totalShakeTimer = time;
        shakeTimer = time;
    }
}
