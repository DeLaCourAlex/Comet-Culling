using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Reference to the pause screen, to disable it
    [SerializeField] GameObject pauseScreen;

    private void Start()
    {
        
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
        DataPermanence.Instance.RestartGame();

        UnpauseGame();
    }

    public void UnpauseGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
