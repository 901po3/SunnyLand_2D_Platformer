/*
 * Class: PlayerController
 * Date: 2020.7.14
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Player movement controller.
*/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; //check what's on bottom
    [SerializeField] private Transform ceilingCheck; //check what's on top
    [SerializeField] private LayerMask[] whatIsGround; //store the layer colliding with groundCheck
    [SerializeField] private float walkSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime; //original jump time
    [SerializeField] private GameObject landingEffect;

    private const float checkRadius = 0.3f; //radius for groundCheck and ceilingCheck
    private bool isFacingRight = true; //for flipping the character
    private bool isGrounded = true;
    private bool isEnemyBelow = false; // check if player's stepping on any enemy
    private Rigidbody2D rigidbody2D;
    private Animator anim;

    private float horizontalMove = 0f; //X-axis input
    private float jumpTimeCounter; //current jump time
    private bool isJumping = false;

    //Singleton
    public static PlayerController instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        landingEffect.SetActive(false);
    }

    private void Update()
    {
        AnimationHandler();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public Collider2D GetGroundCheck()
    {
        for(int i = 0; i < whatIsGround.Length; i++)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround[i]);
            if(collider2D != null)
            {
                if (collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    isEnemyBelow = true;
                    break;
                }
                else if (collider2D.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    isGrounded = true;
                    break;
                }
                return collider2D;
            }
            else
            {
                isGrounded = false;
                isEnemyBelow = false;
            }
        }
        return null;
    }

    private void Jump()
    {
        bool wasGrounded = isGrounded;
        GetGroundCheck();

        float fallingSpeed = 0.0f;
        if(rigidbody2D.velocity.y < 0.0f)
        {
            fallingSpeed = rigidbody2D.velocity.y;
        }

        if (!wasGrounded && isGrounded)
        {
            wasGrounded = isGrounded;
            if (fallingSpeed < -25)
            {
                CinemachineShake.instance.CameraShake(80.0f, 0.3f);
                StartCoroutine(LandingEffect());
            }
            else if (fallingSpeed <= -20)
            {
                CinemachineShake.instance.CameraShake(30.0f, 0.2f);
                StartCoroutine(LandingEffect());
            }
            else if (fallingSpeed < -13 && fallingSpeed > -20)
            {
                CinemachineShake.instance.CameraShake(10.0f, 0.2f);
                StartCoroutine(LandingEffect());
            }           
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x , jumpForce);
        }
        else if (Input.GetButton("Jump") && isJumping) // press longer to jump higher
        {
            if(jumpTimeCounter > 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            } 
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    IEnumerator LandingEffect()
    {
        landingEffect.transform.position = groundCheck.position;
        landingEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        landingEffect.SetActive(false);
    }

    private void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        float speed = walkSpeed * Time.fixedDeltaTime;
        if (isGrounded)
        {
            rigidbody2D.velocity = new Vector2(horizontalMove * speed, rigidbody2D.velocity.y);
            //Debug.Log(rigidbody2D.velocity);
        }
        else if(!isGrounded || rigidbody2D.velocity.y != 0) // add momentum on horizontal movement if player is not on the ground
        {
            Vector2 velocity = new Vector2(rigidbody2D.velocity.x, 0);
            if (Mathf.Abs(rigidbody2D.velocity.x) <= speed || (rigidbody2D.velocity.x >= Mathf.Abs(speed) && horizontalMove < 0) ||
                (rigidbody2D.velocity.x <= -Mathf.Abs(speed) && horizontalMove > 0))
            {
               velocity.x += horizontalMove * 30 * Time.fixedDeltaTime;
            }
            rigidbody2D.velocity = new Vector2(velocity.x, rigidbody2D.velocity.y);
            //Debug.Log(rigidbody2D.velocity);
        }

        if (horizontalMove > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0f && isFacingRight)
        {
            Flip();
        }    
    }

    private void AnimationHandler()
    {
        if (horizontalMove != 0 && isGrounded)
        {
            anim.SetBool("isWalking", true);
        }
        else if (horizontalMove == 0 && isGrounded)
        {
            anim.SetBool("isWalking", false);
        }
    }


    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
