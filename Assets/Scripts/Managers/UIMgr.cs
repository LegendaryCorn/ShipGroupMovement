using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static UIMgr _instance;
    public static UIMgr Instance { get { return _instance; } }
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

    // Main canvas
    public Button pauseButton;

    // Pause Menu
    public GameObject pauseCanvas;
    public Button pResumeButton;
    public Button pMainMenuButton;
    public Button pQuitButton;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public void OnStart()
    {

        pauseButton.onClick.AddListener(PauseMgr.Instance.PauseGame);

        pResumeButton.onClick.AddListener(PauseMgr.Instance.ResumeGame);
        pMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        pQuitButton.onClick.AddListener(Application.Quit);
    }

    public void OnUpdate(float dt)
    {

    }
}
