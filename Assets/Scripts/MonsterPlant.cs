﻿/*
 * Class: MonsterPlant
 * Date: 2020.7.15
 * Last Modified : 2020.7.16
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
    [SerializeField] bool isFacingRight;
    [SerializeField] float flipTime;
    
    private bool isAttacking = false;
    private float attackDelay = 1.5f;
    private float curAttackDelay = 1.5f;

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
        if(curAttackDelay < attackDelay)
        {
            curAttackDelay += Time.deltaTime;
        }
        else
        {
            int layerMask = LayerMask.GetMask("Player");
            RaycastHit2D hit;
            Vector2 dir = (faceCheck.transform.position - transform.position).normalized;
            hit = Physics2D.Raycast(transform.position, dir, 1.5f, layerMask);
            isAttacking = false;
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                // Debug.Log(gameObject + " is attacking");
                isAttacking = true;
                GetComponent<Animator>().SetTrigger("isAttacking");
                PlayerController.instance.SetAttackingPlant(gameObject);
                PlayerController.instance.GetDamaged(new Vector2(80, 80));
                curAttackDelay = 0.0f;
            }
            else
            {
                Debug.DrawRay(transform.position, dir * 1.5f, Color.white);
                PlayerController.instance.SetAttackingPlant(null);
            }
        }
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
