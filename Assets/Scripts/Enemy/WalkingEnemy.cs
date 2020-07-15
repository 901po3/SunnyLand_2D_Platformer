/*
 * Class: WalkingEnemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: For enemy walking on the ground.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Death()
    {
        base.Death();
        if (isDead)
        {
            Debug.Log("walking enemy died");
            gameObject.SetActive(false);
        }
    }

    protected override void Move()
    {
        base.Move();
    }
}
