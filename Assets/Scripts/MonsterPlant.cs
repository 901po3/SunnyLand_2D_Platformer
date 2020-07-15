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
    [SerializeField] float flipTime;

    private const float checkRadius = 0.65f;
    private bool isAttacking = false;

    private void Start()
    {
        InvokeRepeating("Flip", 0.0f, flipTime);
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        RaycastHit2D hit;
        Vector2 dir = (faceCheck.transform.position - transform.position).normalized;
        hit = Physics2D.Raycast(transform.position, dir, 1.5f);
        if(hit.collider != null)
        {
            if (hit.transform.tag == "Player")
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                isAttacking = true;
                GetComponent<Animator>().SetTrigger("isAttacking");
                PlayerController.instance.SetAttackingPlant(gameObject);
            }
            else
            {
                Debug.DrawRay(transform.position, dir * 1.5f, Color.white);
                Debug.Log("Did not Hit");
                PlayerController.instance.SetAttackingPlant(null);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, dir * 1.5f, Color.white);
            Debug.Log("Did not Hit");
            PlayerController.instance.SetAttackingPlant(null);
        }


        //isAttacking = false;
        //Collider2D Collider2D = Physics2D.OverlapCircle(faceCheck.position, checkRadius, whatIsSide);

        //if (Collider2D != null)
        //{
        //    isAttacking = true;
        //    GetComponent<Animator>().SetTrigger("isAttacking");
        //    PlayerController.instance.SetAttackingPlant(gameObject);
        //}
        //else
        //{
        //    PlayerController.instance.SetAttackingPlant(null);
        //}
    }

    protected void Flip()
    {
        if(!isAttacking)
        {
            isFacingRight = !isFacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
