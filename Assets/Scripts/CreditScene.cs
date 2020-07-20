using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.instance.PlayTouchSFX();
            SceneLoader.instance.LoadNextScene("TitleMenuScene");
        }
    }
}
