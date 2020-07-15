/*
 * Class: Enemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Top parent class of all Enemy.
*/

using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Transform[] wayPoints;
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected int curWayPoint = 0;
    [SerializeField] protected float speed;

    protected bool isDead = false;

    protected virtual void Update()
    {
        Death();
    }

    protected virtual void Death() 
    {
        if (isDead) return;
        if(PlayerController.instance.GetEnemyBelow() == gameObject)
        {
            isDead = true;
        }
    }
    protected virtual void Move() 
    {
        //if (Vector2.Distance(wayPoints[curWayPoint] && !isFacingRight)
        //{
        //    Flip();
        //}
        //else if (horizontalMove < 0f && isFacingRight)
        //{
        //    Flip();
        //}
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
