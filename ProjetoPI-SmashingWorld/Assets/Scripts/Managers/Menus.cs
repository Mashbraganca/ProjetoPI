using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menus : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject SettingsMenu;
    [SerializeField]
    AudioSource SFXManager;
    [SerializeField]
    AudioSource MusicManager;
    [SerializeField]
    Toggle FullScreenToggle;
    [SerializeField]
    Slider MusicVolumeBar;
    [SerializeField]
    Slider SFXVolumeBar;
    [SerializeField]
    TMP_Dropdown ResolutionDropdown;



    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        SetFullScreen(Prefs.LoadFullScreen());
        FullScreenToggle.isOn = Prefs.LoadFullScreen();

        SetScreenResolution(Prefs.LoadScreenResolution());
        ResolutionDropdown.value = Prefs.LoadScreenResolution();


        SetSFXVolume(Prefs.LoadSFXVolume());
        SFXVolumeBar.value = Prefs.LoadSFXVolume();

        SetMusicVolume(Prefs.LoadMusicVolume());
        MusicVolumeBar.value = Prefs.LoadMusicVolume();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openSettings()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void closeSettings()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
        
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    public void SetFullScreen(bool fullScreenValue)
    {
        Prefs.SaveFullScreen();
        Screen.fullScreen = fullScreenValue;
        if (!fullScreenValue)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
        }

    }

    public void SetScreenResolution(int res_type)
    {
        Prefs.SaveScreenResolution(res_type);
        switch(res_type)
        {
            case 0:
                Screen.SetResolution(800, 600, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1680, 1050, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1280, 600, Screen.fullScreen);
                break;
        }
    }

    public void SetSFXVolume(float volume)
    {
        SFXManager.volume = volume;
        Prefs.SaveSFXVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        MusicManager.volume = volume;
        Prefs.SaveMusicVolume(volume);
    }
}
