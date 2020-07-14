/*
 * Class: PlayerController
 * Date: 2020.7.14
 * Author: Hyukin Kwon 
 * Description: Player movement controller.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private Transform m_ceilingCheck;
    [SerializeField] private float m_walkSpeed = 6.5f;
    [SerializeField] private float m_jumpForce = 400f;
    [Range(0, .3f)] [SerializeField] private float m_movementSmoothing = .05f;

    private bool m_facingRight = true;
    private bool m_onGround = true;
    private bool m_jump = false;
    private Vector3 m_velocity = Vector3.zero;
    private Rigidbody2D m_rigidbody2D;
    private Animator anim;

    float horizontalMove = 0f;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        InputHandler();
        AnimationHandler();
    }

    private void FixedUpdate()
    {
        Move();
        GroundChecker();
    }

    private void InputHandler()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }
    }

    private void GroundChecker()
    {
        bool wasGrounded = m_onGround;
        m_onGround = false;
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * m_walkSpeed, m_rigidbody2D.velocity.y);
        m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetVelocity, ref m_velocity, m_movementSmoothing, 100, Time.fixedDeltaTime);

        if (m_onGround && m_jump)
        {
            // Add a vertical force to the player.
            m_onGround = false;
            m_rigidbody2D.AddForce(new Vector2(0f, m_jumpForce));
            m_jump = false;
        }


        if (horizontalMove > 0f && !m_facingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0f && m_facingRight)
        {
            Flip();
        }    
    }

    private void AnimationHandler()
    {
        if (horizontalMove != 0 && m_onGround)
        {
            anim.SetBool("isWalking", true);
        }
        else if (horizontalMove == 0 && m_onGround)
        {
            anim.SetBool("isWalking", false);
        }
    }


    private void Flip()
    {
        m_facingRight = !m_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
