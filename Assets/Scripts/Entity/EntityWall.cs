using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWall : MonoBehaviour
{
    public float xScale;
    public float zScale;
    public Vector3 position;

    public enum Shape { Square, Circle };

    public Shape thisShape;

    float naturalScale = 15.0f;

    public void OnStart()
    {
        gameObject.transform.position = position;
        float yScale = thisShape == Shape.Circle ? xScale : 1;
        gameObject.transform.localScale = new Vector3(xScale, yScale, zScale);
    }

    public bool TestPoint(Vector3 pointPos)
    {
        switch (thisShape)
        {
            case Shape.Circle:
                if (Vector3.Distance(position, pointPos) <= xScale * naturalScale / 2.0f) {
                    return true;
                }
                break;
            case Shape.Square:
                if (Mathf.Abs(position.x - pointPos.x) <= xScale * naturalScale / 2.0f && Mathf.Abs(position.z - pointPos.z) <= zScale * naturalScale / 2.0f)
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
