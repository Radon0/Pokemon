using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�|�P�����̃}�X�^�[�f�[�^�F�O������ύX���Ȃ��i�C���X�y�N�^�\�����ύX�\�j
[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    //���O�A�����A�摜�A�^�C�v�A�X�e�[�^�X

    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;

    //�摜
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    //�^�C�v
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //�X�e�[�^�X:hp,at,df,sAt,sDF,sp
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    //�o����Z�ꗗ
    [SerializeField] List<LearnableSkill> learnableSkills;

    //�O�����珑�����������Ƃ�
    //public int Attack()
    //{
    //    return attack;
    //}

    //���t�@�C������attack�̒l�͎擾�ł��邪�ύX�ł��Ȃ�
    public int MaxHP { get => maxHP; }
    public int Attack{�@get => attack; }
    public int Defense { get => defense; }
    public int SpAttack { get => spAttack; }
    public int SpDefense { get => spDefense; }
    public int Speed { get => speed; }

    public List<LearnableSkill> LearnableSkills { get => learnableSkills; }
}

[Serializable]
public class LearnableSkill
{
    //�q�G�����L�[�Őݒ肷��
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
