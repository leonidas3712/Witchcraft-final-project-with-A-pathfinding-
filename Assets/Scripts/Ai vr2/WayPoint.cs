using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    //the nodes connected to the node by a path(stright uninteraptted line)
    public List<WayPoint> neighbors;
    //the previous node in the shortest path calculated
    public WayPoint prev =null;

    public float distanceF =99999,distenceG;
    //whether the node has been visited by the algorithem or not. whether path has been calculated by the the path builder(for better preformence))
    // and wheter this is the finale node or not
    public bool visited,pathsAssigned,sEnd;
    /// <summary>
    /// resets the node information for repetitive ai use(without destroying it completly)
    /// </summary>
    public void ResetPoint()
    {
        visited = false;
        distanceF = 99999;
        distenceG = 0;
    }
}
