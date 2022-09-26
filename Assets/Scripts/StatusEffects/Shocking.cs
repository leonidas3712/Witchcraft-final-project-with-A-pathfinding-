using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocking : StatusEffect
{
    //this status effect slows the target and slows it
    float temp;
    override public void Begin()
    {

        temp = life.Vel;
        life.Vel /= 4;
        if (GetComponents<Shocking>().Length > 1)
        {
            foreach (StatusEffect stat in GetComponents<Shocking>())
            {
                if (stat != this)
                {
                    stat.Timer += this.Timer - Time.time;
                    Destroy(this);
                }
            }
        }
    }
    override public void End()
    {
        life.Vel = temp;
        base.End();
    }

    override public void Effect()
    {
        life.TakeDamage(1);
    }
}
