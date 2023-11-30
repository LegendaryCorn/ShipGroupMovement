using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static MenuMgr _instance;
    public static MenuMgr Instance { get { return _instance; } }
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

    public Button gameButton;
    public Button quitButton;

    public GameObject instructionsPanel;
    public Button startButton;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//
    void Start()
    {
        gameButton.onClick.AddListener(PullUpInstructions);
        quitButton.onClick.AddListener(Application.Quit);
        startButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }


    void Update()
    {
        
    }

    public void PullUpInstructions()
    {
        instructionsPanel.SetActive(true);
    }
}
