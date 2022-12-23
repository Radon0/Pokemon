using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillBase : ScriptableObject
{
    //技のマスターデータ

    //名前、詳細、タイプ、威力、命中率、PP

    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    //他のファイルから参照するためにプロパティを使う
    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
}
