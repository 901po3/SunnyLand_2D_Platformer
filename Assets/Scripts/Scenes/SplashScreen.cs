/*
 * Class: SplashScreen
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: 스플래쉬 스크린
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MoveToTitleScreen());
    }

    //5초뒤 타이틀 화면으로 이동
    IEnumerator MoveToTitleScreen()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("TitleMenuScene");
    }
}
