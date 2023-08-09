using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    public void ResetData() //Player 데이터 삭제
    {
        PlayerPrefs.DeleteAll();
    }
}
