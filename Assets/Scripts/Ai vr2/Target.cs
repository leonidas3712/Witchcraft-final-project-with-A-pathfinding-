using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Target 
{
    //the diraction to attack to from targetPosition
    public Vector3 attackDirection;
    //the location to stand on to be able to attack
    public Vector3 targetPostion;
    public Matter matterUsed;//the matter that was uesd when selecting this target
    public GameObject creature;

    public Target(Vector3 attackDirection, Vector3 targetPostion, Matter matterUsed,GameObject creature)
    {
        this.attackDirection = attackDirection;
        this.matterUsed = matterUsed;
        this.targetPostion = targetPostion;
        this.creature = creature;
    }
    public Target(Vector3 attackDirection, Vector3 targetPostion)
    {
        this.attackDirection = attackDirection;
        this.targetPostion = targetPostion;
    }
}
