/*
 * Class: JumpingEnemy
 * Date: 2020.7.16
 * Last Modified : 2020.7.16
 * Author: Hyukin Kwon 
 * Description: Enemy that jumps
*/

using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField] private int leftJumpTime; //number of jump before turning.
    [SerializeField] private int rightJumpTime; //number of jump before turning.
    [SerializeField] private float jumpFrequency; // delay between jumps
    [SerializeField] private float jumpPower; //Y force that will be applied on rigid2D velocity.x


    [SerializeField] private int curJumpTime = 0;
    private float curJumpFrequency = 0;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private float rayDis = 0.6f;

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

    protected override void Move()
    {
        base.Move();

        if(!isMoving && isGrounded) 
        {
            //checking jump delay
            if (curJumpFrequency < jumpFrequency)
            {
                curJumpFrequency += Time.fixedDeltaTime;
                if (curJumpFrequency >= jumpFrequency)
                {
                    curJumpFrequency = 0;
                    isMoving = true;

                    curJumpTime++;
                    if ((isFacingRight && curJumpTime >= rightJumpTime) ||
                        (!isFacingRight && curJumpTime >= leftJumpTime))
                    {
                        curJumpTime = 0;
                        Flip();
                    }

                    rigidbody2D.AddForce(Vector2.up * jumpPower);
                    isGrounded = false;
                    isJumping = true;
                    rayDis = 0.6f;
                }
            }
        }

        if (isJumping || isFalling) //Move horizontally
        {
            int dir = isFacingRight ? 1 : -1;
            transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            rigidbody2D.velocity = Vector2.zero;
            if(!isGrounded)
            {
                rayDis += Time.deltaTime;
            }
        }

        if (rigidbody2D.velocity.y < 0 && isJumping) //Jumping -> Falling state
        {
            isJumping = false;
            isFalling = true;
        }

        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
    }

    private void GroundCheck()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit;
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
    }
}
