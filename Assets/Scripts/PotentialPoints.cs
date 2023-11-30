using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialPoints : MonoBehaviour
{
    private static PotentialPoints _instance;
    public static PotentialPoints Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            aStarPoints = new List<GameObject>();
            officePoints = new List<GameObject>();
            for (int i = 0; i < aStarObject.transform.childCount; i++)
            {
                aStarPoints.Add(aStarObject.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < officeObject.transform.childCount; i++)
            {
                officePoints.Add(officeObject.transform.GetChild(i).gameObject);
            }
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {

    }

    public GameObject aStarObject;
    public GameObject officeObject;

    public List<GameObject> aStarPoints;
    public List<GameObject> officePoints;
}
