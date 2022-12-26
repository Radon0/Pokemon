using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightColor;
    //dialogのTextを取得して、変更する
    [SerializeField] int letterPerSecond;  //1文字当たりの時間
    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject skillSelector;
    [SerializeField] GameObject skillDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> skillTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    //変更するための関数
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //タイプ形式で文字を表示する
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

    //UIの表示/非表示

    //dialogTextの表示管理
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    //actionSelectorの表示管理
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    //skillSelectorの表示管理
    public void EnableSkillSelector(bool enabled)
    {
        skillSelector.SetActive(enabled);
        skillDetails.SetActive(enabled);
    }

    //選択中のアクションの色を変える
    public void UpdateActionSelection(int selectAction)
    {
        //selectActionが0の時はactionTexts[0] 1の時はactionText[1]の色を青にして、それ以外を黒

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

    //選択中のSkillの色を変える
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
            //覚えている数だけ反映
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

