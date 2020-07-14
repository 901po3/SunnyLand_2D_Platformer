using System.Collections;
using UnityEngine;

/*
 * Class: BGAutoMove
 * Date: 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Background infinte loop movement.
*/

public class BGAutoMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] Children;

    float halfWidth;

    private void Start()
    {
        halfWidth = Children[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    private void Update()
    {
        SideMove();
    }

     //Description: Background infinte loop movement function. 
    private void SideMove()
    {
        for (int i = 0; i < Children.Length; i++)
        {
            
            Children[i].Translate(Vector3.left * speed * Time.deltaTime);

            if(Children[i].position.x + halfWidth <= -10)
            {
                Children[i].position = new Vector3(Children[(i + 1) % Children.Length].position.x + (halfWidth * 2) - 0.5f, Children[i].position.y, Children[i].position.z);         
            }
        }
    }
}
