using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelBtns; //인스펙터의 레벨 버튼 할당

    private void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2); //levelAt : 플레이어의 최대 진행 레벨(현재 'LevelSelect' 씬의 값은 2)

        for (int i = 0; i < levelBtns.Length; i++)
        {
            if(i + 2 > levelAt) // levelAt 값보다 높은 인덱스에 해당하는 버튼 비활성화(levelBtns[1],levelBtns[2])
            {
                levelBtns[i].interactable = false;
            }
        }
    }
}
