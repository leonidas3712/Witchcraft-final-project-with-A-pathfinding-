using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {


    public float burntime;
    void Start()
    {
        type = "Fire";
        //parti = (GameObject)Instantiate(Resources.Load("Fire"), transform);
        //parti.transform.position = transform.position;
    } 

    public override int Reaction(Spell El,int level)
    {
        //fire is put off by frost
        if(El.dom.type == "Frost")
        {
            level -= El.Level;
            //if (level <= 0) Destroy(this);
        }
        else level = base.Reaction(El,level);
        return level;
    }

    public override void Effect(Life life)
    {
        Spreading spreading = life.gameObject.AddComponent<Spreading>();
        List<string> DominantEls = new List<string>();
        //spreading creats its own spelles on contact so it needs only elemnt type list
        foreach(Element el in GetComponent<Spell>().domEl)
        {
            DominantEls.Add(el.type);
        }
        spreading.DominentEls = DominantEls;
        spreading.level = level;
        spreading.Timer += level *3;
        ///
    }
    
}
