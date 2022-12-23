using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//レベルに応じたステータスの違うモンスターを生成するクラス
//データのみを扱う：純粋C#のクラス
public class Pokemon
{
    //ベースとなるデータ
    PokemonBase _base;
    int level;

   //コンストラクタ―：生成時の初期設定
   public Pokemon(PokemonBase pBase,int pLevel)
    {
        _base = pBase;
        level = pLevel;
    }

    //levelに応じたステータスを返すもの：プロパティ（処理を加えることができる）
    //関数バージョン
    //public int Attack()
    //{
    //    return Mathf.FloorToInt(_base.Attack * level / 100f) + 5;
    //}
    //プロパティ
    public int Attack
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((_base.Defense * level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((_base.SpAttack * level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((_base.SpDefense * level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((_base.MaxHP * level) / 100f) + 10; }
    }
}
