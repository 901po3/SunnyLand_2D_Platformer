/*
 * Class: FloatingMovement
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Make Gameobject move up and down to give floating feels.
*/
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [SerializeField] private float intensity = 1f;
    [SerializeField] private float time = 1f;
    private float yOrigin; //original y pos;
    private float curTime = 0;
    
    private void Start()
    {
        yOrigin = transform.position.y;
        InvokeRepeating("Floating", 0.01f, (0.01f / time));
    }

     private void Floating()
    {
        if (curTime < 1)
        {
            curTime += 0.01f / time;
            if (curTime >= 1)
                curTime = 0;
        }
        float y = Mathf.Sin(curTime * 2 * Mathf.PI);
        transform.position = new Vector3(transform.position.x, transform.position.y + y * intensity, transform.position.z);
    }
}
