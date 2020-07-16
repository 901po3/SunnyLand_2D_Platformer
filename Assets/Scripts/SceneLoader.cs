/*
 * Class: SceneLoader
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: Managing Scene interactions.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int curStage = 1;

    public int playerLife;

    //Singleton
    public static SceneLoader instance { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if (curStage == 1)
        {
            PlayerController.instance.SetLife(3);
        }
        else if(curStage > 1)
        {
            PlayerController.instance.SetLife(playerLife);
        }
    }

    public void LoadNextScene()
    {
        curStage++;
        playerLife = PlayerController.instance.GetLife();
        SceneManager.LoadScene("Stage" + (curStage));
        DontDestroyOnLoad(this.gameObject);
    }
}
