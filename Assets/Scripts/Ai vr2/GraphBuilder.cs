using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBuilder : MonoBehaviour
{
    public List<WayPoint> mapGraph = new List<WayPoint>();
    public List<Rectangle> rects = new List<Rectangle>();
    public float agentWidth;
    public bool Baked;
    WayPoint agent = null, endpoint = null;
    //binary mask for raycating(to ignore a layer)
    int layermask = 1 << 8;
    public int iterations = 0;
    float agentY = 13.92266f;

    //the function will intiate the prosses of building a waypoint graph,building it in mapgraph list
    public void BuildGraph()
    {
        Baked = false;
        rects.Clear();
        mapGraph.Clear();
        
        Transform[] mapObjects = GameObject.Find("mapobjects").GetComponentsInChildren<Transform>();
        //add all waypoints
        foreach (Transform mb in mapObjects)
        {
            if (mb.GetComponent<Collider>())
            {
                MakeVerts(mb, agentWidth);
            }
        }
        mapObjects = GameObject.Find("obstacles").GetComponentsInChildren<Transform>();
        //add all waypoints
        foreach (Transform mb in mapObjects)
        {
            if (mb.GetComponent<Collider>())
            {
                MakeVerts(mb, agentWidth);
            }
        }

        StartCoroutine(checkPathes());
    }
    //
    /// <summary>
    /// this is a coroutine, a way for unity to time methods comfortobly. for some reason the triggers that are being used to 
    /// create pathes bteween nodes are being created a frame later then the one one that they are bieng called to be created in
    /// (in builedgraph method). so this corotine will hold the graph creation proccess for one frame then calculate the pathes btween node
    /// </summary>
    /// <returns>ienumarator, so the script can wait a frame then get back to where it stopped</returns>
    IEnumerator checkPathes()
    {
        yield return 0;
        foreach (WayPoint point in mapGraph)
        {
            foreach (WayPoint point2 in mapGraph)
            {
                if (!point2.pathsAssigned && point && point2)
                {
                    //checks if there is a clear path between the two point
                    if (!Physics.Raycast(point.transform.position, Vector3.Normalize(point2.transform.position - point.transform.position), Vector3.Distance(point2.transform.position, point.transform.position), layermask)&&
                        !Physics.Raycast(point2.transform.position, Vector3.Normalize(point.transform.position-point2.transform.position ), Vector3.Distance(point2.transform.position, point.transform.position), layermask))
                    {
                        point.neighbors.Add(point2);
                        point2.neighbors.Add(point);
                    }
                }
            }
            point.pathsAssigned = true;
            Baked = true;
        }
    }
    /// <summary>
    /// adjust the graph to a specific ai
    /// adds the beginning node and finish node 
    /// calculate distences (g) for each node
    /// </summary>
    /// <param name="target"></param>
    /// <param name="self"></param>
    /// <returns>the start node for the graph, the one that represents the ai</returns>
    public WayPoint adjustGraph(GameObject target, GameObject self)
    {

        GameObject vert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), self.transform.position, Quaternion.Euler(0, 0, 0), self.transform);
        mapGraph.Add(vert.GetComponent<WayPoint>());
        agent = vert.GetComponent<WayPoint>();
        if (!Baked)
        {
            return vert.GetComponent<WayPoint>();
        }

        Rectangle targetRect = new Rectangle();
        foreach(Rectangle rect in rects)
        {
            if (rect.representedObject == target)
            {
                rect.trriger.GetComponent<Collider>().enabled = false;
                break;
            }
        }
       /* foreach (WayPoint v in targetRect.verts)
        {
            mapGraph.Remove(v);
            foreach (WayPoint neigh in v.neighbors)
            {
                neigh.neighbors.Remove(v);
            }
        }*/

        //target point
        GameObject targetVert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), target.transform.position, Quaternion.Euler(0, 0, 0), self.transform);
        mapGraph.Add(targetVert.GetComponent<WayPoint>());
        targetVert.GetComponent<WayPoint>().sEnd = true;
        endpoint = targetVert.GetComponent<WayPoint>();

        foreach (WayPoint point in this.mapGraph)
        {
            //set the distance from the final node
            point.distenceG = Vector3.Distance(targetVert.transform.position, point.transform.position);
            //checks if there is a clear path btween the two points
            if (point != agent)
                if (!Physics.Raycast(point.transform.position, Vector3.Normalize(vert.transform.position - point.transform.position), Vector3.Distance(vert.transform.position, point.transform.position), layermask, QueryTriggerInteraction.Collide))
                {
                    point.neighbors.Add(agent);
                    agent.neighbors.Add(point);
                }
            if (point != endpoint)
                if (!Physics.Raycast(point.transform.position, Vector3.Normalize(targetVert.transform.position - point.transform.position), Vector3.Distance(targetVert.transform.position, point.transform.position), layermask, QueryTriggerInteraction.Collide))
                {
                    point.neighbors.Add(endpoint);
                    endpoint.neighbors.Add(point);
                }
        }
        return vert.GetComponent<WayPoint>();
    }
    /// <summary>
    /// readjust the graph
    /// deletes the beginning and finish node
    /// reset distences recorded in each node and the path calculated for the ai
    /// </summary>
    public void reAdjustGraph(GameObject target)
    {
        if (endpoint)
            foreach (WayPoint neibhor in endpoint.neighbors)
            {
                neibhor.neighbors.Remove(endpoint);
            }
        foreach (WayPoint neibhor in agent.neighbors)
        {
            neibhor.neighbors.Remove(agent);
        }

        Destroy(agent.gameObject);
        mapGraph.Remove(agent);
        if (endpoint)
        {
            mapGraph.Remove(endpoint);
            Destroy(endpoint.gameObject);
        }
        foreach (WayPoint point in mapGraph)
        {
            foreach (WayPoint point2 in point.neighbors) Debug.DrawLine(point.transform.position, point2.transform.position, Color.blue, 2);
            point.ResetPoint();

        }
        foreach (Rectangle rect in rects)
        {
            if (rect.representedObject == target)
            {
                rect.trriger.GetComponent<Collider>().enabled = true;
                break;
            }
        }
    }
    /// <summary>
    /// destroy the graph so a new one could be built
    /// </summary>
    public void destroyGraph()
    {
        /*foreach (Rectangle rect in rects)
        {
            Destroy(rect.representedObject);
        }*/
        foreach (Transform mb in GetComponentsInChildren<Transform>())
        {
            if (mb != transform)
            {
                Destroy(mb.gameObject);
            }
        }
        iterations++;
    }
    /// <summary>
    /// create nodes by a given map object
    /// </summary>
    /// <param name="mb"></param>
    /// <param name="offset"></param>
    void MakeVerts(Transform mb, float offset)
    {
        Rectangle rect = new Rectangle();
        GameObject rectTrriger = (GameObject)GameObject.Instantiate(Resources.Load("rectPrefab"), mb.position, Quaternion.Euler(0, 0, 0), transform);
        //rectTrriger.layer = 8;
        rect.representedObject = mb.gameObject;
        rect.trriger = rectTrriger;
        rects.Add(rect);
        float x = mb.GetComponent<Collider>().bounds.size.x * 0.5f;
        float z = mb.GetComponent<Collider>().bounds.size.z * 0.5f;
        rectTrriger.transform.localScale = new Vector3(x * 2 + offset * 2 - 0.1f, mb.GetComponent<Collider>().bounds.size.y, z * 2 + offset * 2 - 0.1f);
        GameObject vert;

        //if the creature point is on the object vert point then the creature point is replaced with it
        vert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), new Vector3(mb.position.x + x + offset, agentY, mb.position.z + z + offset), Quaternion.Euler(0, 0, 0), rectTrriger.transform);
        rect.verts.Add(vert.GetComponent<WayPoint>());
        mapGraph.Add(vert.GetComponent<WayPoint>());

        vert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), new Vector3(mb.position.x + x + offset, agentY, mb.position.z - z - offset), Quaternion.Euler(0, 0, 0), rectTrriger.transform);
        rect.verts.Add(vert.GetComponent<WayPoint>());
        mapGraph.Add(vert.GetComponent<WayPoint>());

        vert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), new Vector3(mb.position.x - x - offset, agentY, mb.position.z - z - offset), Quaternion.Euler(0, 0, 0), rectTrriger.transform);
        rect.verts.Add(vert.GetComponent<WayPoint>());
        mapGraph.Add(vert.GetComponent<WayPoint>());

        vert = (GameObject)GameObject.Instantiate(Resources.Load("WayPoint"), new Vector3(mb.position.x - x - offset, agentY, mb.position.z + z + offset), Quaternion.Euler(0, 0, 0), rectTrriger.transform);
        rect.verts.Add(vert.GetComponent<WayPoint>());
        mapGraph.Add(vert.GetComponent<WayPoint>());



    }
    /// <summary>
    /// an experiment to calculate pathes using algebra, ended up not useing it and using raycasting instead
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    bool Pather(Vector3 p1, Vector3 p2)
    {
        float a = (p1.z - p2.z) / (p1.x - p2.x), b = p2.z - p2.x * a, maxX = Mathf.Max(p1.x, p2.x), maxY = Mathf.Max(p1.z, p2.z), minX = Mathf.Min(p1.x, p2.x), minY = Mathf.Min(p1.z, p2.z);
        int count = 0;
        foreach (Rectangle rect in rects)
        {
            foreach (WayPoint vert in rect.verts)
            {
                int index = rect.verts.IndexOf(vert) + 1;
                if (index == rect.verts.Count)
                    index = 0;
                Vector3 vert2Pos = rect.verts[index].transform.position;
                float a2 = (vert.transform.position.z - vert2Pos.z) / (vert.transform.position.x - vert2Pos.x), b2 = vert2Pos.z - vert2Pos.x * a2;
                float interX = (b - b2) / (a2 / a), interY = a * interX + b;
                if (interX < maxX && interX > minX && interY < maxY && interY > minY) return true;
                count++;
            }
        }
        print(count);
        return false;
    }
}
