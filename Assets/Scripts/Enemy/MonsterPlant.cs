/*
 * Class: MonsterPlant
 * Date: 2020.7.15
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: 죽지않는 장애물 타입 적이다.
 *              기본 패턴은 방향 전환 -> 뱡향에 목표 탐지및 공격
*/

using System.Collections;
using UnityEngine;

public class MonsterPlant : MonoBehaviour
{
    [SerializeField] private Transform faceCheck; //바라보는 곳에 적이 있다면 적을 담는다.
    [SerializeField] bool isFacingRight;
    [SerializeField] float flipTime; //자동 회전 주기

    private bool canAttack = false; //공격 가능한지 체크
    private bool isAttacking = false;
    private float attackFrequency = 1.5f; //공격 주기
    private float curAttackFrequency = 1.5f; //현재 공격 주기

    private void Start()
    {
        InvokeRepeating("Flip", 0.0f, flipTime);
    }

    private void Update()
    {
        Attack();
    }


    //탑지하고 공격한다
    private void Attack()
    {
        //공격 주기 체크
        //처음에 curAttackFrequency이 이미 attackFrequency와 같게 설정해서 첫 공격 이후 부터 주기 체크를 실행한다
        if (curAttackFrequency < attackFrequency)
        {
            curAttackFrequency += Time.deltaTime;
        }
        else  
        {
            int layerMask = LayerMask.GetMask("Player");
            RaycastHit2D hit;
            Vector2 dir = (faceCheck.transform.position - transform.position).normalized;
            hit = Physics2D.Raycast(transform.position, dir, 1.5f, layerMask);
            isAttacking = false;
            if (hit.collider != null) //목표가 공격 범위에 들어오면 공격 시작(데미지 넣는게 아님)
            {
                canAttack = true;
                isAttacking = true;
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                StartCoroutine(AttackDelay()); //데미지 넣는 타이밍 조절 (플레이어에게 피할 시간이 주어진다)
            }
            else //목표가 탐지되지 않으면 공격 중단
            {
                canAttack = false;
                Debug.DrawRay(transform.position, dir * 1.5f, Color.white);
            }
        }
    }

    //데미지 넣는 타이밍 조절 (플레이어에게 피할 시간이 주어진다) 
    IEnumerator AttackDelay()
    {
        //에니메이션과 싱크를 맞춘다.
        GetComponent<Animator>().SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.5f);
        if(canAttack)
        {
            canAttack = false;
            PlayerController.instance.GetDamaged(gameObject, new Vector2(80, 80));       
        }
        curAttackFrequency = 0.0f; //공격이 끝나고 현재 공격 주기를 초기화
    }

    //이미지 플립을 위한 유틸리티 함수
    protected void Flip()
    {
        //공격 목표가 없을때만 회전
        if(!isAttacking && !canAttack)
        {
            isFacingRight = !isFacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
