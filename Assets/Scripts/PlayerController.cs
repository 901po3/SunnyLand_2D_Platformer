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
    [SerializeField] private GameObject enemyDeathEffect;
    [SerializeField] private GameObject[] lifeObj; // Player life UI GameObject....

    private const float checkRadius = 0.35f; //radius for groundCheck and ceilingCheck
    private bool isFacingRight = true; //for flipping the character
    private bool isGrounded = true;
    private bool isBounced = false;
    private GameObject enemyBelow; // check if player's stepping on any enemy
    private Rigidbody2D rigidbody2D;
    private Animator anim;

    private float horizontalMove = 0f; //X-axis input
    private float jumpTimeCounter; //current jump time
    private int life = 3;
    private bool isWalking = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isInvincible = false; // Make Player invincible

    //setter getter
    public GameObject GetEnemyBelow() { return enemyBelow; }

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

        enemyDeathEffect.SetActive(false);
        landingEffect.SetActive(false);
        lifeObj[life].SetActive(true);
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetGroundCheck()
    {
        for(int i = 0; i < whatIsGround.Length; i++)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround[i]);
            if(collider2D != null)
            {
                if (collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemyBelow = collider2D.gameObject;
                    StartCoroutine(Attack());
                    break;
                }
                else if (collider2D.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    isGrounded = true;
                    break;
                }
            }
            else
            {
                isGrounded = false;
                enemyBelow = null;
            }
        }
    }

    private void Jump()
    {
        bool wasGrounded = isGrounded;
        GetGroundCheck();

        float fallingSpeed = 0.0f;
        if(rigidbody2D.velocity.y < 0.0f)
        {
            fallingSpeed = rigidbody2D.velocity.y;
            isJumping = false;
            isFalling = true;
        }

        if(isBounced)
        {
            wasGrounded = false;
            isGrounded = true;
        }
        if ((!wasGrounded && isGrounded))
        {
            isFalling = false;
            wasGrounded = isGrounded;
            if (fallingSpeed < -25)
            {
                CinemachineShake.instance.CameraShake(80.0f, 0.3f);
                StartCoroutine(PlayEffect(landingEffect, 0.4f));
            }
            else if (fallingSpeed <= -20)
            {
                CinemachineShake.instance.CameraShake(30.0f, 0.2f);
                StartCoroutine(PlayEffect(landingEffect, 0.4f));
            }
            //else if (fallingSpeed < -13 && fallingSpeed > -20)
            //{
            //    CinemachineShake.instance.CameraShake(10.0f, 0.2f);
            //    StartCoroutine(LandingEffect());
            //}                     
        }

        if ((Input.GetButtonDown("Jump") && isGrounded))
        {
            isJumping = true;
            anim.SetBool("isJumping", isJumping);
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
        if(rigidbody2D.velocity.y < 2f && !isJumping)
        {
            anim.SetBool("isJumping", isJumping);
        }
        anim.SetBool("isFalling", isFalling);
    }

    IEnumerator PlayEffect(GameObject effect, float stopTime)
    {
        effect.transform.position = groundCheck.position;
        effect.SetActive(true);
        yield return new WaitForSeconds(stopTime);
        effect.SetActive(false);
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

        if (horizontalMove != 0 && isGrounded)
        {
            isWalking = true;
        }
        else if (horizontalMove == 0 && isGrounded)
        {
            isWalking = false;
        }
        anim.SetBool("isWalking", isWalking);

        if (horizontalMove > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0f && isFacingRight)
        {
            Flip();
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isInvincible)
        {
            if (collision.gameObject.tag == "Enemy" && enemyBelow == null)
            {
                Damaged();
            }
        }
    }

    private void Damaged()
    {
        if (life > 0)
        {
            life--;
            StartCoroutine(SetInvincible());
            //Change Image of LifeObj
            for (int i = 0; i < lifeObj.Length; i++)
            {
                if (i == life)
                {
                    lifeObj[i].SetActive(true);
                    continue;
                }
                lifeObj[i].SetActive(false);
            }

            if (life <= 0)
            {
                //GameOver;
            }
        }
    }

    IEnumerator Attack()
    {
        isBounced = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        rigidbody2D.AddForce(Vector2.up * 200);
        Debug.Log(rigidbody2D.velocity);
        isJumping = true;
        isFalling = false;
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
        StartCoroutine(PlayEffect(enemyDeathEffect, 0.3f));

        yield return new WaitForSeconds(0.75f);
        isBounced = false;
    }

    IEnumerator SetInvincible()
    {
        isInvincible = true;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(0.5f);    
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(1.5f);
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.7f);

        yield return new WaitForSeconds(0.5f);     
        isInvincible = false;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
