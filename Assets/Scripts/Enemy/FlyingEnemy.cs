/*
 * Class: FlyingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: Enemy that flys
*/

using UnityEngine;

public class FlyingEnemy : WalkingEnemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Death()
    {
        base.Death();
    }

    protected override void Move()
    {
        base.Move();

        int idx = curWayPoint % wayPoints.Length;

        float dis = Vector2.Distance(wayPoints[idx].position, transform.position);
        Vector2 targetDir = (wayPoints[idx].position - transform.position).normalized;
        if (dis > 0.25f) //move to target
        {
            transform.Translate(new Vector2(0, targetDir.y) * speed * Time.deltaTime);
            isMoving = true;
        }
    }
}
