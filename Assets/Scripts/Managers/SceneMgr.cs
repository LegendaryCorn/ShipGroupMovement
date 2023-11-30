using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static SceneMgr _instance;
    public static SceneMgr Instance { get { return _instance; } }
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



    //*************************************//
    //             FUNCTIONS               //
    //*************************************//
    // Start is called before the first frame update
    void Start()
    {
        PauseMgr.Instance.OnStart();
        ControlMgr.Instance.OnStart();
        UIMgr.Instance.OnStart();
        CameraMgr.Instance.OnStart();
        EntityMgr.Instance.OnStart();
        AIMgr.Instance.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        PauseMgr.Instance.OnUpdate(dt);
        if (!PauseMgr.Instance.gamePaused && dt < 0.1f)
        {
            ControlMgr.Instance.OnUpdate(dt);
            UIMgr.Instance.OnUpdate(dt);
            CameraMgr.Instance.OnUpdate(dt);
            EntityMgr.Instance.OnUpdate(dt);
            AIMgr.Instance.OnUpdate(dt);
        }
    }
}
