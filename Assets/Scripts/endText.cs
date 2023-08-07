using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class endText : MonoBehaviour
{
    public void retryGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void endGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
