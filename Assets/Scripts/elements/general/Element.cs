using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Element : MonoBehaviour {

    
    public string type;
    public int level;

    public virtual void Effect(Life life) { }

    public virtual int Reaction(Spell El,int level)
    {
        if (El.dom.type == "Wind")
        {
            level -= El.Level;
        }
        return level;
    }

}
