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

    public void SetTutorial(bool tutorial)
    {
        if(tutorial)
            DataPermanence.Instance.tutorialNumber = 0;

        else if (!tutorial)
        {
            DataPermanence.Instance.tutorialNumber = 9;
            DataPermanence.Instance.spaceshipEnergy = 100;
        }
            

        DataPermanence.Instance.playerTutorial = tutorial;
    }

    public void SetSFXVolume(float volume)
    {
        DataPermanence.Instance.sfxVolume = volume;
        AudioManager.Instance.sfxVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        DataPermanence.Instance.musicVolume = volume;
        AudioManager.Instance.sfxVolume = volume;
    }
}
