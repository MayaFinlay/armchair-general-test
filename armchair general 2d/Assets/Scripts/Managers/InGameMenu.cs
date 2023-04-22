using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject glitch;
    [SerializeField] private GameObject settcan;

    [SerializeField] private AudioSource musicManager;

    public void Pause()
    {
        main.SetActive(true);
        musicManager.Stop();
    }

    public void Continue()
    {
        main.SetActive(false);
        musicManager.Play();
    }



    public void Settings()
    {
        glitch.SetActive(true);
        Invoke("Switch", 1f);
    }

    public void Switch()
    {
        glitch.SetActive(false);
        main.SetActive(false);
        settings.SetActive(true);
        settcan.SetActive(true);
    }

    public void NextLevel()
    {
        glitch.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            ExitToMenu();
        }
    }

    public void RestartLevel()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitToMenu()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }
}
