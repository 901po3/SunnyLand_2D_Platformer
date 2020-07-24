/*
 * Class: Destination
 * Date: 2020.7.16
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: 스테이지 목표지에 도달하면 사용되는 클래스로 다음 스테이지로 이동을 시킴
*/

using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private string nextStageName;

    private bool loadOnce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //만약 플레이어가 목표지에 도달 한번 불림
        if (collision.gameObject.tag == "Player" && !loadOnce)
        {
            loadOnce = true;

            //일반 스테이지면 다음 씬으로 바로 이동
            if (SceneLoader.instance.GetCurScene() != SceneLoader.Scene.Tutorial)
            {
                PlayerController.instance.SetIsFronze(true);
                SceneLoader.instance.LoadNextScene(nextStageName);
            }
            else
            { 
                //튜토리얼이면 추가 튜토리얼 메세지 출력 시간을 위해 튜토리얼리 끝났다고 설정만 바꿈
                AudioManager.instance.PlayItemSFX();
                SceneLoader.instance.SetIsTutorialSceneFinished(true);
            }
        }            
    }

}
