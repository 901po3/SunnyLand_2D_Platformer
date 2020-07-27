/*
 * Class: FlyingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: 웨이포인트를 이용하며 하늘을 나는 에너미 타입 클래스
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

    //WalkingEnemy의 Move()함수를 이용한 FlyingEnemy의 Move()함수
    //개선점 존재 -> 상속의 이점은 살렸지만 내부적으로 중복되는 코드가 발생했음
    protected override void Move()
    {
        base.Move(); //WalkingEnemy의 Move() 함수로부터 X축 이동 설정

        int idx = curWayPoint % wayPoints.Length;

        //웨이포인트 이동을 위해 필요한 인덱스 넘버와 타겟 거리 설정
        float dis = Vector2.Distance(wayPoints[idx].position, transform.position);
        Vector2 targetDir = (wayPoints[idx].position - transform.position).normalized;
        if (dis > 0.25f) //move to target
        {
            //Y에 추가로 값을 주어 상하이동 패턴 추가
            transform.Translate(new Vector2(0, targetDir.y) * speed * Time.deltaTime);
            isMoving = true;
        }
    }
}
