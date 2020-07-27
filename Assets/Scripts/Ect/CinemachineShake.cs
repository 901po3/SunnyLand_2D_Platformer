/*
 * Class: CinemachineShake
 * Date: 2020.7.15
 * Last Modified : 2020.7.24
 * Author: Hyukin Kwon 
 * Description: 시네머시 카매라를 사용해 카매라 흔들림 효과 만들고 제어하기
*/

using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer; //현재 카매라를 얼마나 흔들었는지
    private float totalShakeTimer; //점차적으로 흔들림 강도를 조절하기 위한 변수
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

    //카매라 흔들림 효과 부과 함수
    public void CameraShake(float intensity, float time)
    {
        //씬에 있는 시네머신 카매라를 받아 흔들림 효과를 부여 (강도와 시간 설정)
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        totalShakeTimer = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            //카매라의 흔들림 강도를 시간에 따라 점차적으로 줄임
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0.0f, shakeTimer / totalShakeTimer);

            //설정한 시간이 지나면 흔들림 멈춤
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.0f;
            }
        }
    }
}
