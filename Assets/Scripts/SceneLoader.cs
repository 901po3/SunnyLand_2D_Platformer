/*
 * Class: SceneLoader
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: Managing Scene interactions.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private float fadeOutSpeed;

    private float curFadeOutSpeed = 0;
    private int curStage = 0;
    public int playerLife = 3;

    //Singleton
    public static SceneLoader instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Debug.Log(playerLife);
    }

    private void Update()
    {
        
    }

    public void LoadNextScene()
    {
        if(curStage > 0)
        {
            playerLife = PlayerController.instance.GetLife();
            Debug.Log(playerLife);
        }
        curStage++;
        SceneManager.LoadScene("Stage" + (curStage));
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator SceneChangeEffect(float targetAlpha)
    {
        yield return new WaitForSeconds(fadeOutSpeed);

        yield return new WaitForSeconds(1f);
    }
}
