using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    
    PlayerInputManager manager;
    bool dualcontroller = false;
    [SerializeField]
    GameObject controllerCanvas;
    GameObject prefabp1,prefabp2;

    // Salvando referencia de p1,p2 caso mudanaçs sejam necessárias
    GameObject p1, p2;



    void Start()
    {
        //TODO: Separar prefabs de p1 e p2 e alterar antes de chamar JoinPlayer()
        manager = GetComponent<PlayerInputManager>();
        
        //Settings P1 control
        if (dualcontroller)
        {
            p1 = manager.JoinPlayer(0, 0, "Controller").gameObject;
        }
        else
        {
            p1 = manager.JoinPlayer(0, 0, "Keyboard").gameObject;
        }

        //Spawning P2 - always joystick
        p2 = manager.JoinPlayer(1, 0, "Controller").gameObject;

    }

    public void SetDualController()
    {
        dualcontroller = true;
        gameObject.SetActive(true);
        controllerCanvas.SetActive(false);
    }

    public void SetKeyboardController()
    {
        dualcontroller = false;
        gameObject.SetActive(true);
        controllerCanvas.SetActive(false);
    }
}
