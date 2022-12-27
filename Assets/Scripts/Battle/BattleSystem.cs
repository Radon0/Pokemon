using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,  //行動選択
    PlayerSkill,    //技選択
    EnemySkill,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;  //0:fight 1:run
    int currentSkill;   //0:leftup 1:rightup 2:leftdown 3:rightdown

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        //モンスターの生成と描画
        playerUnit.SetUp();
        enemyUnit.SetUp();
        //HUDの描画
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogBox.SetSkillNames(playerUnit.Pokemon.Skills);
        yield return dialogBox.TypeDialog($"やせいの {enemyUnit.Pokemon.Base.Name} があらわれた.");
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("どうする？"));
    }

    void PlayerSkill()
    {
        state = BattleState.PlayerSkill;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableSkillSelector(true);
    }

    //PlayerSkillの実行
    IEnumerator PerformPlayerSkill()
    {
        state = BattleState.Busy;

        //技を決定
        Skill skill = playerUnit.Pokemon.Skills[currentSkill];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}は{skill.Base.Name}をつかった");
        playerUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        enemyUnit.PlayerHitAnimation();
        //Enemyダメージ計算
        DamageDetails damageDetails = enemyUnit.Pokemon.TakeDamage(skill, playerUnit.Pokemon);
        //HP反映
        yield return enemyHud.UpdateHP();
        //相性/クリティカルのメッセージ
        yield return ShowDamageDetails(damageDetails);
        //戦闘不能ならメッセージ
        //戦闘可能ならEnemySkill
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}はやられた");
            enemyUnit.PlayerFaintAnimation();
        }
        else
        {
            StartCoroutine(EnemySkill());
        }
    }

    IEnumerator EnemySkill()
    {
        state = BattleState.EnemySkill;

        //技を決定
        Skill skill = enemyUnit.Pokemon.GetRandomSkill();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}は{skill.Base.Name}をつかった");
        enemyUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        playerUnit.PlayerHitAnimation();
        //Enemyダメージ計算
        DamageDetails damageDetails = playerUnit.Pokemon.TakeDamage(skill, enemyUnit.Pokemon);
        //HP反映
        yield return playerHud.UpdateHP();
        //相性/クリティカルのメッセージ
        yield return ShowDamageDetails(damageDetails);
        //戦闘不能ならメッセージ
        //戦闘可能ならEnemySkill
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}はやられた");
            playerUnit.PlayerFaintAnimation();
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog($"急所にあたった");
        }
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"効果はバツグンだ");
        }
        else if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"効果はいまひとつのようだ…");
        }
    }


    private void Update()
    {
        if(state==BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerSkill)
        {
            HandleSkillSelection();
        }
    }

    //PlayerActionでの行動を処理する
    void HandleActionSelection()
    {
        //下を入力するとRun,上を入力するとFightになる
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            {
                currentAction++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                currentAction--;
            }
        }

        //色をつけてどちらを選択してるかわかるようにする
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerSkill();
            }
        }
    }
    void HandleSkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentSkill < playerUnit.Pokemon.Skills.Count - 1)
            {
                currentSkill++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentSkill > 0)
            {
                currentSkill--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSkill < playerUnit.Pokemon.Skills.Count - 2)
            {
                currentSkill += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSkill > 1)
            {
                currentSkill -= 2;
            }
        }

        //色をつけてどちらを選択してるかわかるようにする
        dialogBox.UpdateSkillSelection(currentSkill,playerUnit.Pokemon.Skills[currentSkill]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            //技選択のUI非表示
            dialogBox.EnableSkillSelector(false);
            //メッセージ復活
            dialogBox.EnableDialogText(true);
            //技決定の処理
            StartCoroutine(PerformPlayerSkill());
        }
    }
}
