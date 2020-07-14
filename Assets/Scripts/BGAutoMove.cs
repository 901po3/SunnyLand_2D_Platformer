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

    private void Update()
    {
        SideMove();
    }

     //Description: Background infinte loop movement function. 
    private void SideMove()
    {
        for (int i = 0; i < Children.Length; i++)
        {
            SpriteRenderer sp = Children[i].GetComponent<SpriteRenderer>();
            Children[i].Translate(Vector3.left * speed * Time.deltaTime);

            if (sp.bounds.max.x + sp.size.x/2 < Camera.main.rect.xMin)
            {
                Children[i].position = new Vector3(Children[(i + 1) % Children.Length].position.x + sp.bounds.size.x - 0.5f, Children[i].position.y, Children[i].position.z);         
            }
        }
    }
}
