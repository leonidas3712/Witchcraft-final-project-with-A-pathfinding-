using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePicker1 : TargetPicker
{
    public override Target TargetFinder()
    {
        GameObject targetFoe = Ai.FoesInSight[0];

        foreach (GameObject foe in Ai.FoesInSight)
        {
            if (Vector3.Distance(transform.position, foe.transform.position) < Vector3.Distance(transform.position, targetFoe.transform.position))
                targetFoe = foe;
        }
        if (targetFoe == null)
            print("fuck2");
        //the position which you aim to get to
        return GetComponent<Matter>().GameObjectToTarget(targetFoe);

    }
}
