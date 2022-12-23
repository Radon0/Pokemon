using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���x���ɉ������X�e�[�^�X�̈Ⴄ�����X�^�[�𐶐�����N���X
//�f�[�^�݂̂������F����C#�̃N���X
public class Pokemon
{
    //�x�[�X�ƂȂ�f�[�^
    PokemonBase _base;
    int level;

    public int HP { get; set; }

    //�g����Z
    public List<Skill> Skills { get; set; }

   //�R���X�g���N�^�\�F�������̏����ݒ�
   public Pokemon(PokemonBase pBase,int pLevel)
    {
        _base = pBase;
        level = pLevel;
        HP = pBase.MaxHP;

        //�g����Z�̐ݒ�G�o����Z�̃��x���ȏ�Ȃ�AMoves�ɒǉ�
        foreach(LearnableSkill learnableSkill in pBase.LearnableSkills)
        {
            if(level >= learnableSkill.Level)
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
