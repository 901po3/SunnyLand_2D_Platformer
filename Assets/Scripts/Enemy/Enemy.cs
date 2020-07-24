/*
 * Class: Enemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: 적의 행동을 다루는 클래스들의 부모 클래스
*/

using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected bool isFacingRight;

    protected bool isDead = false;
    protected bool isMoving = false;
    protected bool isReusable = false; //적이 죽고나서 다시 사용할수 있는지 여부를 정하는 변수 - 런타임에서 생성 파괴를 피하기 위해 필요
    protected Animator anim = null;
    protected Rigidbody2D rigidbody2D = null;

    //Setter
    public void SetSpeed(float _speed) { speed = _speed; }
    public void SetIsReusable(bool _isReusable) { isReusable = _isReusable; }

    //Getter
    public bool GetIsFacingRight() { return isFacingRight; }
    public bool GetIsMoving() { return isMoving; }
    public bool GetIsReusable() { return isReusable; }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        Death(); //모든 적은 죽음
    }


    //모든 적이 죽으면 SetActive(false)로 설정
    protected virtual void Death() 
    {
        if (isDead) return;
        if(PlayerController.instance.GetEnemyBelow() == gameObject)
        {
            isDead = true;
            Debug.Log("enemy died");
            gameObject.SetActive(false);
        }
    }
    protected virtual void Move() 
    {

    }

    //이미지 플립을 위한 유틸리티 함수
    protected void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
