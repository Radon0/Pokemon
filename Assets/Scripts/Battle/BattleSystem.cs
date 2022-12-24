using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //ÉÇÉìÉXÉ^Å[ÇÃê∂ê¨Ç∆ï`âÊ
        playerUnit.SetUp();
        enemyUnit.SetUp();
        //HUDÇÃï`âÊ
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} appeared.");
        yield return new WaitForSeconds(1);
        dialogBox.EnableActionSelector(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableDialogText(false);
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableSkillSelector(true);
        }
    }

}
