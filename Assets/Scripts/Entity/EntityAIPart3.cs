using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAIPart3 : EntityAI
{
    public class PotentialField
    {
        public PotentialField(GameObject o, Vector3 p, float c, float e, bool r)
        {
            objField = o;
            pointField = p;
            con = c;
            exp = e;
            repulsive = r;
        }

        public Vector3 Calculate(Vector3 pos)
        {
            Vector3 secondPos = objField != null ? objField.transform.position : pointField;
            float pot = con * Mathf.Pow(Vector3.Distance(pos, secondPos), exp);
            Vector3 force = pot * Vector3.Normalize(secondPos - pos) * (repulsive ? -1 : 1);
            return force;
        }

        GameObject objField; // Points to Game Object
        public Vector3 pointField; // Points to static point
        float con; // constant
        float exp; // exponent
        bool repulsive;
    };

    public List<PotentialField> repulsiveFields;
    public PotentialField currTarget;

    public override void OnStart()
    {
        base.OnStart();
        StartFields();
    }

    public void StartFields()
    {
        repulsiveFields = new List<PotentialField>();
        currTarget = new PotentialField(null, Vector3.zero, 0, 1, false);
        List<GameObject> setList = null;

        // Add walls
        for(int i = 0; i < EntityMgr.Instance.wallEntities.Count; i++)
        {
            EntityWall w = EntityMgr.Instance.wallEntities[i];
            float mult = 1500.0f;
            mult *= w.xScale == w.zScale ? 1.0f : 1.0f;
            repulsiveFields.Add(new PotentialField(w.gameObject, Vector3.zero, mult * Mathf.Max(w.xScale, w.zScale), -3, true));
        }

        // Check if environment needs special points
        if(PermData.Instance.env == 3)
        {
            setList = PotentialPoints.Instance.aStarPoints;
        }
        if(PermData.Instance.env == 7)
        {
            setList = PotentialPoints.Instance.officePoints;
        }
        if(setList != null)
        {
            for (int i = 0; i < setList.Count; i++)
            {
                repulsiveFields.Add(new PotentialField(null, setList[i].transform.position, 700, -3, true));
            }
        }

        // Add boats
        for (int i = 0; i < EntityMgr.Instance.boatEntities.Count; i++)
        {
            GameObject boat = EntityMgr.Instance.boatEntities[i].gameObject;
            if (boat != gameObject)
            {
                repulsiveFields.Add(new PotentialField(boat, Vector3.zero, 20, -3, true));
            }
        }
    }

    public override void GoToPoint(float dt)
    {
        if (Vector3.SqrMagnitude(entity.parent.transform.position - commands[0].pointToFollow) < 25)
        {
            commands.RemoveAt(0);
            return;
        }
        if(currTarget.pointField != commands[0].pointToFollow)
        {
            currTarget = new PotentialField(null, commands[0].pointToFollow, 20, -1f, false);
        }

        Vector3 fullField = currTarget.Calculate(entity.physics.position);

        for(int i = 0; i < repulsiveFields.Count; i++)
        {
            fullField = fullField + repulsiveFields[i].Calculate(entity.physics.position);
        }

        entity.physics.desiredHeading = Mathf.Atan2(fullField.x, fullField.z);
        if (entity.physics.desiredHeading < 0) { entity.physics.desiredHeading += Mathf.PI * 2; }
        entity.physics.desiredSpeed = entity.physics.maxSpeed * (Mathf.Cos(Utils.AngleBetween(entity.physics.desiredHeading, entity.physics.heading)) + 1.0f) / 2.0f;
    }
}
