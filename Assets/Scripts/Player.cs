using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //�����Ă��Ȃ���
       if(!isMoving)
        {
            //�L�[�{�[�h���͂��󂯕t����
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //�΂߈ړ�
            if(input.x != 0)
            {
                input.y = 0;
            }

            //���͂���������
            if(input != Vector2.zero)
            {
                //������ς�����
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                //���͕���ǉ�
                Vector2 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }
    
    //�R���[�`�����g���ď��X�ɖړI�n�ɋ߂Â���
    IEnumerator Move(Vector3 targetPos)
    {
        //�ړ����͓��͂��󂯕t�������Ȃ�
        isMoving = true;

        //target�ƌ��݂�position�̍�������Ԃ́AMoveTowards��targetPos�ɋ߂�
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,          //���݂̏ꏊ
                targetPos,                   //�ړI�n
                moveSpeed * Time.deltaTime   //�X�s�[�h
                );
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
}
