using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestingStatus
{
    TEST,
    PROD
}

public class Utils : MonoBehaviour
{
    public static TestingStatus status = TestingStatus.TEST;
    
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

    public static float getAbsoluteDifference(float a, float b)
    {
        return Math.Abs(Math.Abs(a) - Math.Abs(b));
    }

    public static float getDistance(float a, float b)
    {
        if (a > 0 && b < 0)
            return a - b;

        else if (a < 0 && b > 0)
            return b - a;

        else
            return getAbsoluteDifference(a, b);
    }
}
