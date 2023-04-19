using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Camera cam;
    public GameObject main;
    public GameObject settings;
    public GameObject settcan;
    public GameObject glitch;

    // Start is called before the first frame update
    void Start()
    {
        main.SetActive(true);
        settings.SetActive(false);
        settcan.SetActive(false);

    }

    // Update is called once per frame
    public void Settings()
    {
        glitch.SetActive(true);
        Invoke("Switch", 1f);
        
    }

    public void Switch()
    {
        glitch.SetActive(false);
        main.SetActive(false);
        gameObject.SetActive(false);
        settings.SetActive(true);
        settcan.SetActive(true);
    }

    public void StartGame()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
