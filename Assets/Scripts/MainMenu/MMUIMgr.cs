using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MMUIMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static MMUIMgr _instance;
    public static MMUIMgr Instance { get { return _instance; } }
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

    // Main Panel
    public Button playButton;
    public Button quitButton;

    // Selection Panel
    public GameObject selectionPanel;
    public Button startButton;
    public Button backButton;
    public List<Button> partButtons;
    public List<Button> envButtons;


    //*************************************//
    //             FUNCTIONS               //
    //*************************************//
    
    void Start()
    {
        playButton.onClick.AddListener(() => SetSelectionActive(true));
        quitButton.onClick.AddListener(Application.Quit);

        startButton.onClick.AddListener(StartGame);
        backButton.onClick.AddListener(() => SetSelectionActive(false));

        partButtons[0].onClick.AddListener(() => OnPartClick(0));
        partButtons[1].onClick.AddListener(() => OnPartClick(1));
        partButtons[2].onClick.AddListener(() => OnPartClick(2));
        partButtons[3].onClick.AddListener(() => OnPartClick(3));

        envButtons[0].onClick.AddListener(() => OnEnvClick(0));
        envButtons[1].onClick.AddListener(() => OnEnvClick(1));
        envButtons[2].onClick.AddListener(() => OnEnvClick(2));
        envButtons[3].onClick.AddListener(() => OnEnvClick(3));
        envButtons[4].onClick.AddListener(() => OnEnvClick(4));
        envButtons[5].onClick.AddListener(() => OnEnvClick(5));
        envButtons[6].onClick.AddListener(() => OnEnvClick(6));
        envButtons[7].onClick.AddListener(() => OnEnvClick(7));
    }

    void Update()
    {
        
    }

    public void SetSelectionActive(bool b)
    {
        selectionPanel.SetActive(b);
        OnPartClick(0);
        OnEnvClick(0);
    }

    public void OnPartClick(int b)
    {
        partButtons[PermData.Instance.part].interactable = true;
        PermData.Instance.part = b;
        partButtons[b].interactable = false;
    }

    public void OnEnvClick(int b)
    {
        envButtons[PermData.Instance.env].interactable = true;
        PermData.Instance.env = b;
        envButtons[b].interactable = false;
    }

    public void StartGame()
    {
        Debug.Log(PermData.Instance.part.ToString() + "," + PermData.Instance.env.ToString());
        SceneManager.LoadScene(PermData.Instance.part + 1);
    }
}
