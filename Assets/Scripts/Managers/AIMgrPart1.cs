using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIMgrPart1 : AIMgr
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static AIMgrPart1 _instance;
    public AIMgrPart1 Instance { get { return _instance; } }
    protected override void Awake()
    {
        base.Awake();
        if (_instance == null && _instance == this)
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

    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject copyWaypoint;
    public List<GameObject> wayPoints;
    public List<GameObject> lines;
    public Material linemat;

    public GameObject officeObject;
    public List<Vector3> officePoints;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public override void OnStart()
    {
        base.OnStart();

        officePoints = new List<Vector3>();
        for(int i = 0; i < officeObject.transform.childCount; i++) 
        {
            officePoints.Add(officeObject.transform.GetChild(i).position);
        }

        if(PermData.Instance.env == 7)
        {
            RunOffice();
        }
    }

    public override void LeftClick(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && PermData.Instance.env != 7)
        {
            RaycastHit[] hits;
            Ray ray = CameraMgr.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);
            bool w = false; // If wall is hit (dont do anything)
            int f = -1; // If floor is hit

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.CompareTag("Floor"))
                {
                    f = i;
                }
                w = w || hits[i].transform.CompareTag("Wall");
            }

            startPoint.SetActive(false);
            DestroyDrawings();

            if (f > -1 && !w)
            {
                startPoint.SetActive(true);
                startPoint.transform.position = new Vector3(hits[f].point.x, 0, hits[f].point.z);
            }

            if(startPoint.activeSelf && endPoint.activeSelf)
            {
                GeneratePath();
            }
        }
    }

    public override void RightClick(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject() && PermData.Instance.env != 7)
        {
            RaycastHit[] hits;
            Ray ray = CameraMgr.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);
            bool w = false; // If wall is hit (dont do anything)
            int f = -1; // If floor is hit

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.CompareTag("Floor"))
                {
                    f = i;
                }
                w = w || hits[i].transform.CompareTag("Wall");
            }

            endPoint.SetActive(false);
            DestroyDrawings();

            if (f > -1 && !w)
            {
                endPoint.SetActive(true);
                endPoint.transform.position = new Vector3(hits[f].point.x, 0, hits[f].point.z);
            }

            if (startPoint.activeSelf && endPoint.activeSelf)
            {
                GeneratePath();
            }
        }
    }

    public void GeneratePath()
    {
        // Calculations
        List<int> l = AStar(FindClosestPoint(startPoint.transform.position), FindClosestPoint(endPoint.transform.position));
        if (l.Contains(-1)){ return; }
        l = CrunchPath(l);

        // Draw
        lines.Add(RenderLine(startPoint.transform.position, waypoints[l[0]]));
        lines.Add(RenderLine(endPoint.transform.position, waypoints[l[l.Count - 1]]));
        for (int i = 0; i < l.Count; i++)
        {
            GameObject way = Instantiate(copyWaypoint);
            way.transform.position = waypoints[l[i]];
            wayPoints.Add(way);
            if(i != 0)
            {
                lines.Add(RenderLine(waypoints[l[i - 1]], waypoints[l[i]]));
            }
        }
    }

    public void DestroyDrawings()
    {
        // Reset Drawings
        for (int i = 0; i < wayPoints.Count; i++)
        {
            Destroy(wayPoints[i]);
        }
        wayPoints = new List<GameObject>();
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i]);
        }
        lines = new List<GameObject>();
    }

    public GameObject RenderLine(Vector3 p1, Vector3 p2)
    {
        GameObject line = new GameObject();
        LineRenderer l = line.AddComponent(typeof(LineRenderer)) as LineRenderer;
        Vector3[] v = { p1 + 0.5f * Vector3.up, p2 + 0.5f * Vector3.up };
        l.positionCount = 2;
        l.SetPositions(v);
        l.material = linemat;
        return line;
    }

    public void RunOffice()
    {
        for(int i = 0; i < officePoints.Count - 1; i++)
        {
            List<int> l = AStar(FindClosestPoint(officePoints[i]), FindClosestPoint(officePoints[i+1]));
            if (l.Contains(-1)) { return; }
            l = CrunchPath(l);

            for (int j = 0; j < l.Count; j++)
            {
                GameObject way = Instantiate(copyWaypoint);
                way.transform.position = waypoints[l[j]];
                wayPoints.Add(way);
                if (j != 0)
                {
                    lines.Add(RenderLine(waypoints[l[j - 1]], waypoints[l[j]]));
                }
            }
        }
    }
}
