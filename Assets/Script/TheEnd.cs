using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    public GameObject EndingUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            EndingUI.SetActive(true);
        }
    }

    public void RetryButton()
    {
        // Restart level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Make game over menu disappear
        EndingUI.SetActive(false);
    }

    public void QuitButton()
    {
        // Close the game
        Application.Quit();
    }
}
