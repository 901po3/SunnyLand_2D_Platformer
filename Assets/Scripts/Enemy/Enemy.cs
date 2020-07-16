/*
 * Class: Enemy
 * Date: 2020.7.15
 * Last Modified : 2020.7.15
 * Author: Hyukin Kwon 
 * Description: Top parent class of all Enemy.
*/

using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected bool isFacingRight;

    protected bool isDead = false;
    protected bool isMoving = false;
    protected Animator anim = null;
    protected Rigidbody2D rigidbody2D = null;

    //setter getter
    public bool GetIsMoving() { return isMoving; }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        Death();
    }

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

    protected void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
