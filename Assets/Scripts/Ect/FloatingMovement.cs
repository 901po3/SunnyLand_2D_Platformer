﻿/*
 * Class: FloatingMovement
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: 게임오브젝트를 위아래로 반복해서 움직이는 효과를 부여함
*/
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [SerializeField] private float intensity = 1f; //흔들림 강도
    [SerializeField] private float time = 1f; //(왕복하는데 걸리는 시간) * (0.5)
    private float curTime = 0;
    
    private void Start()
    {
        InvokeRepeating("Floating", 0.01f, (0.01f / time));
    }

    //상하이동 함수
     private void Floating()
    {
        //시간을 x로 위치를 y로 보고 Sin그래프를 이용
        if (curTime < 1)
        {
            curTime += 0.01f / time;
            if (curTime >= 1)
                curTime = 0;
        }
        float y = Mathf.Sin(curTime * 2 * Mathf.PI);
        transform.position = new Vector3(transform.position.x, transform.position.y + y * intensity, transform.position.z);
    }
}
