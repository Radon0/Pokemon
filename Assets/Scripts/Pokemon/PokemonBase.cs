using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ポケモンのマスターデータ：外部から変更しない（インスペクタ―だけ変更可能）
[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    //名前、説明、画像、タイプ、ステータス

    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;

    //画像
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    //タイプ
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //ステータス:hp,at,df,sAt,sDF,sp
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    //外部から書き換えたいとき
    //public int Attack()
    //{
    //    return attack;
    //}

    //他ファイルからattackの値は取得できるが変更できない
    public int MaxHP { get => maxHP; }
    public int Attack{　get => attack; }
    public int Defense { get => defense; }
    public int SpAttack { get => spAttack; }
    public int SpDefense { get => spDefense; }
    public int Speed { get => speed; }
   
}

public enum PokemonType
{
    None,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
}
