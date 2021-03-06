﻿/*
 * Class: PlayerController
 * Date: 2020.7.14
 * Last Modified : 2020.7.25
 * Author: Hyukin Kwon 
 * Description: 플레이어 이동, 에니메이션, 상태변화를 다룸
*/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; //아래 체크 ->땅 감지용
    [SerializeField] private LayerMask[] whatIsGround; //아래에 닿을수 있는 모든 레이어를 담는다. (땅, 적, 장애물, 추락지점)
    [SerializeField] private float walkSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime; //점프 강도를 측정하는데 쓰이는 시간 변수
    [SerializeField] private GameObject landingEffect;
    [SerializeField] private GameObject enemyDeathEffect;
    [SerializeField] private GameObject[] lifeObj; //생명력 UI
    [SerializeField] private Transform respawnPos;
    [SerializeField] private int life;

    private const float checkRadius = 0.35f; //땅 체크 범위
    private GameObject enemyBelow; // 적이 아래에 있으면 담는다.
    private Rigidbody2D rigidbody2D;
    private Animator anim;

    private float horizontalMove = 0f; //X-axis 방향값
    private float jumpTimeCounter; //점프 강도를 측정하는데 쓰이는 시간 변수
    private bool isFacingRight = true;
    private bool isWalking = false;
    private bool checkCollisionOnce = false; //최적화용
    //점프 관련 불리언 변수들
    private bool isGrounded = true;
    private bool isBounced = false;
    private bool isJumping = false;
    private bool isFalling = false;
    //플레이어 상태 변화 관련 불리언 변수들
    private bool isInvincible = false; // 플레이어를 무적으로 만든다.
    private bool isFrozen = false; //플레이어를 못움직이게 한다.
    private bool isRespawning = false;
    //Input상태 관련 불리언 변수들... InputMode에서 불리언을 맴버 변수로 갖게되면 후에 코업플레이 기능이 추가되면 사용 불가능해짐.
    private bool wasJumpButtonPressed = false;
    private bool isJumpButtonPressed = false;
    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    //Setter
    public void SetEnemyBelow(GameObject _enemyBelow) { enemyBelow = _enemyBelow; }
    public void SetLife(int _life) { life = _life; }
    public void SetIsFronze(bool _isfrozen) { isFrozen = _isfrozen; }
    public void SetIsRightButtonPressed(bool _isRightButtonPressed) { isRightButtonPressed = _isRightButtonPressed; }
    public void SetIsLeftButtonPressed(bool _isLeftButtonPressed) { isLeftButtonPressed = _isLeftButtonPressed; }
    public void SetIsJumpButtonPressed(bool _isJumpButtonPressed) { isJumpButtonPressed = _isJumpButtonPressed; }
    public void SetIsHorizontalMove(float _horizontalMove) { horizontalMove = _horizontalMove; }

    //Getter
    public GameObject GetEnemyBelow() { return enemyBelow; }
    public int GetLife() { return life; }
    public bool GetIsFrozen() { return isFrozen; }
    public bool GetIsRightButtonPressed() { return isRightButtonPressed; }
    public bool GetIsLeftButtonPressed() { return isLeftButtonPressed; }
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

        //생명력 UI 현재 라이프에 따라 설정
        life = 3;
        ChangeLifeHud();
    }

    private void Update()
    {
        //팝업 메뉴가 활성화 되있지 않으면 인풋을 받는다.
        if(!SceneLoader.instance.GetIsSettingMenuOn() && !SceneLoader.instance.GetIsGameOverMenuOn())
        {
            InputMode.PlayerInputHandler(this);
        }
    }

    private void FixedUpdate()
    {
        //Rigidbody를 사용하는 함수들은 FixedUpdate()에서 사용한다.
        if (!isFrozen)
        {
            Jump();
            Move();
        }
    }

    //(땅, 적, 장애물) 물체가 아래 있는지 체크
    //공격 규칙상 이 함수가 초기 공격 판정으로도 사용됨.
    private void CheckBelow() 
    {
        for(int i = 0; i < whatIsGround.Length; i++)
        {
            Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround[i]);
            if(collider2D != null)
            {
                //장애물과 적은 밟는 방향과 속도까지 고려해서 판정한다.
                Vector2 dir; 
                if (collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isInvincible) //적일때 그리고 플레이어가 무적상태가 아닐때
                {
                    dir = (collider2D.transform.position - transform.position).normalized;
                    if (dir.y < 0 && rigidbody2D.velocity.y < 0f)
                    {
                        enemyBelow = collider2D.gameObject;
                        AudioManager.instance.PlayAttackSFX();
                        StartCoroutine(Bounce(450)); //적을 밟으면 반동을 준다.
                    }
                }
                else if (collider2D.gameObject.layer == LayerMask.NameToLayer("Obstacle")) //장애물일때
                {
                    dir = (collider2D.transform.position - transform.position).normalized;
                    if (dir.y < 0 && rigidbody2D.velocity.y < 0f)
                    {
                        StartCoroutine(Bounce(450)); //장애물을 밟으면 반동을 준다.
                    }
                }
                else if (collider2D.gameObject.layer == LayerMask.NameToLayer("Ground")) //땅일때
                {
                    isGrounded = true;
                    if(isFalling) //추락중이였으면 속도에따라 렌딩 연출
                    {
                        isFalling = false;
                        if (rigidbody2D.velocity.y < -25)
                        {
                            CinemachineShake.instance.CameraShake(80.0f, 0.3f);
                            StartCoroutine(PlayEffect(landingEffect, 0.4f));
                            AudioManager.instance.PlayLandingSFX();
                        }
                        if (rigidbody2D.velocity.y <= -18)
                        {
                            CinemachineShake.instance.CameraShake(20.0f, 0.2f);
                            StartCoroutine(PlayEffect(landingEffect, 0.3f));
                            AudioManager.instance.PlayLandingSFX();
                        }
                    }
                }
                break;
            }
            else
            {
                isGrounded = false;
                enemyBelow = null;
            }
        }
    }

    //점프와 추락 관련 함수 - Y축 이동
    private void Jump()
    {
        //속도로 추락 판정
        if(rigidbody2D.velocity.y < 0f)
        {
            isJumping = false;
            isFalling = true;
        }
        CheckBelow(); //위에서 속도로 isFalling이 true가 되어도 땅에 닿고있으면 false가 된다.

        if (isBounced) //적이나 장애물을 밟았을때 잠시 점프를 가능하게 해준다. <-- 바닥에서보다 더 높게 뛸수 있게 해줌.
        {
            isGrounded = true;
        }
        if (isJumpButtonPressed && isGrounded && !wasJumpButtonPressed) //점프를 시작할때
        {
            wasJumpButtonPressed = true;
            isBounced = false;
            isJumping = true;
            anim.SetBool("isJumping", isJumping);
            jumpTimeCounter = jumpTime; //더 높게 뛸때 최대 높이를 시간으로 제어해준다.
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x , jumpForce);
            AudioManager.instance.PlayJumpSFX();
        }
        if (isJumping) // 점프 버튼을 계속 눌러 더 높게 뛴다.
        {
            //최대 높이 시간 제어
            if(jumpTimeCounter > 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            } 
            else
            {
                isJumping = false; //최대 높이에 도달해 강제로 점프 종료
            }
        }
        if (!isJumpButtonPressed) //점프롤 자의로 종료
        {
            isJumping = false;
            wasJumpButtonPressed = false;
        }      

        //에니메이션
        if(rigidbody2D.velocity.y < 2f && !isJumping)
        {
            anim.SetBool("isJumping", isJumping);
        }
        anim.SetBool("isFalling", isFalling);
        anim.SetFloat("velocityY", rigidbody2D.velocity.y);
    }

    //X축 이동을 담당하는 함수
    private void Move()
    {
        float speed = walkSpeed * Time.fixedDeltaTime;
        float momentumSpeed = 30f;

        if (!isGrounded || rigidbody2D.velocity.y != 0 || isBounced) // 공중에 있으면 모맨텀을 적용한다.
        {
            //속도와 방향을 고려해 모맨텀 속도 적용
            Vector2 velocity = new Vector2(rigidbody2D.velocity.x, 0);               
            if (Mathf.Abs(rigidbody2D.velocity.x) <= speed || (rigidbody2D.velocity.x >= Mathf.Abs(speed) && horizontalMove < 0) ||
                (rigidbody2D.velocity.x <= -Mathf.Abs(speed) && horizontalMove > 0))
            {
                velocity.x += horizontalMove * momentumSpeed * Time.fixedDeltaTime;
            }
            rigidbody2D.velocity = new Vector2(velocity.x, rigidbody2D.velocity.y);
        }
        else if (isGrounded) //땅에 있다면 모맨텀 없이 바로 속도를 적용한다.
        {
            rigidbody2D.velocity = new Vector2(horizontalMove * speed, rigidbody2D.velocity.y);
        }

        //에니메이션과 플립
        if(isGrounded)
        {
            isWalking = (horizontalMove != 0) ? true : false;
        }
        if (horizontalMove > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0f && isFacingRight)
        {
            Flip();
        }
        anim.SetBool("isWalking", isWalking);
    }

    //플레이어가 공격 받을떄 호출되는 함수
    public void GetDamaged(GameObject attackingObject, Vector2 boucnePower)
    {
        //공격을 받는 방향에 따라 피해 반동 방향이 달라짐
        Vector2 dir = (attackingObject.transform.position - transform.position);      
        if (dir.x > 0)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.AddForce(new Vector2(-boucnePower.x, boucnePower.y));
        }
        else if (dir.x < 0)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.AddForce(new Vector2(boucnePower.x, boucnePower.y));
        }
        ChangeLife(); //생명력 변경
        attackingObject = null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!isInvincible && !checkCollisionOnce) //적과의 충돌을 지속적으로 검사
        {
            if ((collision.gameObject.tag == "Enemy" && enemyBelow == null)) //적에게 공격받으면
            {
                GetDamaged(collision.gameObject, new Vector2(100, 500));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //추가적인 지면 체크
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
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
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //아이템 (당근, 마법 열매)를 먹었는지 체크
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
            SceneLoader.instance.SetIsGameFinsihed(true); //게임이 끝남
            SceneLoader.instance.LoadNextScene("stage0"); //마을로 이동
        }
    }

    //추락해서 죽었을때 리스폰 위치로 이동시킨다.
    //부드러운 연출을 위해 코루틴 사용
    IEnumerator Respawn()
    {
        isFrozen = true; //플레이어를 못 움직이게 한다.
        ChangeLife();

        yield return new WaitForSeconds(0.3f);
        transform.position = respawnPos.position;
        ToIdle();
        if (!isFacingRight)
            Flip();

        yield return new WaitForSeconds(1.0f);
        horizontalMove = 0f;
        isRespawning = false;
        isFrozen = false; //리스폰이 끝났을때 다시 움직이게 해준다.

    }

    //체력의 변동이 필요할때 호출되는 함수
    private void ChangeLife()
    {
        if (life > 0)
        {
            if (SceneLoader.instance.GetCurScene() != SceneLoader.Scene.Tutorial)
            {
                life--;
            }
            anim.SetTrigger("isHurt");
            StartCoroutine(SetInvincible()); //공격 받고 체력이 달았을때 잠시 무적 상태로 돌입
            ChangeLifeHud();
            AudioManager.instance.PlayDamagedSFX();
            if (life <= 0)
            {
                GameOver.instance.TurnOnGameOver(); 
            }
        }
    }

    //적이나 장애물을 밟을때 반동을 주는 용도로 쓰이는 함수
    IEnumerator Bounce(float vPower)
    {
        isBounced = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        rigidbody2D.AddForce(Vector2.up * vPower);
        StartCoroutine(PlayEffect(enemyDeathEffect, 0.3f));

        yield return new WaitForSeconds(0.2f);
        isBounced = false;
    }

    //무적 상태 코루틴
    IEnumerator SetInvincible()
    {
        //1. 투명하게 만들고 컨트롤이 불가능해진다
        isInvincible = true;
        checkCollisionOnce = true;
        isFrozen = true;
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

        //2. 다시 컨트롤이 가능해지며 적과의 충돌로 발생한 Impulse Force를 초기화한다.
        //   적과의 충돌을 끈다.
        yield return new WaitForSeconds(0.4f);    
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        isFrozen = false;
        ToIdle();

        //3. 원래 색으로 돌아오며 적과의 충돌을 킨다.
        yield return new WaitForSeconds(0.5f);     
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);

        //4. 무적 모드가 끝난다.
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        checkCollisionOnce = false; //적과의 충돌을 다시 가능캐 한다.
        //이때 이미 콜라이더가 겹치고 있을수 있기에 적과의 충돌 체크는 OnCollisionStay2D()를 사용함.
    }

    //이미지 회전
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //체력에 맞는 HUD를 사용
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

    //플레이어 상태 초기화
    private void ToIdle()
    {
        rigidbody2D.velocity = Vector2.zero;
        isFalling = false;
        isJumping = false;
        isWalking = false;
        anim.SetBool("isFalling", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isWalking", false);
    }

    //이펙트 재생 함수
    IEnumerator PlayEffect(GameObject effect, float stopTime)
    {
        effect.transform.position = groundCheck.position;
        effect.SetActive(true);
        yield return new WaitForSeconds(stopTime);
        effect.SetActive(false);
    }
}
