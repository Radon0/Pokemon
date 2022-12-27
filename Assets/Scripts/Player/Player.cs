using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

    Animator animator;

    //�ǔ����Layer
    [SerializeField] LayerMask solidObjectsLayer;
    //���ނ画���Layer
    [SerializeField] LayerMask longGrassLayer;
    //���݈ˑ��������FUnityAction
    public UnityAction OnEncounted;
    [SerializeField] GameController gameController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void HandleUpdate()
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
                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
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
        CheckForEncounters();
    }

    //targetPos�Ɉړ��\���𒲂ׂ�֐�
    bool IsWalkable(Vector2 targetPos)
    {
        //targetPos�ɔ��a0.2f�̉~��Ray���΂��āA�Ԃ�������true
        //���̔ے肾����!��Ԃ��āA�ʂ�Ȃ�����
        //�������@
        //bool hit = Physics2D.OverlapCircle(targetPos, 0.2f, SolidObjects);
        //return !hit;
        //�������A
        return !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer);
        //�������B
        //return !Physics2D.OverlapCircle(targetPos, 0.2f, SolidObjects) == false;
    }

    //�����̏ꏊ����A�~��Layer���΂��āA���ނ�Layer�ɓ���������A�����_���G���J�E���g
    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer))
        {
            //�����_���G���J�E���g
            if(Random.Range(0,100) < 10)
            {
                Debug.Log("�����X�^�[�ɑ���");
                //gameController.StartBattle();
                OnEncounted();
                animator.SetBool("isMoving", false);
            }
        }
    }
}
