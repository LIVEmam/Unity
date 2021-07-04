using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;//다음 행동지표를 결정할 변수
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D ccollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5);
        // 초기화 함수 안에 넣어서 실행될 때 마다(최초 1회) nextMove변수가 초기화 되도록함 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ccollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //nextMove 에 0:멈춤 -1:왼쪽 1:오른쪽 으로 이동 

        //Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)

        //자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        //한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom"));

        //탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if (raycast.collider == null)
        {
            Turn();
            //nextMove = nextMove * (-1); //우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
            //CancelInvoke(); //think를 잠시 멈춘 후 재실행
            //Invoke("Think", 5);
        }

        void Think() //몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤  으로 3가지 행동을 판단
        {
            //Set Next Active
            //Random.Range : 최소<= 난수 <최대 /범위의 랜덤 수를 생성(최대는 제외이므로 주의해야함)
            nextMove = Random.Range(-1, 2);

            //Sprite Animation
            //WalkSpeed변수를 nextMove로 초기화 
            animator.SetBool("WalkSpeed", true);


            //Flip Sprite
            if (nextMove != 0) //서있을 때 굳이 방향을 바꿀 필요가 없음 
                spriteRenderer.flipX = nextMove == 1; //nextmove 가 1이면 방향을 반대로 변경  

            float time = Random.Range(2f, 5f); //생각하는 시간을 랜덤으로 부여 
            //Think(); : 재귀함수 : 딜레이를 쓰지 않으면 CPU과부화 되므로 재귀함수쓸 때는 항상 주의 
            //->Think()를 직접 호출하는 대신 Invoke()사용
            Invoke("Think", time); //매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행 
        }

        void Turn()
        {

            nextMove = nextMove * (-1); //우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
            spriteRenderer.flipX = nextMove == 1;

            CancelInvoke(); //think를 잠시 멈춘 후 재실행
            Invoke("Think", 2);

        }

    }

    public void OnDamanged()
    {
        //알파
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //플립y
        spriteRenderer.flipY = true;

        //콜라인더 Disable
        ccollider.enabled = false;

        //die effect jump
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        //사라지기
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
