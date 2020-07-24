/*
 * Class: Destination
 * Date: 2020.7.16
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: move to next stage when player reaches to the destination.
*/

using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private string nextStageName;

    private bool loadOnce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !loadOnce)
        {
            loadOnce = true;

            if (SceneLoader.instance.GetCurScene() != SceneLoader.Scene.Tutorial)
            {
                PlayerController.instance.SetIsFronze(true);
                SceneLoader.instance.LoadNextScene(nextStageName);
            }
            else
            {
                AudioManager.instance.PlayItemSFX();
                SceneLoader.instance.SetIsTutorialSceneFinished(true);
            }
        }            
    }

}
