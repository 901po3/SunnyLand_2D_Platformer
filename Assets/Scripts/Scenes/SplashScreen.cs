/*
 * Class: SplashScreen
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: SplashScreen
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

    IEnumerator MoveToTitleScreen()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("TitleMenuScene");
    }
}
