/*
 * Class: WalkingEnemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: 웨이포인트를 사용하며 땅을 걷는 에너미 타입 클래스
*/

using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] protected Transform[] wayPoints;
    [SerializeField] protected int curWayPoint = 0;
    [SerializeField] protected float stopTime = 1f; //각 웨이포인트에 도달하면 쉬는 시간. 0이면 바로 이동
      
    private float curStopTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void Death()
    {
        base.Death();
    }

    //웨이포인트를 이용한 이동 함수
    protected override void Move()
    {
        if (wayPoints.Length == 0) return;

        base.Move();
        //웨이포인트 이동을 위해 필요한 인덱스 넘버와 타겟 거리 설정
        int idx = curWayPoint % wayPoints.Length; 
        float dis = Vector2.Distance(wayPoints[idx].position, transform.position);
        Vector2 targetDir = (wayPoints[idx].position - transform.position).normalized;

        if (dis > 0.25f) //이동해야 할 거리가 남으면 이동
        {
            //Y가 0이므로 상하 이동은 불가능하다
            transform.Translate(new Vector2(targetDir.x, 0) * speed * Time.deltaTime);
            isMoving = true;
        }
        else //이동해야 할 거리가 없으면 설정한 쉬는 시간만큼 정지 후 다음 웨이포인트 설정
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
        
        //이동 방향에 따른 이미지 플립
        if (targetDir.x > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (targetDir.x < 0f && isFacingRight)
        {
            Flip();
        }

        anim.SetBool("isMoving", isMoving);
    }
}
