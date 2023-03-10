using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float getDistanceTo(Vector3 origin, Vector3 dest)
    {
        float target_x = dest.x;
        float target_y = dest.y;

        float origin_x = origin.x;
        float origin_y = origin.y;

        float diff_x = target_x - origin_x;
        float diff_y = target_y - origin_y;

        return Mathf.Sqrt(Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2));
    }

    public static float getAngleToInRadians(Vector3 origin, Vector3 dest)
    {
        float target_x = dest.x;
        float target_y = dest.y;

        float origin_x = origin.x;
        float origin_y = origin.y;

        float diff_x = target_x - origin_x;
        float diff_y = target_y - origin_y;

        return Mathf.Atan2(diff_y, diff_x);
    }

    public static float getEllipsePerimeter(float a, float b)
    {
        if (b > a)
        {
            float c = b;
            b = a;
            a = c;
        }
        float h = Mathf.Pow(((a - b) / (a + b)), 2);
        float p = Mathf.PI * (a + b) * (1 + (3 * h / (10 + Mathf.Sqrt(4 - 3 * h))));
        return p;
    }
}
