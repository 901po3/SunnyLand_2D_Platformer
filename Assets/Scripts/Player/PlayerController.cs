﻿/*
 * Class: PlayerController
 * Date: 2020.7.14
 * Last Modified : 2020.7.23
 * Author: Hyukin Kwon 
 * Description: 플레이어의 이동과 에니메이션 상태를 다룸
*/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; //check what's on bottom
    [SerializeField] private Transform[] rightChecks; //check what's on right
    [SerializeField] private Transform[] leftChecks; //check what's on left
    [SerializeField] private LayerMask[] whatIsGround; //store the layer colliding with groundCheck
    [SerializeField] private float walkSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime; //original jump time
    [SerializeField] private GameObject landingEffect;
    [SerializeField] private GameObject enemyDeathEffect;
    [SerializeField] private GameObject[] lifeObj; // Player life UI GameObject....
    [SerializeField] private Transform respawnPos;

    private const float checkRadius = 0.35f; //radius for groundCheck and ceilingCheck
    private GameObject enemyBelow; // check if player's stepping on any enemy
    private GameObject enemyRight; // check if enemy's on right
    private GameObject enemyLeft; // check if enemy's on left
    private GameObject attackingPlant; //Plant attacking player
    private Rigidbody2D rigidbody2D;
    private Animator anim;

    private float horizontalMove = 0f; //X-axis input
    private float jumpTimeCounter; //current jump time
    [SerializeField] private int life;
    private bool isFacingRight = true; //for flipping the character
    private bool isGrounded = true;
    private bool isLadnded = false;
    private bool isBounced = false;
    private bool isWalking = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isInvincible = false; // Make Player invincible
    private bool isRespawning = false;
    private bool isFrozen = false;
    private bool checkCollisionOnce = false;
    private bool wasJumpButtonPressed = false;
    private bool isJumpButtonPressed = false;
    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    //setter getter
    public void SetAttackingPlant(GameObject _attackingPlant) { attackingPlant = _attackingPlant; }
    public void SetLife(int _life) { life = _life; }
    public void SetIsFronze(bool _isfrozen) { isFrozen = _isfrozen; }
    public void SetEnemyBelow(GameObject _enemyBelow) { enemyBelow = _enemyBelow; }
    public void SetIsRightButtonPressed(bool _isRightButtonPressed) { isRightButtonPressed = _isRightButtonPressed; }
    public void SetIsLeftButtonPressed(bool _isLeftButtonPressed) { isLeftButtonPressed = _isLeftButtonPressed; }
    public void SetIsJumpButtonPressed(bool _isJumpButtonPressed) { isJumpButtonPressed = _isJumpButtonPressed; }
    public void SetIsHorizontalMove(float _horizontalMove) { horizontalMove = _horizontalMove; }

    public GameObject GetEnemyBelow() { return enemyBelow; }
    public int GetLife() { return life; }
    public bool GetIsFrozen() { return isFrozen; }
    public bool GetIsLeftButtonPressed() { return isLeftButtonPressed; }
    public bool GetIsRightButtonPressed() { return isRightButtonPressed; }
    public bool GetIsJumpButtonPressed() { return isJumpButtonPressed; }
    public bool GetWasJumpButtonPressed() { return wasJumpButtonPressed; }
    public float GetHorizontalMove() { return horizontalMove; }
    public bool GetIsGrounded() { return isGrounded; }

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
        horizontalMove = 0f;
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        enemyDeathEffect.SetActive(false);
        landingEffect.SetActive(false);

        life = 3;
        ChangeLifeHud();
    }

    private void Update()
    {
        if(!SceneLoader.instance.GetIsSettingMenuOn() && !SceneLoader.instance.GetIsGameOverMenuOn())
        {
            InputMode.PlayerInputHandler(this);
        }
        Jump();
        SideChecker();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //Check side for dectecting enemy
    private void SideChecker()
    {
        for (int i = 0; i < whatIsGround.Length; i++)
        {
            for(int j = 0; j < rightChecks.Length; j++)
            {
                Collider2D rightCollider2D = Physics2D.OverlapCircle(rightChecks[j].position, checkRadius, whatIsGround[i]);
                Collider2D leftCollider2D = Physics2D.OverlapCircle(leftChecks[j].position, checkRadius, whatIsGround[i]);

                if(rightCollider2D != null)
                {
                    enemyRight = rightCollider2D.gameObject;
                    enemyLeft = null;
                }
                else if(leftCollider2D)
                {
                    enemyRight = null;
                    enemyLeft = leftCollider2D.gameObject;
                }
            }
        }
    }

    //check ground for detecting ground, obstacle, and Enemy
    private void GroundCheck()
    {
        for(int i = 0; i < whatIsGround.Length; i++)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround[i]);
            if(collider2D != null)
            {
                if ((collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isInvincible)
                    || collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    Vector2 pos = (collider2D.transform.position - transform.position).normalized;
                    if(pos.y < 0 && rigidbody2D.velocity.y < 0f)
                    {
                        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                        {
                            enemyBelow = collider2D.gameObject;
                            AudioManager.instance.PlayAttackSFX();
                            StartCoroutine(Bounce(200));
                        }
                        else if(collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                        {
                            StartCoroutine(Bounce(200));
                        }
                        break;
                    }
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


    //Handles Jump and falling and landing effect
    private void Jump()
    {
        if (isFrozen) return;
        bool wasGrounded = isGrounded;
        GroundCheck();

        float fallingSpeed = 0.0f;
        if(rigidbody2D.velocity.y < 0f)
        {
            fallingSpeed = rigidbody2D.velocity.y;
            isJumping = false;
            isFalling = true;
        }

        if ((!wasGrounded && isGrounded) || isLadnded)
        {
            isFalling = false;
            wasGrounded = isGrounded;

            if (fallingSpeed < -30)
            {
                CinemachineShake.instance.CameraShake(100.0f, 0.3f);
                StartCoroutine(PlayEffect(landingEffect, 0.4f));
                AudioManager.instance.PlayLandingSFX();
            }
            else if (fallingSpeed < -23)
            {
                CinemachineShake.instance.CameraShake(80.0f, 0.3f);
                StartCoroutine(PlayEffect(landingEffect, 0.4f));
                AudioManager.instance.PlayLandingSFX();
            }
            else if (fallingSpeed <= -18)
            {
                CinemachineShake.instance.CameraShake(20.0f, 0.2f);
                StartCoroutine(PlayEffect(landingEffect, 0.3f));
                AudioManager.instance.PlayLandingSFX();
            }                 
        }

        if (isBounced)
        {
            isGrounded = true;
            wasJumpButtonPressed = false;
        }
        if ((isJumpButtonPressed && isGrounded && !wasJumpButtonPressed) || isBounced)
        {
            AudioManager.instance.PlayJumpSFX();
            wasJumpButtonPressed = true;
            isBounced = false;
            isJumping = true;
            anim.SetBool("isJumping", isJumping);
            jumpTimeCounter = jumpTime;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x , jumpForce);
        }
        if (wasJumpButtonPressed && isJumping) // press longer to jump higher
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
        if (!isJumpButtonPressed && wasJumpButtonPressed)
        {
            wasJumpButtonPressed = false;
            isJumping = false;
        }      
        if(rigidbody2D.velocity.y < 2f && !isJumping)
        {
            anim.SetBool("isJumping", isJumping);
        }
        anim.SetBool("isFalling", isFalling);
        anim.SetFloat("velocityY", rigidbody2D.velocity.y);
    }

    //Move player applying velocity x, y
    private void Move()
    {
        if (isFrozen) return;

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

    //This is called when player get damage
    public void GetDamaged(Vector2 boucnePower)
    {
        Vector2 dir = Vector2.zero;
        if (attackingPlant != null)
        {
            dir = (attackingPlant.transform.position - transform.position);
        }
        Debug.Log(dir);

        if (isFacingRight)
        {
            if (enemyRight || dir.x > 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.AddForce(new Vector2(-boucnePower.x, boucnePower.y));
            }
            else if (enemyLeft || dir.x < 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.AddForce(new Vector2(boucnePower.x, boucnePower.y));
            }
        }
        else
        {
            if (enemyRight || dir.x > 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.AddForce(new Vector2(boucnePower.x, boucnePower.y));
            }
            else if (enemyLeft || dir.x < 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.AddForce(new Vector2(-boucnePower.x, boucnePower.y));
            }
        }

        //remove impulse force acted on the enemy
        if (enemyRight != null)
            enemyRight.GetComponent<Rigidbody2D>().velocity 
                = new Vector2(0, enemyRight.GetComponent<Rigidbody2D>().velocity.y);
        if (enemyLeft != null)
            enemyLeft.GetComponent<Rigidbody2D>().velocity
                = new Vector2(0, enemyLeft.GetComponent<Rigidbody2D>().velocity.y);

        Damaged();
        Debug.Log(attackingPlant);
        attackingPlant = null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!isInvincible && !checkCollisionOnce)
        {
            if ((collision.gameObject.tag == "Enemy" && enemyBelow == null))
            {
                GetDamaged(new Vector2(100, 500));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isLadnded = true;
        }
        else if (collision.gameObject.tag == "FallingSpot")
        {
            if(!isRespawning)
            {
                isRespawning = true;
                StartCoroutine(Respawn());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isLadnded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(life < 3)
        {
            if (collision.gameObject.tag == "Carrot")
            {
                life++;
                ChangeLifeHud();
                Destroy(collision.gameObject);
                AudioManager.instance.PlayItemSFX();
            }
        }
        if(collision.transform.tag == "MagicFruit")
        {
            isFrozen = true;
            AudioManager.instance.PlayItemSFX();
            SceneLoader.instance.SetIsGameFinsihed(true);
            SceneLoader.instance.LoadNextScene("stage0");
        }
    }

    IEnumerator Respawn()
    {
        isFrozen = true;
        Damaged();

        yield return new WaitForSeconds(0.3f);
        transform.position = respawnPos.position;
        rigidbody2D.velocity = Vector2.zero;
        ToIdle();
        if (!isFacingRight)
            Flip();

        yield return new WaitForSeconds(1.0f);
        horizontalMove = 0f;
        isRespawning = false;
        isFrozen = false;

    }

    //This will be called inside of GetDamaged() function
    private void Damaged()
    {
        if (life > 0)
        {
            if (SceneLoader.instance.GetCurScene() != SceneLoader.Scene.Tutorial)
                life--;
            anim.SetTrigger("isHurt");
            StartCoroutine(SetInvincible());
            ChangeLifeHud();
            AudioManager.instance.PlayDamagedSFX();

            if (life <= 0)
            {
                GameOver.instance.TurnOnGameOver();
            }
        }
    }

    IEnumerator Bounce(float vPower)
    {
        isBounced = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        rigidbody2D.AddForce(Vector2.up * vPower);
        StartCoroutine(PlayEffect(enemyDeathEffect, 0.3f));

        yield return new WaitForSeconds(0.75f);
        isBounced = false;
    }

    IEnumerator SetInvincible()
    {
        isInvincible = true;
        checkCollisionOnce = true;
        isFrozen = true;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(0.4f);    
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        rigidbody2D.velocity = Vector2.zero;
        isFrozen = false;
        ToIdle();

        yield return new WaitForSeconds(0.5f);     
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);

        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        checkCollisionOnce = false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void ChangeLifeHud()
    {
        for (int i = 0; i < lifeObj.Length; i++)
        {
            if (i == life)
            {
                lifeObj[i].SetActive(true);
                continue;
            }
            lifeObj[i].SetActive(false);
        }
    }

    private void ToIdle()
    {
        isFalling = false;
        isJumping = false;
        isWalking = false;
        anim.SetBool("isFalling", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isWalking", false);
    }

    IEnumerator PlayEffect(GameObject effect, float stopTime)
    {
        effect.transform.position = groundCheck.position;
        effect.SetActive(true);
        yield return new WaitForSeconds(stopTime);
        effect.SetActive(false);
    }

}