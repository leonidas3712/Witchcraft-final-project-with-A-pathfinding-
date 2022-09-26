using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    //all graphs
    GraphBuilder[] graphs;
    
    GameObject mapObjects;
    //how many objects mapobjects containes
    int mapObjectsLength;
    private void Start()
    {
        graphs = GetComponentsInChildren<GraphBuilder>();
        mapObjects = GameObject.Find("mapobjects");
        mapObjectsLength = mapObjects.GetComponentsInChildren<Transform>().Length;
    }
    private void Update()
    {
        int newLength = mapObjects.GetComponentsInChildren<Transform>().Length;
        if (newLength != mapObjectsLength)
        {
            foreach (GraphBuilder graph in graphs)
            {
                //notice th recall bug(when a graph is checked for validation afteter bieng recalled)
                if (graph.mapGraph.Count != 0)
                {
                    graph.destroyGraph();
                    graph.BuildGraph();
                }
            }
            mapObjectsLength = newLength;
        }

    }
}
