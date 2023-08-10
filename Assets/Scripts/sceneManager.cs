using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneManager : MonoBehaviour
{
    public static bool GamePause = true;

    public GameObject pauseMenuCanvas;
    public GameObject optionMenuCanvas;

    public AudioManager audioManager;

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

    public void LevelSelect()
    {
        audioManager.PlaySFX();
        StartCoroutine("LoadSceneWithDelay"); // 버튼 사운드 재생 후 씬 로드
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level1Scene");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level2Scene");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level3Scene");
    }

    public void StartMenu()
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
