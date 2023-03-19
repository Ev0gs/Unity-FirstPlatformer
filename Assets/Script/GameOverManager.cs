using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public void OnPlayerDeath()
    {
        gameOverUI.SetActive(true);
    }

    public void RetryButton()
    {
        // Restart level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Make game over menu disappear
        gameOverUI.SetActive(false);
    }

    public void QuitButton()
    {
        // Close the game
        Application.Quit();
    }
}
