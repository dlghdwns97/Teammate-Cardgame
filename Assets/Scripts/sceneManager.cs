using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneManager : MonoBehaviour
{
    public static bool GamePause = true;

    public GameObject pauseMenuCanvas;
    public GameObject optionMenuCanvas;

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
    }

    public void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
    }

    public void optionMenu()
    {
        optionMenuCanvas.SetActive(true);
    }

    public void optionMenuExit()
    {
        optionMenuCanvas.SetActive(false);
    }

    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void enterStartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
