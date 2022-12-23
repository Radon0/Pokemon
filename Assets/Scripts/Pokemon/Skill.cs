using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill 
{
    //Pokemonが実際に使うときの技データ

    //技のマスターデータを持つ
    //使いやすいようにするためPPをもつ

    //Pokemon.csが参照するのでpublicにしておく
    public SkillBase Base { get; set; }
    public int PP { get; set; }


    //初期設定
    public Skill(SkillBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}
