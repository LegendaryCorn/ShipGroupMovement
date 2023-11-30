using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static CameraMgr _instance;
    public static CameraMgr Instance { get { return _instance; } }
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

    public Camera mainCamera;
    public Vector3 cameraPosition;
    public Vector3 cameraRotaion;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public void OnStart()
    {

    }

    public void OnUpdate(float dt)
    {
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.eulerAngles = cameraRotaion;
    }
}
