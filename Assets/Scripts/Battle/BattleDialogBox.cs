using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightColor;
    //dialog��Text���擾���āA�ύX����
    [SerializeField] int letterPerSecond;  //1����������̎���
    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject skillSelector;
    [SerializeField] GameObject skillDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> skillTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    //�ύX���邽�߂̊֐�
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //�^�C�v�`���ŕ�����\������
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }

        yield return new WaitForSeconds(0.7f);
    }

    //UI�̕\��/��\��

    //dialogText�̕\���Ǘ�
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    //actionSelector�̕\���Ǘ�
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    //skillSelector�̕\���Ǘ�
    public void EnableSkillSelector(bool enabled)
    {
        skillSelector.SetActive(enabled);
        skillDetails.SetActive(enabled);
    }

    //�I�𒆂̃A�N�V�����̐F��ς���
    public void UpdateActionSelection(int selectAction)
    {
        //selectAction��0�̎���actionTexts[0] 1�̎���actionText[1]�̐F��ɂ��āA����ȊO����

        for(int i=0; i < actionTexts.Count; i++)
        {
            if(selectAction == i)
            {
                actionTexts[i].color = highlightColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }

    //�I�𒆂�Skill�̐F��ς���
    public void UpdateSkillSelection(int selectSkill,Skill skill)
    { 
        for (int i = 0; i < skillTexts.Count; i++)
        {
            if (selectSkill == i)
            {
                skillTexts[i].color = highlightColor;
            }
            else
            {
                skillTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP {skill.PP}/{skill.Base.PP}";
        typeText.text = skill.Base.Type.ToString();
    }

    public void SetSkillNames(List<Skill> skills)
    {
        for(int i=0; i<skillTexts.Count;i++)
        {
            //�o���Ă��鐔�������f
            if(i<skills.Count)
            {
                skillTexts[i].text = skills[i].Base.Name;
            }
            else
            {
                skillTexts[i].text = ".";
            }
        }
    }
}

