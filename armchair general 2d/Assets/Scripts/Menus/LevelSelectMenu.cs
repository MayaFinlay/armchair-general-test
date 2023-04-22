using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{

    public GameObject icons;
    public GameObject menuBackground;
    public GameObject canvas;
    public GameObject settingsBackground;
    public GameObject glitch;

    private void OnEnable()
    {
        icons.SetActive(true);
    }

    public void BackToMenu()
    {
        glitch.SetActive(true);
        Invoke("GlitchOff", 1f);
    }

    public void GlitchOff()
    {
        glitch.SetActive(false);
        settingsBackground.SetActive(false);
        menuBackground.SetActive(true);
        canvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Tutorial()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("Tutorial");
    }

    public void Level1()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("Level2");
    }

    public void Level3()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("Level3");
    }
}
