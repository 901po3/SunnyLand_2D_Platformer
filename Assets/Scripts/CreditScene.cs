using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScene : MonoBehaviour
{
    [SerializeField] private AudioClip tocuhSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SceneLoader.instance.GetSfxVolume();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            audioSource.PlayOneShot(tocuhSound);
            SceneLoader.instance.LoadNextScene("TitleMenuScene");
        }
    }
}
