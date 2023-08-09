using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelBtns; //�ν������� ���� ��ư �Ҵ�

    private void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2); //levelAt : �÷��̾��� �ִ� ���� ����(���� 'LevelSelect' ���� ���� 2)

        for (int i = 0; i < levelBtns.Length; i++)
        {
            if(i + 2 > levelAt) // levelAt ������ ���� �ε����� �ش��ϴ� ��ư ��Ȱ��ȭ(levelBtns[1],levelBtns[2])
            {
                levelBtns[i].interactable = false;
            }
        }
    }
}
