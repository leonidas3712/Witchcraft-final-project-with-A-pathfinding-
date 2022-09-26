using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : StatusEffect
{

    /// <summary>
    /// this class case damage in conssistant intervals
    /// </summary>

    override public void Begin()
    {
        if (GetComponents<Burning>().Length > 1)
        {
            foreach (StatusEffect stat in GetComponents<Burning>())
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
        life.TakeDamage(1);
    }

}
