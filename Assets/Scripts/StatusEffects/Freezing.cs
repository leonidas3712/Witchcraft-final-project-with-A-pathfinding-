using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezing : StatusEffect
{
    //the velocity that the target had before bieng frozen
    float preVel;

    //this status stops the target in place

    override public void Begin()
    {
        preVel = life.Vel;
        life.Vel = 0;
        float tempTimer = Timer - Time.time;
        if (GetComponent<Burning>())
        {
            tempTimer -= GetComponent<Burning>().Timer - Time.time;
            float tempTimer2 = GetComponent<Burning>().Timer - Time.time;
            if (tempTimer <= 0)
            {
                GetComponent<Burning>().Timer -= Timer - Time.time;
                Destroy(this);
            }
            Timer -= tempTimer2;
        }
        if (GetComponents<Freezing>().Length > 1)
        {
            foreach (StatusEffect stat in GetComponents<Freezing>())
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
        life.Vel = preVel;
        StatusEffect stat = gameObject.AddComponent<Slowing>();
        InterTimer = Time.time + Intervals;
        stat.Timer = Time.time + 2;
        base.End();
    }

}
