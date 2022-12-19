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
       //動いていない時
       if(!isMoving)
        {
            //キーボード入力を受け付ける
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //入力があったら
            if(input != Vector2.zero)
            {
                //入力分を追加
                Vector2 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        //targetと現在のpositionの差がある間は、MoveTowardsでtargetPosに近く
        while((targetPos - transform.position).sqrMagnitude>Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
}
