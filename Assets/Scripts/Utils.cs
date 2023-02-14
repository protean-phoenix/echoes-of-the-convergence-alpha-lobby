using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    public static byte[] ConvertDoubleToByteArray(double d)
    {
        return BitConverter.GetBytes(d);
    }

    public static double ConvertByteArrayToDouble(byte[] b)
    {
        return BitConverter.ToDouble(b, 0);
    }
}