/*
 * Class: BGAutoMove
 * Date: 2020.7.14
 * Last Modified : 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Background infinte loop movement.
*/

using System.Collections;
using UnityEngine;

public class BGAutoMove : MonoBehaviour
{
    [SerializeField] float speed; //background scrolling speed;
    [SerializeField] Transform[] children; //backgrounds

    private void Update()
    {
        SideMove();
    }

     //Description: Background infinte loop movement function. 
    private void SideMove()
    {
        for (int i = 0; i < children.Length; i++)
        {
            SpriteRenderer sp = children[i].GetComponent<SpriteRenderer>();
            children[i].Translate(Vector3.left * speed * Time.deltaTime);

            if (sp.bounds.max.x + sp.size.x/2 < Camera.main.rect.xMin)
            {
                children[i].position = new Vector3(children[(i + 1) % children.Length].position.x 
                    + sp.bounds.size.x - 0.5f, children[i].position.y, children[i].position.z);         
            }
        }
    }
}
