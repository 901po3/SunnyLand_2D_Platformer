/*
 * Class: WalkingEnemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: For enemy walking on the ground.
*/

using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float stopTime = 1f; //stay time at waypoints
    private float curStopTime;

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
        if (wayPoints.Length == 0) return;

        base.Move();
        int idx = curWayPoint % wayPoints.Length;

        float dis = Vector2.Distance(wayPoints[idx].position, transform.position);
        Vector2 targetDir = (wayPoints[idx].position - transform.position).normalized;
        if (dis > 0.25f) //move to target
        {
            transform.Translate(new Vector2(targetDir.x, 0) * speed * Time.deltaTime);
            isMoving = true;
        }
        else //change target
        {
            if(curStopTime < stopTime)
            {
                curStopTime += Time.deltaTime;
                isMoving = false;
            }
            else
            {
                curStopTime = 0;
                idx = curWayPoint = (curWayPoint + 1) % wayPoints.Length;
            }
        }

        if (targetDir.x > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (targetDir.x < 0f && isFacingRight)
        {
            Flip();
        }
    }
}
