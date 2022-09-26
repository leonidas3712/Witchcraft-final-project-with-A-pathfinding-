using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FormalEl  {

    public string type;
    public int level;

    public FormalEl(string t,int l)
    {
        type = t;
        level = l;
    }
}
