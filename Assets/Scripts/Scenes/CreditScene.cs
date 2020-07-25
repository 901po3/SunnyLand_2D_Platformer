/*
 * Class: CreditScene
 * Date: 2020.7.18
 * Last Modified : 2020.7.18
 * Author: Hyukin Kwon 
 * Description: 크래딧씬 
*/
using UnityEngine;

public class CreditScene : MonoBehaviour
{
    private void Update()
    {
        //클릭하면 타이틀 화면으로
        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.instance.PlayTouchSFX();
            SceneLoader.instance.LoadNextScene("TitleMenuScene");
        }
    }
}
