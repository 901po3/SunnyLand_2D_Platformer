/*
 * Class: Destination
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
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
        if(collision.gameObject.tag == "Player")
        {
            if(!loadOnce)
            {
                loadOnce = true;
                PlayerController.instance.SetIsFronze(true);
                SceneLoader.instance.LoadNextScene(nextStageName);
            }
        }
    }

}
