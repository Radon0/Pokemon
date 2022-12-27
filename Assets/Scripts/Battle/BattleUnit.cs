using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    Vector3 originalPos;
    Color originalColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    //�o�g���Ŏg�������X�^�[��ێ�
    //�����X�^�[�̉摜�𔽉f
    public void SetUp(Pokemon pokemon)
    {
        //_base���烌�x���ɉ����������X�^�[�𐶐�����
        //BattleSystem�Ŏg������v���p�e�B�ɓ����
        Pokemon = pokemon;

        Image image = GetComponent<Image>();
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        if (!isPlayerUnit)
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        image.color = originalColor;
        PlayerEnterAnimation();
    }

    //�o��Anim
    public void PlayerEnterAnimation()
    {
        if(isPlayerUnit)
        {
            //���[�ɔz�u
            transform.localPosition = new Vector3(-600, originalPos.y);
        }
        else
        {
            //�E�[�ɔz�u
            transform.localPosition = new Vector3(550, originalPos.y);
        }
        //�퓬���̈ʒu�܂ŃA�j���[�V����
        transform.DOLocalMoveX(originalPos.x, 1f);
    }
    //�U��Anim
    public void PlayerAttackAnimation()
    {
        //�V�[�P���X
        //�E�ɓ�������A���̈ʒu�ɖ߂�
        Sequence sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));  //���ɒǉ�
        }
        else
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));  //���ɒǉ�
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));  //���ɒǉ�
    }
    //�_���[�WAnim
    public void PlayerHitAnimation()
    {
        //�F����xGLAY�ɂ��Ă���߂�
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    //�퓬�s�\Anim
    public void PlayerFaintAnimation()
    {
        //���ɂ�����Ȃ���A�����Ȃ�
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }
}
