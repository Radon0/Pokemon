using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
//レベルに応じたステータスの違うモンスターを生成するクラス
//データのみを扱う：純粋C#のクラス
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    //ベースとなるデータ
    public PokemonBase Base { get => _base; }
    public int Level { get => level; }

    public int HP { get; set; }

    //使える技
    public List<Skill> Skills { get; set; }

   //コンストラクタ―：生成時の初期設定
   public void Init()
    {
        HP = MaxHP;

        Skills = new List<Skill>();
        //使える技の設定；覚える技のレベル以上なら、Movesに追加
        foreach(LearnableSkill learnableSkill in Base.LearnableSkills)
        {
            if(Level >= learnableSkill.Level)
            {
                //技を覚える
                Skills.Add(new Skill(learnableSkill.Base));
            }

            //4つ以上の技は使えない
            if(Skills.Count >=4)
            {
                break;
            }
        }
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
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }

    public DamageDetails TakeDamage(Skill skill,Pokemon attacker)
    {
        //クリティカル
        float critical = 1f;
        //6.25%でクリティカル
        if(Random.value * 100<=6.25f)
        {
            critical = 2f;
        }
        //相性
        float type = TypeChart.GetEffectiveness(skill.Base.Type, Base.Type1) * TypeChart.GetEffectiveness(skill.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type
        };

        float attack = (skill.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;  //条件演算子?:
        float defense = (skill.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * skill.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP<=0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Skill GetRandomSkill()
    {
        int r = Random.Range(0, Skills.Count);
        return Skills[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; } //戦闘不能かどうか
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
