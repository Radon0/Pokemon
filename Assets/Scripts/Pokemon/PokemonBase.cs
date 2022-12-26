using System;
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

    //覚える技一覧
    [SerializeField] List<LearnableSkill> learnableSkills;

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

    public List<LearnableSkill> LearnableSkills { get => learnableSkills; }
    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite FrontSprite { get => frontSprite; }
    public Sprite BackSprite { get => backSprite; }
    public PokemonType Type1 { get => type1; }
    public PokemonType Type2 { get => type2; }
}

[Serializable]
public class LearnableSkill
{
    //ヒエラルキーで設定する
    [SerializeField] SkillBase _base;
    [SerializeField] int level;

    public SkillBase Base { get => _base; }
    public int Level { get => level; }
}

public enum PokemonType
{
    None,
    Normal,
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

public class TypeChart
{
    static float[][] chart =
    {
        //Atk/Def            NOR  FIR  WAT  ELE  GRS  ICE  FIG  POI
        /*NOR*/ new float[]{  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f},
        /*FIR*/ new float[]{  1f,0.5f,0.5f,  1f,  2f,  2f,  1f,  1f},
        /*WAT*/ new float[]{  1f,  2f,0.5f,  1f,0.5f,  1f,  1f,  1f},
        /*ELE*/ new float[]{  1f,  1f,  2f,0.5f,0.5f,  1f,  1f,  1f},
        /*GRS*/ new float[]{  1f,0.5f,  2f,  1f,0.5f,  1f,  1f,0.5f},
        /*ICE*/ new float[]{  1f,0.5f,0.5f,  1f,  2f,0.5f,  1f,  1f},
        /*FIG*/ new float[]{  2f,  1f,  1f,  1f,  1f,  2f,  1f,0.5f},
        /*POI*/ new float[]{  1f,  1f,  1f,  1f,  2f,  1f,  1f,0.5f},
    };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if(attackType==PokemonType.None || defenseType==PokemonType.None)
        {
            return 1f;
        }
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;
        return chart[row][col];
    }
}
