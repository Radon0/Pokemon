using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

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

            //���͂���������
            if(input != Vector2.zero)
            {
                //���͕���ǉ�
                Vector2 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                StartCoroutine(Move(targetPos));
            }
        }
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