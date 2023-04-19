using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SettingsManager : MonoBehaviour
{
    // Variables
    public GameObject graphicsTab;
    public GameObject audioTab;
    public GameObject controlTab;
    public GameObject gameplayTab;
    public GameObject icons;
    public GameObject menuBackground;
    public GameObject canvas;
    public GameObject settingsBackground;
    public GameObject glitch;
    public AudioMixer audioMix;
    //public AudioMixer audioMixer;
    public Dropdown resolutionsDD;
    public Camera cam;
    Resolution[] resolutions;



    // Start is called before the first frame update
    void Start()
    {

        // Get resolutions for the dropdown menu
        resolutions = Screen.resolutions;
        resolutionsDD.ClearOptions();

        // Create a new list for the resolution setting
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        // For loop that adds all the resolution sizes to the list
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDD.AddOptions(options);
        resolutionsDD.value = currentResolutionIndex;
        resolutionsDD.RefreshShownValue();
    }

    // Function for when you first open settings
    private void OnEnable()
    {
        audioTab.SetActive(true);
        controlTab.SetActive(false);
        graphicsTab.SetActive(false);
        gameplayTab.SetActive(false);
        icons.SetActive(true);
    }


    // Button function to switch to the graphics tab
    public void Graphics()
    {
        graphicsTab.SetActive(true);
        audioTab.SetActive(false);
        controlTab.SetActive(false);
        gameplayTab.SetActive(false);
    }

    // Button function to switch to the audio tab
    public void Audio()
    {
        graphicsTab.SetActive(false);
        audioTab.SetActive(true);
        controlTab.SetActive(false);
        gameplayTab.SetActive(false);
    }

    // Button function to switch to the controls tab
    public void Controls()
    {
        graphicsTab.SetActive(false);
        audioTab.SetActive(false);
        controlTab.SetActive(true);
        gameplayTab.SetActive(false);
    }

    // Button function to switch to the controls tab
    public void Camera()
    {
        graphicsTab.SetActive(false);
        audioTab.SetActive(false);
        controlTab.SetActive(false);
        gameplayTab.SetActive(false);
    }

    // Button function to switch to the controls tab
    public void Gameplay()
    {
        graphicsTab.SetActive(false);
        audioTab.SetActive(false);
        controlTab.SetActive(false);
        gameplayTab.SetActive(true);
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


    public void SetVolumeMaster (float volume)
    {
        audioMix.SetFloat("Master", Mathf.Log10(volume) * 20);
        print(volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audioMix.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeMusic(float volume)
    {
        audioMix.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeVoice(float volume)
    {
        audioMix.SetFloat("Voice", Mathf.Log10(volume) * 20);
    }

    // Dropdown function for the quality of the game
    public void SetQuality (int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // Toggle function for setting the game to fullscreen
    public void SetFullscreen (bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    // Function that sets the resolution to the chosen one from the dropdown menu
    public void SetResolution (int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //// Slider function that handles the sensitivity
    //public void SetSensitivity(float volume)
    //{
    //    cam.GetComponent<CameraMovement>().sensitivity = volume;
    //}

    //// Slider function that handles the FOV
    //public void SetFOV (float fov)
    //{
    //    cam.fieldOfView = fov;
    //}
}
