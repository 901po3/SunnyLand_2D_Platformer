/*
 * Class: JumpingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: 웨이포인트 없이 점프 횟수로 이동하는 에너미 타입 클래스 
 *              예시) 좌3 우2 ->좌로 3번 점프 후 우로 2번 점프 반복
*/

using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField] private int leftJumpTime = 0; //우로 몇번 점프 할지 정하는 변수
    [SerializeField] private int rightJumpTime = 0; //좌로 몇번 점프 할지 정하는 변수
    [SerializeField] private float jumpFrequency; // 점프 주기
    [SerializeField] private int curJumpTime = 0; //점프 몇번 했는지 카운팅 하는 변수
    [SerializeField] private float jumpPower;

    private float curJumpFrequency = 0;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private float rayDis = 0.6f; //아래방향 체크를 위한 레이케스트 디스턴스 변수

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Death()
    {
        base.Death();
    }

    //점프하는 적의 이동을 다루는 함수 (웨이포인트X)
    protected override void Move()
    {
        base.Move();

        //움직이지 않고 땅에 있을때 점프 주기 체크후 주기마다 점프 실행한다.
        if(!isMoving && isGrounded) 
        {
            if (curJumpFrequency < jumpFrequency)
            {
                curJumpFrequency += Time.fixedDeltaTime;
                if (curJumpFrequency >= jumpFrequency)
                {
                    curJumpFrequency = 0;
                    isMoving = true;

                    curJumpTime++; //점프 타임이 오른쪽 및 왼쪽 점프 횟수와 같아지면 다른 방향으로 회전한다.
                    if ((isFacingRight && curJumpTime >= rightJumpTime) ||
                        (!isFacingRight && curJumpTime >= leftJumpTime))
                    {
                        curJumpTime = 0;
                        Flip();
                    }

                    rigidbody2D.AddForce(Vector2.up * jumpPower); //Y축 이동
                    isGrounded = false;
                    isJumping = true;
                    rayDis = 0.6f; //여기서 레이 길이를 초기화
                }
            }
        }

        if (isJumping || isFalling) 
        {
            int dir = isFacingRight ? 1 : -1;
            transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime); //점프중이면 X축 이동
        }
        else //점프가 끝났을때 
        {
            isMoving = false;
            rigidbody2D.velocity = Vector2.zero;
            if(!isGrounded)
            {
                //레이 디스턴스를 점차적으로 늘리는데 GroundCheck() 함수에서 목표가 닿을때까지 레이를 쏘는 용도로 사용된다.
                rayDis += Time.deltaTime;
            }
        }

        if (rigidbody2D.velocity.y < 0 && isJumping) //점프에서 떨어짐으로 변할때. (Y속도가 양수에서 음수로 변화하는 순간)
        {
            isJumping = false;
            isFalling = true;
        }

        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
    }

    //땅 체크 함수
    //추가로 경사면에 착지시 제대로 그라운드 체크가 안되는 경우가 발생할때 해결해주는 함수
    private void GroundCheck()
    {
        //기본적이 착지 용도 레이케스트
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit;
        //rayDis는 점차 늘어나기 떄문에 이번 프레임에 땅에 아무것도 안닿아도 언젠가는 닿게된다.
        //길이를 점차 늘리는 이유는 경사면에 비스듬이 착지해 현재 레이의 길이가 짧아 그라운드 체크가 실패하는 경우를 방지하기 위해서이다.
        hit = Physics2D.Raycast(transform.position, Vector2.down, rayDis, layerMask);
        if (hit.collider != null) 
        {
            Debug.DrawRay(transform.position, Vector2.down * hit.distance, Color.yellow);
            isGrounded = true;
            isFalling = false;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.white);
            isGrounded = false;
        }

        //이미 사용중인 개채의 추락 지점 체크 레이케스트 
        if(!isReusable)
        {
            layerMask = LayerMask.GetMask("FallingSpot");
            hit = Physics2D.Raycast(transform.position, Vector2.down, rayDis, layerMask);
            if (hit.collider != null)
            {
                isReusable = true;
            }
        }
    }

}
