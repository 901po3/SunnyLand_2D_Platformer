/*
 * Class: MonsterPlant
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: MonsterPlant's behabivour
 *                  Standing -> Dectecting -> Attacking (it doesn't move)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlant : MonoBehaviour
{
    [SerializeField] private Transform faceCheck;
    [SerializeField] private LayerMask whatIsSide;
    [SerializeField] bool isFacingRight;

    private const float checkRadius = 0.3f;

    private void Start()
    {

    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        Collider2D Collider2D = Physics2D.OverlapCircle(faceCheck.position, checkRadius, whatIsSide);

        if (Collider2D != null)
        {
            GetComponent<Animator>().SetTrigger("isAttacking");
        }
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
