using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;//���� �ൿ��ǥ�� ������ ����
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D ccollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5);
        // �ʱ�ȭ �Լ� �ȿ� �־ ����� �� ����(���� 1ȸ) nextMove������ �ʱ�ȭ �ǵ����� 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ccollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //nextMove �� 0:���� -1:���� 1:������ ���� �̵� 

        //Platform check(�� ���� ���������� �ڵ��� ���ؼ� ������ Ž��)

        //�ڽ��� �� ĭ �� ������ Ž���ؾ��ϹǷ� position.x + nextMove(-1,1,0�̹Ƿ� ������)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        //��ĭ �� �κоƷ� ������ ray�� ��
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        //���̸� ���� ���� ������Ʈ�� Ž�� 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom"));

        //Ž���� ������Ʈ�� null : �� �տ� ������ ����
        if (raycast.collider == null)
        {
            Turn();
            //nextMove = nextMove * (-1); //�츮�� ���� ������ �ٲپ� �־����� Think�� ��� ���߾����
            //CancelInvoke(); //think�� ��� ���� �� �����
            //Invoke("Think", 5);
        }

        void Think() //���Ͱ� ������ �����ؼ� �Ǵ� (-1:�����̵� ,1:������ �̵� ,0:����  ���� 3���� �ൿ�� �Ǵ�
        {
            //Set Next Active
            //Random.Range : �ּ�<= ���� <�ִ� /������ ���� ���� ����(�ִ�� �����̹Ƿ� �����ؾ���)
            nextMove = Random.Range(-1, 2);

            //Sprite Animation
            //WalkSpeed������ nextMove�� �ʱ�ȭ 
            animator.SetBool("WalkSpeed", true);


            //Flip Sprite
            if (nextMove != 0) //������ �� ���� ������ �ٲ� �ʿ䰡 ���� 
                spriteRenderer.flipX = nextMove == 1; //nextmove �� 1�̸� ������ �ݴ�� ����  

            float time = Random.Range(2f, 5f); //�����ϴ� �ð��� �������� �ο� 
            //Think(); : ����Լ� : �����̸� ���� ������ CPU����ȭ �ǹǷ� ����Լ��� ���� �׻� ���� 
            //->Think()�� ���� ȣ���ϴ� ��� Invoke()���
            Invoke("Think", time); //�Ű������� ���� �Լ��� time���� �����̸� �ο��Ͽ� ����� 
        }

        void Turn()
        {

            nextMove = nextMove * (-1); //�츮�� ���� ������ �ٲپ� �־����� Think�� ��� ���߾����
            spriteRenderer.flipX = nextMove == 1;

            CancelInvoke(); //think�� ��� ���� �� �����
            Invoke("Think", 2);

        }

    }

    public void OnDamanged()
    {
        //����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //�ø�y
        spriteRenderer.flipY = true;

        //�ݶ��δ� Disable
        ccollider.enabled = false;

        //die effect jump
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        //�������
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
