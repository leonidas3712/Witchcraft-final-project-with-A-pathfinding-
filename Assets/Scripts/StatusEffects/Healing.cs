using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : StatusEffect
{

    override public void Begin()
    {
        if (GetComponents<Healing>().Length > 1)
        {
            foreach (StatusEffect stat in GetComponents<Healing>())
            {
                if (stat != this)
                {
                    stat.Timer += this.Timer - Time.time;
                    Destroy(this);
                }
            }
        }
    }


    override public void Effect()
    {
        life.TakeDamage(-2);
        print("heal");
    }
}
