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
        //ÉÇÉìÉXÉ^Å[ÇÃê∂ê¨Ç∆ï`âÊ
        playerUnit.SetUp();  
        enemyUnit.SetUp();
        //HUDÇÃï`âÊ
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        StartCoroutine(dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} appeared."));
    }
}
