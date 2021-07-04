using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid2D;
    CapsuleCollider2D col2D;


    bool inputRight = false;
    bool inputLeft = false;
    bool inputjump = false;

    public float movespeed;
    public float jumpPower;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<CapsuleCollider2D>();
    }


    

    void Update()
    {
        #region Move
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("Moving", true);
            //transform.Translate(Vector3.right * Time.deltaTime * movespeed);
            inputRight = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("Moving", true);
            //transform.Translate(Vector3.left * Time.deltaTime * movespeed);
            inputLeft = true;
        }
        else
            animator.SetBool("Moving", false);
        #endregion

        #region Jump
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(col2D.bounds.center, new Vector2(1, col2D.bounds.size.y * 1.1f), 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        RaycastHit2D raycastHitWall = Physics2D.BoxCast(col2D.bounds.center, new Vector2(col2D.bounds.size.x * 1.1f, 1), 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));

        bool isGround = raycastHitGround.collider != null;
        bool isWall = raycastHitWall.collider != null;


        if (isGround == false)
        {
            animator.SetBool("jumping", true);
            fall();
        }
        else
        {
            animator.SetBool("jumping", false);
            fall();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jumping"))
        {
            inputjump = true;
        }
        #endregion

        #region Move
        ////속도제한
        //if (rigid2D.velocity.x >= 2.5f)
        //    rigid2D.AddForce(new Vector2(2.5f, 0));
        //else if (rigid2D.velocity.x <= -2.5f)
        //    rigid2D.AddForce(new Vector2(-2.5f, 0));

        if (inputRight)
        {
            inputRight = false;

            if (isWall == true && raycastHitWall.point.x > transform.position.x && isGround == false)
                rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
            else if (rigid2D.velocity.x <= 2.5f)
                rigid2D.AddForce(new Vector2(movespeed, 0));
            else
                rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        }
        if (inputLeft)
        {
            inputLeft = false;            
            if (isWall == true && raycastHitWall.point.x < transform.position.x && isGround == false)
                rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
            else if (rigid2D.velocity.x >= -2.5f)
                rigid2D.AddForce(new Vector2(-movespeed, 0));
            else
                rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
        }
        #endregion

        #region jump
        if (inputjump)
        {
            inputjump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
        }

        #endregion
    }


    void fall()
    {
        if (animator.GetBool("jumping") == true)
        {
            animator.SetBool("falling", true);
        }
        else if (animator.GetBool("jumping") == false)
        {
            animator.SetBool("falling", false);
        }
    }
}
