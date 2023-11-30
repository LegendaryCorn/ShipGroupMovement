using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPhysics : MonoBehaviour
{
    public BoatEntity entity;

    public Vector3 position;
    public Vector3 velocity;

    public float speed;
    public float desiredSpeed;
    public float heading;
    public float desiredHeading;
    public float acceleration;
    public float turnRate;
    public float minSpeed;
    public float maxSpeed;

    public void OnStart()
    {
        
    }

    public void OnUpdate(float dt)
    {
        OrientedPhysicsUpdate(dt);

        position += dt * velocity;
        entity.parent.transform.position = position;
        entity.parent.transform.eulerAngles = new Vector3(0, heading * 180.0f / 3.141592f, 0);
    }

    void OrientedPhysicsUpdate(float dt)
    {
        // Update heading
        if (heading < 0)
        {
            heading += Mathf.PI * 2;
        }
        if(Utils.ApproximatelyEqual(desiredHeading, heading))
        {
            
        }
        else if(Utils.AngleBetween(desiredHeading, heading) > 0)
        {
            heading += turnRate * dt;
        }
        else if (Utils.AngleBetween(desiredHeading, heading) < 0)
        {
            heading -= turnRate * dt;
        }
        if(heading < 0)
        {
            heading += Mathf.PI * 2;
        }
        if(heading > Mathf.PI * 2)
        {
            heading -= Mathf.PI * 2;
        }

        // Update speed
        if (Utils.ApproximatelyEqual(desiredSpeed, speed))
        {

        }
        else if(desiredSpeed > speed)
        {
            speed += acceleration * dt;
        }
        else if(desiredSpeed < speed)
        {
            speed -= acceleration * dt;
        }

        // Update velocity
        velocity = new Vector3(speed * Mathf.Sin(heading), 0, speed * Mathf.Cos(heading));
    }
}
