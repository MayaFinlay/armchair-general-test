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
    public GameObject levelSelect;
    public GameObject lsCan;

    [Header("Audio")]
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip settingsClip;
    static bool firstTime = true;


    // Start is called before the first frame update
    void Start()
    {
        main.SetActive(true);
        settings.SetActive(false);
        settcan.SetActive(false);

        levelSelect.SetActive(false);
        lsCan.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(StartNarrative());

    }

    private IEnumerator StartNarrative()
    {
        if (firstTime)
        {
            firstTime = false;
            yield return new WaitForSecondsRealtime(1f);
            voiceSource.clip = startClip;
            voiceSource.Play();

            yield return new WaitUntil(() => voiceSource.isPlaying == false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    public void Settings()
    {
        glitch.SetActive(true);
        Invoke("SwitchSettings", 1f);
        
    }

    public void LevelSelect()
    {
        glitch.SetActive(true);
        Invoke("SwitchLevelSelect", 1f);
    }
    public void SwitchSettings()
    {
        glitch.SetActive(false);
        main.SetActive(false);
        gameObject.SetActive(false);
        settings.SetActive(true);
        settcan.SetActive(true);


        voiceSource.clip = settingsClip;
        voiceSource.Play();
    }

    public void SwitchLevelSelect()
    {
        glitch.SetActive(false);
        main.SetActive(false);
        gameObject.SetActive(false);
        levelSelect.SetActive(true);
        lsCan.SetActive(true);
    }

    public void StartGame()
    {
        glitch.SetActive(true);
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
