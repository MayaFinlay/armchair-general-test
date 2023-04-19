using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Variables
    public static bool paused = false;
    public bool gameOver = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject GUI;
    public GameObject gameOverMenu;
    public Text scoretxt;
    public Text highscoretxt;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GUI.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Detect if escape key has been pressed
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Function for pausing the game
    public void Pause()
    {
        pauseMenu.SetActive(true);
        GUI.SetActive(false);
        Time.timeScale = 0;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function for resuming the game
    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GUI.SetActive(true);
        Time.timeScale = 1;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Function if the player recieves a game over
    public void GameOver(int score)
    {
        Time.timeScale = 0;
        gameOver = true;
        GUI.SetActive(false);
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Update text on game over screen
        scoretxt.text = "SCORE: " + score;
        highscoretxt.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore", 0);

        // Detect if player got a new highscore
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("Highscore", score);
            highscoretxt.text = "NEW HIGHSCORE!!!";
        }
    }

    // Button function for the player to restart
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Button function for settings
    public void Settings()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    // Button function for going back
    public void Back()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    // Button function for quitting
    public void Quit()
    {
        Application.Quit();
    }
}
