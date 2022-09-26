using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPicker : MonoBehaviour
{
    protected CreatureAi Ai;
    public List<GameObject> priorityList;
    void Start()
    {
        Ai = GetComponent<CreatureAi>();
        priorityList = Ai.FoesInSight;
    }
   
    /// <summary>
    /// find a target to attack
    /// </summary>
    /// <returns></returns>
    public virtual Target TargetFinder() { return null; }

}
