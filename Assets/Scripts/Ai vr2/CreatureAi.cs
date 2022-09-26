using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAi : MonoBehaviour
{
    Creature myCreature;
    List<Matter> domMatters;
    Matter secMatter, thirdMatter;
    GameObject enemyWizard;
    //strings that will be used to determine on which side the creature will fight on
    public string enemy, foe;

    //stats
    public float intervals, movmentSpeed, sightRange;
    //*****
    //timing stuff
    float timer = 0, pathfindingTimer;
    //targeting
    public List<GameObject> FoesInSight = new List<GameObject>();
    List<Target> targets = new List<Target>();
    public Target target;

    //pathfinding related:
    public PathNode[,] locationGraph;
    int arenaHight, arenaWidth;
    //the above are used for an older virsion of the path finding that uses a grid graph and not waypoint graph
    public Vector3 nextPosition = Vector3.zero;
    List<Vector3> path = new List<Vector3>();
    GraphBuilder GB;
    //used to track the graph iteration so the ai will be updated accordingly
    int graphIteration = 0;

    private void Awake()
    {
        GB = GameObject.Find("MapGraphBuilder").GetComponent<GraphBuilder>();
        myCreature = GetComponent<Creature>();
        secMatter = myCreature.sec;
        thirdMatter = myCreature.third;
        domMatters = myCreature.domMatters;
        if (myCreature.side == "Player")
        {
            enemy = "EnemyWizard";
            foe = "Foe";
        }
        else
        {
            enemy = "Player";
            foe = "Friendly";
        }
        enemyWizard = GameObject.FindGameObjectWithTag(enemy);
        movmentSpeed = myCreature.life.Vel;
        intervals = 3 / movmentSpeed;
        if (intervals < 0.4) intervals = 0.4f;
    }
    /// <summary>
    /// act is the descision tree, it will invoke all the ai actions
    /// </summary>
    public void Act()
    {
        //add the enemy wizard to the foes in sight list
        //if (Vector3.Distance(enemyWizard.transform.position, transform.position) <= sightRange)
        FoesInSight.Add(enemyWizard);
        foreach (GameObject foe in GameObject.FindGameObjectsWithTag(foe))
        {
            if (Vector3.Distance(foe.transform.position, transform.position) <= sightRange)
                FoesInSight.Add(foe);
        }
        //if there are foes in sight
        /* if (FoesInSight.Count > 0)
         {*/
        foreach (Matter matter in domMatters)
            targets.Add(matter.picker.TargetFinder());
        if (targets.Count != 0)
        {
           // if (target == null || target.creature != targets[0].creature)
                target = targets[0];//to be changed!!

            //if you reached your target
            //print("dis = "+Vector3.Distance(transform.position, target.targetPostion));
            if (Vector3.Distance(transform.position, target.targetPostion) <= 0.1f)
            {
                if ((GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0) && target.matterUsed.type != "Metal")
                {
                    transform.SetParent(GameObject.Find("mapobjects").transform);
                    //gameObject.layer = 8;
                    secMatter.Stop();
                    nextPosition = Vector3.zero;
                    print("stop");
                }
                else
                {
                    if (timer <= Time.time)
                    {
                        target.matterUsed.Damage(target.attackDirection, myCreature.spell);
                        timer = Time.time + intervals;
                    }
                }
            }
            else
            {
                move();
            }
        }
        else
        {
            secMatter.Stop();
            print("stop");
        }

        /*  }
          else//go for the enemy wizard
          {
              foreach (Matter matter in domMatters)
                  targets.Add(matter.GameObjectToTarget(enemyWizard));
              target = targets[0];//to be changed!!
              print("dis = " + Vector3.Distance(transform.position, target.targetPostion));
              move();
              print("move enemy wizard");
          }*/

        targets.Clear();
        FoesInSight.Clear();
    }
    /// <summary>
    /// will call a grph to be built if one doesnt exist and invoke pathfinding with A* and creature movment
    /// </summary>
    void move()
    {
        transform.SetParent(GameObject.Find("creatures").transform);
        gameObject.layer = 0;
        if (GB.mapGraph.Count == 0)
        {
            GB.BuildGraph();
        }
        if (nextPosition == Vector3.zero || graphIteration != GB.iterations || nextPosition == null)
        {
            path.Clear();
            PathFinder2();
           /* int i = 0;
            while (i<3&&nextPosition==Vector3.zero)
            {
                targetAdjuster();
                PathFinder2();
                i++;
            }*/

        }

        if (pathfindingTimer < Time.time)
        {
            if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                nextPosition = path[path.Count - 1];
                path.Remove(path[path.Count - 1]);
            }
            //pathfindingTimer = Time.time + 0.3f;
        }
        if (Vector3.Distance(transform.position, nextPosition) > 0.1f && nextPosition != Vector3.zero && nextPosition != null)
            secMatter.Move(movmentSpeed, nextPosition);
        print("move");
    }
    /// <summary>
    /// the A* algorithem, it modifies the 'path' location list and move() use it 
    /// </summary>
    void PathFinder2()
    {
        //begines by adjusting the graph
        WayPoint cur = GB.adjustGraph(target.creature, gameObject), startPoint = cur;
        graphIteration = GB.iterations;
        cur.distanceF = 0;
        //the list in which calculated node will be kept track on with
        List<WayPoint> calculatedPoints = new List<WayPoint>();
        bool found = true;
        //the loop which that will run the algorithem itself
        while (calculatedPoints.Count < GB.mapGraph.Count)
        {
            bool end = false;
            cur.visited = true;
            foreach (WayPoint point in cur.neighbors)
            {
                if (!point.visited)
                {
                    float newDisF = Vector3.Distance(point.transform.position, cur.transform.position) + cur.distanceF;
                    if (point.distanceF > newDisF)
                    {
                        calculatedPoints.Add(point);
                        point.distanceF = newDisF;
                        point.prev = cur;
                    }
                    if (point.sEnd)
                    {
                        end = true;
                        cur = point;
                        break;
                    }
                }
            }
            if (end) break;
            float minDis = 999999999;
            foreach (WayPoint point in calculatedPoints)
            {
                if (minDis > point.distanceF + point.distenceG && !point.visited)
                {
                    minDis = point.distanceF + point.distenceG;
                    cur = point;
                }
            }
            //in case that it couldnt reach all the nodes but also not the finsh node
            if (minDis == 999999999)
            {
                found = false;
                break;
            }
        }
        if (found)
        {
            while (cur.prev && cur != startPoint)
            {
                path.Add(cur.transform.position);
                cur = cur.prev;
            }
            nextPosition = path[path.Count - 1];
            path.Remove(nextPosition);
        }
        else nextPosition = Vector3.zero;
        //readjust the graph so that other ai could use it
        GB.reAdjustGraph(target.creature);
        print("pathfinder Excuted" + gameObject);
    }

   
}