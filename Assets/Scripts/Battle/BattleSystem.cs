using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,  //行動選択
    PlayerMove,    //技選択
    EnemyMove,
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

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} appeared.");
        yield return new WaitForSeconds(1);
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
    }

    private void Update()
    {
        if(state==BattleState.PlayerAction)
        {
            HandleActionSelection();
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

        void PlayerMove()
        {
            state = BattleState.PlayerMove;
            dialogBox.EnableDialogText(false);
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableSkillSelector(true);
        }

        //色をつけてどちらを選択してるかわかるようにする
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }
        }
    }

}
