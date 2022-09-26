using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HelpfulFuncs {

    public static Vector3 Norm1(Vector3 theVec)
    {
        Vector3 newVec = theVec;
        float a = Mathf.Atan2(newVec.z, newVec.x);
        newVec = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
        return newVec;
    }

    public static Vector3 Norm1Turnc(Vector3 theVec,int num)
    {
        Vector3 newVec = theVec;
        float a = Mathf.Atan2(newVec.z, newVec.x);
        newVec = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
        double ten = Mathf.Pow(10,num);
        newVec = new Vector3((float)(Math.Truncate((double)newVec.x * ten) / ten), (float)(Math.Truncate((double)newVec.y * ten) / ten), (float)(Math.Truncate((double)newVec.z * ten) / ten));
        return newVec;
    }
}
