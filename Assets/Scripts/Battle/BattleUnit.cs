using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    Vector3 originalPos;
    Color originalColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    //バトルで使うモンスターを保持
    //モンスターの画像を反映
    public void SetUp(Pokemon pokemon)
    {
        //_baseからレベルに応じたモンスターを生成する
        //BattleSystemで使うからプロパティに入れる
        Pokemon = pokemon;

        Image image = GetComponent<Image>();
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        if (!isPlayerUnit)
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        image.color = originalColor;
        PlayerEnterAnimation();
    }

    //登場Anim
    public void PlayerEnterAnimation()
    {
        if(isPlayerUnit)
        {
            //左端に配置
            transform.localPosition = new Vector3(-600, originalPos.y);
        }
        else
        {
            //右端に配置
            transform.localPosition = new Vector3(550, originalPos.y);
        }
        //戦闘時の位置までアニメーション
        transform.DOLocalMoveX(originalPos.x, 1f);
    }
    //攻撃Anim
    public void PlayerAttackAnimation()
    {
        //シーケンス
        //右に動いた後、元の位置に戻る
        Sequence sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));  //後ろに追加
        }
        else
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));  //後ろに追加
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));  //後ろに追加
    }
    //ダメージAnim
    public void PlayerHitAnimation()
    {
        //色を一度GLAYにしてから戻す
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    //戦闘不能Anim
    public void PlayerFaintAnimation()
    {
        //下にさがりながら、薄くなる
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }
}
