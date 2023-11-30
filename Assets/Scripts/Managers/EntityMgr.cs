using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static EntityMgr _instance;
    public static EntityMgr Instance { get { return _instance; } }
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

    public List<BoatEntity> selectedEntities = new List<BoatEntity>();

    // List of entities
    public List<BoatEntity> boatEntities;
    public List<EntityWall> wallEntities;

    // Walls to copy
    public List<GameObject> copyObjects;
    List<int> intToCopy = new List<int> { 20, 30, 100, 1 };
    List<int> objectToCopy = new List<int> { 0, 0, 0, 2, 1, 1, 1, 3};

    // Start/End points
    public GameObject startPos;
    public GameObject endPos;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public void OnStart()
    {
        CreateWalls();
        RelocateShips();
    }

    public void OnUpdate(float dt)
    {
        // Update robots
        for(int i = 0; i < boatEntities.Count; i++)
        {
            boatEntities[i].OnUpdate(dt);
        }
    }

    void CreateWalls() 
    {
        int i = intToCopy[PermData.Instance.env % 4];
        int o = objectToCopy[PermData.Instance.env];
        GameObject obj = copyObjects[o];
        wallEntities = new List<EntityWall>();

        if (i == 1)
        {
            GameObject copy = Instantiate(obj);
            copy.transform.position = Vector3.zero;

            if(PermData.Instance.env == 3)
            {
                startPos.transform.position = new Vector3(-25, 0, 0);
                endPos.transform.position = new Vector3(25, 0, 0);
            }

            if(PermData.Instance.env == 7)
            {
                startPos.transform.position = new Vector3(-100, 0, 5);
                endPos.transform.position = new Vector3(-75, 0, 5);
            }
        }
        else
        {
            for(int j = 0; j < i; j++)
            {
                GameObject copy = Instantiate(obj);
                EntityWall wall = copy.GetComponent<EntityWall>();
                float a = 80.0f;
                float b = 80.0f;
                while((a > 60.0f || a < -60.0f) && (b > 60.0f || b < -60.0f))
                {
                    a = Random.Range(-100.0f, 100.0f);
                    b = Random.Range(-100.0f, 100.0f);
                }
                wall.position = new Vector3(a, 0, b);
                wall.xScale = Random.Range(1.0f, 1.5f);
                if (o == 1)
                {
                    wall.zScale = Random.Range(1.0f, 1.5f);
                    wall.thisShape = EntityWall.Shape.Square;
                }
                else
                {
                    wall.zScale = wall.xScale;
                    wall.thisShape = EntityWall.Shape.Circle;
                }

                wall.OnStart();
                wallEntities.Add(wall);
            }
        }
    }

    void RelocateShips()
    {
        for(int i = 0; i < boatEntities.Count; i++)
        {
            boatEntities[i].physics.position.x = Random.Range(startPos.transform.position.x - 10.0f, startPos.transform.position.x + 10.0f);
            boatEntities[i].physics.position.z = Random.Range(startPos.transform.position.z - 10.0f, startPos.transform.position.z + 10.0f);
            boatEntities[i].physics.heading = Random.Range(0.0f, Mathf.PI * 2);
            boatEntities[i].OnStart();
        }
    }
}
