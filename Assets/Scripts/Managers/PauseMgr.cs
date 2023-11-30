using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static PauseMgr _instance;
    public static PauseMgr Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    //*************************************//
    //             VARIABLES               //
    //*************************************//

    public bool gamePaused;
    public bool doublePause;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public void OnStart()
    {

    }

    public void OnUpdate(float dt)
    {
        doublePause = false;
        if (gamePaused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();

        }
    }

    public void PauseGame()
    {
        if (!doublePause)
        {
            gamePaused = true;
            UIMgr.Instance.pauseCanvas.SetActive(true);
            doublePause = true;
        }
    }

    public void ResumeGame()
    {
        if (!doublePause)
        {
            gamePaused = false;
            UIMgr.Instance.pauseCanvas.SetActive(false);
            doublePause = true;
        }
    }
}
