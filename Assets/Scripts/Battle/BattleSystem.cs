using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //�����X�^�[�̐����ƕ`��
        playerUnit.SetUp();
        enemyUnit.SetUp();
        //HUD�̕`��
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogBox.SetSkillNames(playerUnit.Pokemon.Skills);
        yield return dialogBox.TypeDialog($"�₹���� {enemyUnit.Pokemon.Base.Name} �������ꂽ.");
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("�ǂ�����H"));
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

    //PlayerAction�ł̍s������������
    void HandleActionSelection()
    {
        //������͂����Run,�����͂����Fight�ɂȂ�
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

        //�F�����Ăǂ����I�����Ă邩�킩��悤�ɂ���
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
    }
}
