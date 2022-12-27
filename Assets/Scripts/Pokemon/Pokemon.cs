using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
//���x���ɉ������X�e�[�^�X�̈Ⴄ�����X�^�[�𐶐�����N���X
//�f�[�^�݂̂������F����C#�̃N���X
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    //�x�[�X�ƂȂ�f�[�^
    public PokemonBase Base { get => _base; }
    public int Level { get => level; }

    public int HP { get; set; }

    //�g����Z
    public List<Skill> Skills { get; set; }

   //�R���X�g���N�^�\�F�������̏����ݒ�
   public void Init()
    {
        HP = MaxHP;

        Skills = new List<Skill>();
        //�g����Z�̐ݒ�G�o����Z�̃��x���ȏ�Ȃ�AMoves�ɒǉ�
        foreach(LearnableSkill learnableSkill in Base.LearnableSkills)
        {
            if(Level >= learnableSkill.Level)
            {
                //�Z���o����
                Skills.Add(new Skill(learnableSkill.Base));
            }

            //4�ȏ�̋Z�͎g���Ȃ�
            if(Skills.Count >=4)
            {
                break;
            }
        }
    }

    //level�ɉ������X�e�[�^�X��Ԃ����́F�v���p�e�B�i�����������邱�Ƃ��ł���j
    //�֐��o�[�W����
    //public int Attack()
    //{
    //    return Mathf.FloorToInt(_base.Attack * level / 100f) + 5;
    //}
    //�v���p�e�B
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
        //�N���e�B�J��
        float critical = 1f;
        //6.25%�ŃN���e�B�J��
        if(Random.value * 100<=6.25f)
        {
            critical = 2f;
        }
        //����
        float type = TypeChart.GetEffectiveness(skill.Base.Type, Base.Type1) * TypeChart.GetEffectiveness(skill.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type
        };

        float attack = (skill.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;  //�������Z�q?:
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
    public bool Fainted { get; set; } //�퓬�s�\���ǂ���
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
