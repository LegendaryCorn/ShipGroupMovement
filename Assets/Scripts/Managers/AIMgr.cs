using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static AIMgr _instance;
    public static AIMgr Instance { get { return _instance; } }
    protected virtual void Awake()
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

    // TODO placeholder values
    public List<Vector3> waypoints;

    //public float[,] waypointConnections;
    public List<Dictionary<int, float>> waypointConnections;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public virtual void OnStart()
    {
        SetUpPhysics();
        GeneratePoints(130.0f, 50);
        GenerateConnections(50);
    }

    public void OnUpdate(float dt)
    {
        LeftClick(dt);
        RightClick(dt);
    }
    
    public virtual void LeftClick(float dt)
    {

    }

    public virtual void RightClick(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && EntityMgr.Instance.selectedEntities.Count > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit[] hits;
            Ray ray = CameraMgr.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);
            int f = -1; // If floor is hit
            bool w = false; // If wall is hit (dont do anything)

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.gameObject.CompareTag("Floor"))
                {
                    f = i;
                }
                w = w || hits[i].transform.CompareTag("Wall");
            }

            // End for loop

            if (f >= 0 && !w)
            {
                Vector3 p = hits[f].point;
                p.y = 0;
                AddCommand(new EntityAI.CommandType(EntityAI.Command.GoToPoint, null, p));
            }
            else
            {
                AddCommand(null);
            }
        }
    }

    public void AddCommand(EntityAI.CommandType c)
    {
        for(int i = 0; i < EntityMgr.Instance.selectedEntities.Count; i++)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                EntityMgr.Instance.selectedEntities[i].ai.commands.Clear();
            }
            if (c != null)
            {
                EntityMgr.Instance.selectedEntities[i].ai.commands.Add(c);
            }
        }
    }

    public List<int> AStar(int startPoint, int endPoint)
    {
        List<List<int>> pathToList = new List<List<int>>();
        List<int> visitedPoints = new List<int>();
        for (int i = 0; i < waypoints.Count; i++) // List of {-1} represents that the point hasn't been reached
        {
            pathToList.Add(new List<int> { -1 });
        }

        pathToList[startPoint] = new List<int> { startPoint };
        visitedPoints.Add(startPoint);
        while (!visitedPoints.Contains(endPoint))
        {
            List<int> currLowestList = null;
            float currLowestSum = float.MaxValue;

            foreach(int a in visitedPoints)
            {
                if (!pathToList[a].Contains(-1))
                { // The first point is visited
                    foreach(int b in waypointConnections[a].Keys)
                    {
                        if (pathToList[b].Contains(-1))
                        { // The second point is unvisited, the path exists

                            // Calculate
                            List<int> l = new List<int>(pathToList[a]);
                            l.Add(b);

                            float x = GetPathLength(l) + GetHeuristic(b, endPoint);
                            if(x < currLowestSum)
                            {
                                currLowestSum = x;
                                currLowestList = l;
                            }
                        }
                    }
                }
            }
            if (currLowestSum == float.MaxValue) { return new List<int> { -1 }; }
            int newPoint = currLowestList[currLowestList.Count - 1];
            pathToList[newPoint] = new List<int>(currLowestList);
            visitedPoints.Add(newPoint);
        }

        
        return pathToList[endPoint];
    }

    public float GetPathLength(List<int> path)
    {
        float total = 0;

        for(int i = 0; i < path.Count - 1; i++)
        {
            total += waypointConnections[path[i]][path[i+1]];
        }

        return total;
    }

    public float GetHeuristic(int p, int e)
    {
        return Vector3.Distance(waypoints[p], waypoints[e]);
    }


    //
    //
    //

    public void GeneratePoints (float bounds, int points)
    {
        waypoints = new List<Vector3>();
        for(int i = 0; i < points; i++)
        {
            for(int j = 0; j < points; j++)
            {
                float a = (i * (2 * bounds / (points - 1))) - bounds;
                float b = (j * (2 * bounds / (points - 1))) - bounds;
                Vector3 newPoint = new Vector3(a, 0, b);

                waypoints.Add(newPoint);
            }
        }
    }

    public void GenerateConnections(int points)
    {
        waypointConnections = new List<Dictionary<int, float>>();
        for(int i = 0; i < waypoints.Count; i++)
        {
            waypointConnections.Add(new Dictionary<int, float>());
        }

        float singleConnection = Vector3.Distance(waypoints[0], waypoints[1]);
        float diagonalConnection = Vector3.Distance(waypoints[0], waypoints[points + 1]);

        for(int i = 0; i < waypoints.Count; i++)
        {
            int x = i % points;
            int y = i / points;
            for (int a = -1; a <= 1; a++)
            {
                if (waypoints[i].y < 1000)
                {
                    for (int b = -1; b <= 1; b++)
                    {
                        int secondPoint = x + a + points * (y + b);
                        if ((a == 0 && b == 0) || x + a >= points || x + a < 0 || y + b >= points || y + b < 0) // invalid
                        {

                        }
                        else if ((a == 0 || b == 0) && CheckPath(waypoints[i], waypoints[secondPoint], singleConnection)) // single
                        {
                            waypointConnections[i].Add(secondPoint, singleConnection);
                        }
                        else if (CheckPath(waypoints[i], waypoints[secondPoint], diagonalConnection)) // diagonal
                        {
                            waypointConnections[i].Add(secondPoint, diagonalConnection);
                        }
                    }
                }
            }
        }
    }

    public bool CheckPath(Vector3 p1, Vector3 p2, float dist)
    {
        RaycastHit[] hits = Physics.RaycastAll(p1, p2 - p1, dist);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }

    public List<int> CrunchPath(List<int> l)
    {
        List<int> crunchedPath = new List<int>();

        crunchedPath.Add(l[0]);

        for(int i = 0; i < l.Count; i++)
        {
            if(i == l.Count - 1)
            {
                crunchedPath.Add(l[i]);
            }
            else if(!CheckPath(waypoints[crunchedPath[crunchedPath.Count - 1]], waypoints[l[i]], Vector3.Distance(waypoints[crunchedPath[crunchedPath.Count - 1]], waypoints[l[i]])))
            {
                crunchedPath.Add(l[i - 1]);
            }
        }

        return crunchedPath;
    }

    public void SetUpPhysics()
    {
        Physics.autoSimulation = false; // Unity Raycast is really weird
        Physics.Simulate(0.01f);
        Physics.autoSimulation = true;
    }

    public int FindClosestPoint(Vector3 loc)
    {
        int closest = -1;
        float closestdist = Mathf.Infinity;

        for(int i = 0; i < waypoints.Count; i++)
        {
            float dist = Vector3.Distance(loc, waypoints[i]);
            if(dist < closestdist && CheckPath(loc, waypoints[i], dist))
            {
                closest = i;
                closestdist = dist;
            }
        }

        return closest;
    }
}
