using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    public int nextSceneLoad;

    private void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1; //������� ������

    }

    public void NextScene() //endCanvas�� "��������" ��ư ������ �� ȣ��
    {
       
        SceneManager.LoadScene(nextSceneLoad);

        if(nextSceneLoad > PlayerPrefs.GetInt("levelAt")) //�����Ȳ ������ ���� levelAt �� ������Ʈ
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }

    }
}
