using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAIPart2 : EntityAI
{
    public List<Vector3> aStarList = new List<Vector3>();

    public override void GoToPoint(float dt)
    {
        if(aStarList.Count == 0 || pointToFollow != commands[0].pointToFollow)
        {
            pointToFollow = commands[0].pointToFollow;
            List<int> l = AIMgr.Instance.AStar(AIMgr.Instance.FindClosestPoint(entity.physics.position), AIMgr.Instance.FindClosestPoint(commands[0].pointToFollow));
            if (l.Contains(-1))
            {
                commands.RemoveAt(0);
                return;
            }
            l = AIMgr.Instance.CrunchPath(l);

            aStarList = new List<Vector3>();
            for(int i = 0; i < l.Count; i++)
            {
                aStarList.Add(AIMgr.Instance.waypoints[l[i]]);
            }
            aStarList.Add(commands[0].pointToFollow);

        }
        if(Vector3.SqrMagnitude(entity.parent.transform.position - aStarList[0]) < 2)
        {
            if(aStarList.Count == 1)
            {
                commands.RemoveAt(0);
            }
            aStarList.RemoveAt(0);
        }
        else
        {
            entity.physics.desiredHeading = Mathf.Atan2(aStarList[0].x - entity.physics.position.x, aStarList[0].z - entity.physics.position.z);
            entity.physics.desiredSpeed = entity.physics.maxSpeed;
        }
    }
}
