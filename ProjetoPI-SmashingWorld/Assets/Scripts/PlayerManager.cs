using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    
    PlayerInputManager manager;
    bool dualcontroller = false;
    [SerializeField]
    GameObject controllerCanvas;
    GameObject prefabp1,prefabp2;
    [SerializeField]
    GameObject mainHUD;
    [SerializeField]
    GameObject ScenarioObjects;
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject GameOverMenu;
    [SerializeField]
    GameObject GameOverMenuText;
    [SerializeField]
    GameObject SpawnPointp1, SpawnPointp2;
    [SerializeField]
    CameraAdjust cameraAdjust;
    [SerializeField]
    AudioClip battlemusic;

    bool paused = false;

    // Salvando referencia de p1,p2 caso mudanaçs sejam necessárias
    GameObject p1, p2;
    PlayerController controllerp1, controllerp2;



    void Awake()
    {
        //TODO: Separar prefabs de p1 e p2 e alterar antes de chamar JoinPlayer()
        manager = GetComponent<PlayerInputManager>();
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>().clip = battlemusic;
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>().Play();

    }



    public void SetDualController()
    {
        dualcontroller = true;
        gameObject.SetActive(true);
        controllerCanvas.SetActive(false);
        mainHUD.SetActive(true);
        ScenarioObjects.SetActive(true);
        SpawnPlayers();

    }

    public void SetKeyboardController()
    {
        dualcontroller = false;
        gameObject.SetActive(true);
        controllerCanvas.SetActive(false);
        mainHUD.SetActive(true);
        ScenarioObjects.SetActive(true);
        SpawnPlayers();

    }

    private void SpawnPlayers()
    {
        if (dualcontroller)
        {
            p1 = manager.JoinPlayer(0, 0, "Controller").gameObject;
            SetupP1();
        }
        else
        {
            p1 = manager.JoinPlayer(0, 0, "Keyboard").gameObject;
            SetupP1();
        }

        //Spawning P2 - always joystick
        p2 = manager.JoinPlayer(1, 0, "Controller").gameObject;
        SetupP2();

        cameraAdjust.SetupCamera();
    }


        //Settings P1 control
    public void FinishGame(int player)
    {
        if(player==1)  GameOverMenuText.GetComponent<TextMeshProUGUI>().text = "Player 2 Wins";
        else GameOverMenuText.GetComponent<TextMeshProUGUI>().text = "Player 1 Wins";
        ScenarioObjects.SetActive(false);
        GameOverMenu.SetActive(true);
        Destroy(p1);
        Destroy(p2);

         //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if(!paused)
        {
            paused = true;
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else
        {
            paused = false;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }

    }



 

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void SetupP1()
    {
        controllerp1 = p1.GetComponent<PlayerController>();
        controllerp1.HUD = GameObject.FindGameObjectWithTag("P1HUD").GetComponent<HUDBars>();
        controllerp1.playerid = 1;
        controllerp1.registerDeathEvent(FinishGame);
        controllerp1.GetComponent<PlayerController>().registerPauseEvent(PauseGame);
        controllerp1.SpawnPoint = SpawnPointp1;
        p1.transform.position = SpawnPointp1.transform.position;
        p1.tag = "Player1";
    }

    private void SetupP2()
    {
        controllerp2 = p2.GetComponent<PlayerController>();
        controllerp2.HUD = GameObject.FindGameObjectWithTag("P2HUD").GetComponent<HUDBars>();
        controllerp2.playerid = 2;
        controllerp2.registerDeathEvent(FinishGame);
        controllerp2.GetComponent<PlayerController>().registerPauseEvent(PauseGame);
        controllerp2.SpawnPoint = SpawnPointp2;
        p2.transform.position = SpawnPointp2.transform.position;
        p2.tag = "Player2";
    }

}
