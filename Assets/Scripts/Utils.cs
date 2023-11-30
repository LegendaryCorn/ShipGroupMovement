using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool ApproximatelyEqual(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.01f;
    }

    public static float AngleBetween(float a, float b)
    {
        float diff = a - b;
        if(diff > Mathf.PI)
        {
            return diff - 2 * Mathf.PI;
        }
        if (diff < -Mathf.PI)
        {
            return diff + 2 * Mathf.PI;
        }
        return diff;
    }

    public static Rect GetScreenRect(Vector3 sp1, Vector3 sp2)
    {
        sp1.y = Screen.height - sp1.y;
        sp2.y = Screen.height - sp2.y;

        Vector3 topLeft = Vector3.Min(sp1, sp2);
        Vector3 bottomRight = Vector3.Max(sp1, sp2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }


    public static Bounds GetViewportBounds(Camera cam, Vector3 sp1, Vector3 sp2)
    {
        Vector3 v1 = Camera.main.ScreenToViewportPoint(sp1);
        Vector3 v2 = Camera.main.ScreenToViewportPoint(sp2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        min.z = cam.nearClipPlane;
        max.z = cam.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;

    }
}
