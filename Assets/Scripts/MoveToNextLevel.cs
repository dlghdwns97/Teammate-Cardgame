using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    public int nextSceneLoad;

    private void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1; //현재씬의 다음씬

    }

    public void NextScene() //endCanvas의 "다음으로" 버튼 눌렀을 때 호출
    {
       
        SceneManager.LoadScene(nextSceneLoad);

        if(nextSceneLoad > PlayerPrefs.GetInt("levelAt")) //진행상황 저장을 위해 levelAt 값 업데이트
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }

    }
}
