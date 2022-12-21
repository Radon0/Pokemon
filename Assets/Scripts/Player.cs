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
       //動いていない時
       if(!isMoving)
        {
            //キーボード入力を受け付ける
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //斜め移動
            if(input.x != 0)
            {
                input.y = 0;
            }

            //入力があったら
            if(input != Vector2.zero)
            {
                //向きを変えたい
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                //入力分を追加
                Vector2 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }
    
    //コルーチンを使って徐々に目的地に近づける
    IEnumerator Move(Vector3 targetPos)
    {
        //移動中は入力を受け付けたくない
        isMoving = true;

        //targetと現在のpositionの差がある間は、MoveTowardsでtargetPosに近く
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,          //現在の場所
                targetPos,                   //目的地
                moveSpeed * Time.deltaTime   //スピード
                );
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
}
