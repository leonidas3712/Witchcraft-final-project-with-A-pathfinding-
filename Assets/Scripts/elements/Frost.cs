using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Element
{

    void Start()
    {
        type = "Frost";
    }

    
    public override int Reaction(Spell El, int level)
    {
        //frost melts by fire, though im not sure if that sooposed to be written in both fire and frost
        if (El.dom.type == "Fire")
        {
            level -= El.Level;
            //if (level <= 0) Destroy(this);
        }
        else level = base.Reaction(El, level);
        return level;
    }

    public override void Effect(Life life)
    {
        Slowing slowing = life.gameObject.AddComponent<Slowing>();
        slowing.Timer += level;
    }
}
