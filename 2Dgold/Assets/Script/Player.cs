using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 주석
    public GameManager gameManager;
    public float maxspeed;
    public float JumpPower;
    Rigidbody2D rigid;
    SpriteRenderer SpriteRenderer;
    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("Isjumping"))
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("Isjumping", true);
        }


        //stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //방향전환
        if (Input.GetButton("Horizontal"))
            SpriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("Iswalking", false);
        else
            anim.SetBool("Iswalking", true);

    }

    void FixedUpdate()
    {
        //move
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxspeed)
            rigid.velocity = new Vector2(maxspeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxspeed * (-1))
            rigid.velocity = new Vector2(maxspeed * (-1), rigid.velocity.y);

        //Debug.Log(rigid.velocity);

        //Landing Platfrom
        if (rigid.velocity.y < 0)
        {
            //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platfrom"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.52f)
                {
                    anim.SetBool("Isjumping", false);
                }
            }
        }
    }
    #endregion
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                Onattack(collision.transform);
            }
            else //damaged
                OnDamanged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            //point
            bool isBeonze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if (isBeonze)
                gameManager.stagepoint += 50;
            else if (isSilver)
                gameManager.stagepoint += 100;
            else if (isGold)
                gameManager.stagepoint += 250;

            //사라지기
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //다음맵
            gameManager.NextStage();
        }
    }
    void Onattack(Transform enemy)
    {
        //point
        gameManager.stagepoint += 100;

        //
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //enemy die
        Enemy enemy1 = enemy.GetComponent<Enemy>();
        enemy1.OnDamanged();
    }
    void OnDamanged(Vector2 tergetPos)
    {
        //Hp down
        gameManager.HpDown();

        //체인지 레이어
        gameObject.layer = 9;

        //알파값 변경
        SpriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //리지드 바디 포스
        int dirs = transform.position.x - tergetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirs, 1) * 10, ForceMode2D.Impulse);

        Invoke("offDamaged", 2);

        //에니메이션
        anim.SetTrigger("doDamaged");
    }

    void offDamaged()
    {
        gameObject.layer = 8;
        SpriteRenderer.color = new Color(1, 1, 1, 1);
    }
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D ccollider;
    public void OnDie()
    {
        //알파
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //플립y
        spriteRenderer.flipY = true;

        //콜라인더 Disable
        ccollider.enabled = false;

        //die effect jump
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}

