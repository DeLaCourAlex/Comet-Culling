// All functionality for moving between screens and selecting options in the menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Reference to the pause screen, to disable it when moving from pause back to main menu
    [SerializeField] GameObject pauseScreen;

    // Move to the next scene
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Restart the game
    public void RestartGame()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
        DataPermanence.Instance.RestartGame();

        UnpauseGame();
    }

    // Unpause the game
    public void UnpauseGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    // Toggle the tutorial
    public void SetTutorial(bool tutorial)
    {
        if(tutorial)
            DataPermanence.Instance.tutorialNumber = 0;

        else if (!tutorial)
        {
            DataPermanence.Instance.tutorialNumber = 12;
            DataPermanence.Instance.spaceshipEnergy = 100;
            
            DataPermanence.Instance.seedB = 10;
            DataPermanence.Instance.seedA = 10;
        }

        DataPermanence.Instance.playerTutorial = tutorial;
    }

    // Alter the SFX and music volume
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
