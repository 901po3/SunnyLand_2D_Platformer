/*
 * Class: BGAutoMove
 * Date: 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Background infinte loop movement.
*/

using System.Collections;
using UnityEngine;

public class BGAutoMove : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] Transform[] m_children;

    private void Update()
    {
        SideMove();
    }

     //Description: Background infinte loop movement function. 
    private void SideMove()
    {
        for (int i = 0; i < m_children.Length; i++)
        {
            SpriteRenderer sp = m_children[i].GetComponent<SpriteRenderer>();
            m_children[i].Translate(Vector3.left * m_speed * Time.deltaTime);

            if (sp.bounds.max.x + sp.size.x/2 < Camera.main.rect.xMin)
            {
                m_children[i].position = new Vector3(m_children[(i + 1) % m_children.Length].position.x 
                    + sp.bounds.size.x - 0.5f, m_children[i].position.y, m_children[i].position.z);         
            }
        }
    }
}
