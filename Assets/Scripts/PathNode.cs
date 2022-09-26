using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PathNode 
{
    public int preX =-1, preY=-1, x,y;
    public bool visited = false;
    public float distence;
    public PathNode(int x, int y, int distence)
    {
        this.x = x;
        this.y = y;
        this.distence = distence;
    }
}
