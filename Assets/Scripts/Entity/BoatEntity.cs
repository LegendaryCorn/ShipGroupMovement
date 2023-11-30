using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEntity : MonoBehaviour
{
    public GameObject parent;

    public EntityPhysics physics;
    public EntityAI ai;

    public GameObject selectedIndicator;

    public void OnStart()
    {
        ai.OnStart();
        physics.OnStart();
    }

    public virtual void OnUpdate(float dt)
    {
        ai.OnUpdate(dt);
        physics.OnUpdate(dt);
    }
}
