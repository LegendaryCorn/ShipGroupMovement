using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermData : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static PermData _instance;
    public static PermData Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    //*************************************//
    //             VARIABLES               //
    //*************************************//

    public int part;
    public int env;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//
}
