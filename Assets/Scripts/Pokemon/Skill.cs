using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill 
{
    //Pokemon�����ۂɎg���Ƃ��̋Z�f�[�^

    //�Z�̃}�X�^�[�f�[�^������
    //�g���₷���悤�ɂ��邽��PP������

    //Pokemon.cs���Q�Ƃ���̂�public�ɂ��Ă���
    public SkillBase Base { get; set; }
    public int PP { get; set; }


    //�����ݒ�
    public Skill(SkillBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}
