﻿/*
 * Class: Enemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Top parent class of all Enemy.
*/

using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float speed;
    protected bool isDead = false;

    protected virtual void Update()
    {
        Death();
    }

    protected virtual void Death() 
    {
        if (isDead) return;
    }
    protected virtual void Move() { }
}