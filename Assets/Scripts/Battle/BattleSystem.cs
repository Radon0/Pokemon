using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState
{
    Start,
    PlayerAction,  //�s���I��
    PlayerSkill,    //�Z�I��
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
    [SerializeField] PartyScreen partyScreen;

    //[SerializeField] GameController gameController;
    public UnityAction BattleOver;


    BattleState state;
    int currentAction;  //0:fight 1:run
    int currentSkill;   //0:leftup 1:rightup 2:leftdown 3:rightdown

    PokemonParty playerParty;
    Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        //�����X�^�[�̐����ƕ`��
        playerUnit.SetUp(playerParty.GetHealthyPokemon());
        enemyUnit.SetUp(wildPokemon);
        //HUD�̕`��
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetSkillNames(playerUnit.Pokemon.Skills);
        yield return dialogBox.TypeDialog($"�₹���� {enemyUnit.Pokemon.Base.Name} �������ꂽ.");
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        dialogBox.SetDialog("�ǂ�����H");
    }

    void OpenPartyScreen()
    {
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void PlayerSkill()
    {
        state = BattleState.PlayerSkill;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableSkillSelector(true);
    }

    //PlayerSkill�̎��s
    IEnumerator PerformPlayerSkill()
    {
        state = BattleState.Busy;

        //�Z������
        Skill skill = playerUnit.Pokemon.Skills[currentSkill];
        skill.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}��{skill.Base.Name}��������");
        playerUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        enemyUnit.PlayerHitAnimation();
        //Enemy�_���[�W�v�Z
        DamageDetails damageDetails = enemyUnit.Pokemon.TakeDamage(skill, playerUnit.Pokemon);
        //HP���f
        yield return enemyHud.UpdateHP();
        //����/�N���e�B�J���̃��b�Z�[�W
        yield return ShowDamageDetails(damageDetails);
        //�퓬�s�\�Ȃ烁�b�Z�[�W
        //�퓬�\�Ȃ�EnemySkill
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}�͂��ꂽ");
            enemyUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            //gameController.EndBattle();
            BattleOver();
        }
        else
        {
            StartCoroutine(EnemySkill());
        }
    }

    IEnumerator EnemySkill()
    {
        state = BattleState.EnemySkill;

        //�Z������
        Skill skill = enemyUnit.Pokemon.GetRandomSkill();
        skill.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}��{skill.Base.Name}��������");
        enemyUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        playerUnit.PlayerHitAnimation();
        //Enemy�_���[�W�v�Z
        DamageDetails damageDetails = playerUnit.Pokemon.TakeDamage(skill, enemyUnit.Pokemon);
        //HP���f
        yield return playerHud.UpdateHP();
        //����/�N���e�B�J���̃��b�Z�[�W
        yield return ShowDamageDetails(damageDetails);
        //�퓬�s�\�Ȃ烁�b�Z�[�W
        //�퓬�\�Ȃ�EnemySkill
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}�͂��ꂽ");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            //gameController.EndBattle();
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                //�����X�^�[�̐����ƕ`��
                playerUnit.SetUp(nextPokemon);
                //HUD�̕`��
                playerHud.SetData(nextPokemon);
                dialogBox.SetSkillNames(nextPokemon.Skills);
                yield return dialogBox.TypeDialog($"�����I {nextPokemon.Base.Name}!");
                PlayerAction();
            }
            else
            {
                BattleOver();
            }
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
            yield return dialogBox.TypeDialog($"�}���ɂ�������");
        }
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ̓o�c�O����");
        }
        else if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ͂��܂ЂƂ̂悤���c");
        }
    }


    public void HandleUpdate()
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

    //PlayerAction�ł̍s������������
    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {   
            currentAction++;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentAction--;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        //�F�����Ăǂ����I�����Ă邩�킩��悤�ɂ���
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerSkill();
            }
            else if (currentAction == 1)
            {
                //Bag
            }
            else if (currentAction == 2)
            {
                //Pokemon
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                //Run
            }
        }
    }
    void HandleSkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSkill++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSkill--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSkill += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSkill -= 2;
        }

        currentSkill = Mathf.Clamp(currentSkill, 0, playerUnit.Pokemon.Skills.Count - 1);

        //�F�����Ăǂ����I�����Ă邩�킩��悤�ɂ���
        dialogBox.UpdateSkillSelection(currentSkill,playerUnit.Pokemon.Skills[currentSkill]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            //�Z�I����UI��\��
            dialogBox.EnableSkillSelector(false);
            //���b�Z�[�W����
            dialogBox.EnableDialogText(true);
            //�Z����̏���
            StartCoroutine(PerformPlayerSkill());
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            //�Z�I����UI��\��
            dialogBox.EnableSkillSelector(false);
            //���b�Z�[�W����
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }
}
