using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FormalMat  {
    public int mass;
    public string type;
    public FormalMat(string t, int l)
    {
        type = t;
        mass = l;
    }
}
