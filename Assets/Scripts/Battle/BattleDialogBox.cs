using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    //dialog��Text���擾���āA�ύX����
    [SerializeField] int letterPerSecond;  //1����������̎���
    [SerializeField] Text dialogText;

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
    }
}
