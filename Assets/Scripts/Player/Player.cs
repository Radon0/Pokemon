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

    //壁判定のLayer
    [SerializeField] LayerMask solidObjectsLayer;
    //草むら判定のLayer
    [SerializeField] LayerMask longGrassLayer;
    //相互依存を解消：UnityAction
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
                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
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
        CheckForEncounters();
    }

    //targetPosに移動可能かを調べる関数
    bool IsWalkable(Vector2 targetPos)
    {
        //targetPosに半径0.2fの円のRayを飛ばして、ぶつかったらtrue
        //その否定だから!を返して、通れなくする
        //書き方①
        //bool hit = Physics2D.OverlapCircle(targetPos, 0.2f, SolidObjects);
        //return !hit;
        //書き方②
        return !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer);
        //書き方③
        //return !Physics2D.OverlapCircle(targetPos, 0.2f, SolidObjects) == false;
    }

    //自分の場所から、円のLayerを飛ばして、草むらLayerに当たったら、ランダムエンカウント
    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer))
        {
            //ランダムエンカウント
            if(Random.Range(0,100) < 10)
            {
                Debug.Log("モンスターに遭遇");
                //gameController.StartBattle();
                OnEncounted();
                animator.SetBool("isMoving", false);
            }
        }
    }
}
