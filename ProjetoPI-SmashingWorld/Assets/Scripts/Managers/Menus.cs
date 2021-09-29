using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject SettingsMenu;
    


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);


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
        print(fullScreenValue);
        Screen.fullScreen = fullScreenValue;
        if (!fullScreenValue)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
        }

    }
}
