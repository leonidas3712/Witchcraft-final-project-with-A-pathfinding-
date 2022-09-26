using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowing : StatusEffect {
    float preVel, preSpeed, times,heatvalue/*the amount of velocity it gains each beat*/;

    /// <summary>
    /// this status slows the target and slowly fasten it till it reachs its previous speed
    /// </summary>
    
    private void FixedUpdate()
    {
        //freezing overrides slowing
        if(GetComponent<Freezing>())
            Destroy(this);
    }
    public override void Begin()
    {
        preVel = life.Vel;
        life.Vel /= 2;
        times=(Timer-Time.time)/Intervals;
        heatvalue = life.Vel / times;
        if (GetComponents<Slowing>().Length > 1)
        {
            foreach (StatusEffect stat in GetComponents<Slowing>())
            {
                if (stat != this)
                {
                    stat.Timer += this.Timer - Time.time;
                    Destroy(this);
                }
            }
        }
    }
    public override void Effect()
    {
        if (life.Vel + heatvalue < preVel)
            life.Vel += heatvalue;
        else
            life.Vel = preVel;

    }
    public override void End()
    {
        life.Vel = preVel;
        base.End();
    }
}
