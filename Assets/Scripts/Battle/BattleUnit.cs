using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;  //��킹�郂���X�^�[���Z�b�g����
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    //�o�g���Ŏg�������X�^�[��ێ�
    //�����X�^�[�̉摜�𔽉f
    public void SetUp()
    {
        //_base���烌�x���ɉ����������X�^�[�𐶐�����
        //BattleSystem�Ŏg������v���p�e�B�ɓ����
        Pokemon = new Pokemon(_base, level);

        Image image = GetComponent<Image>();
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        if (!isPlayerUnit)
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
    }
}
