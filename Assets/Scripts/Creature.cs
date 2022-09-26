using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    //the spell of the creature
    public Spell spell;
    //the life of the creature
    public Life life;
    public List<FormalMat> mats;
    //the ai of the creature
    CreatureAi Ai;
    //a list that is being used during the sorting of the matters
    public List<Matter> domMatters = new List<Matter>();

    //the matters that the creature is made of
    public Matter dom, sec, third;
    //the side the creature belong to
    public string side;

    /// <summary>
    /// sort the matters that constitute the creature and adds the fitting ai
    /// </summary>
    /// <param name="mats"> a list of formal elements that will constitute the creature</param>
    /// <param name="Felements">the formal elements of the spell the created the creature </param>
    public void Create(List<FormalMat> mats, List<FormalEl> Felements)
    {
        if (side == "Player")
            tag = "Friendly";
        else tag = "Foe";
        List<FormalEl> Felements2 = Felements;
        foreach (FormalEl el in Felements2) if (el.type == "Life")
            {
                string prefabName = "";
                if (GetComponent<Life>())
                {
                    prefabName = GetComponent<Life>().clientPreFabName;
                    Destroy(GetComponent<Life>());
                }
                life = gameObject.AddComponent<Life>();
                life.clientPreFabName = prefabName;
                life.level = el.level;
                Felements2.Remove(el);
                break;
            }
        //in case that the spell containd only life, add the life el to the attack spell(should be rivised in case that an empty fel list can be send to "Create" func)
        if (Felements2.Count == 0)
            Felements2.Add(new FormalEl("Life", life.level));
        ////////

        spell = gameObject.AddComponent<Spell>();
        spell.Create(Felements2);
        this.mats = mats;
        List<Matter> matters = Matters(mats);

        transform.position = GetComponentInParent<Transform>().position;
        foreach (Matter mat in matters)
        {
            life.Hp += mat.Mass;
            if (!dom) dom = mat;
            else
            {
                if (mat.Mass > dom.Mass)
                    dom = mat;
            }

        }

        life.Vel = 10 / life.Hp;


        foreach (Matter mat in matters)
        {
            if (mat.Mass == dom.Mass) domMatters.Add(mat);
            else
            {
                if (!sec) sec = mat;
                else if (mat.Mass > sec.Mass) sec = mat;
                else third = mat;
            }
        }
        if (!sec && domMatters.Count == 1)
        {
            sec = dom;
        }


        Ai = gameObject.AddComponent<CreatureAi>();
        

        foreach (Matter dommatter in domMatters)
        {
            dommatter.creatureIntial();
        }
        /*switch (dom.type)
        {
            case "Wood":
                intel = gameObject.AddComponent<TreeCreature>();
                break;
            case "Earth":
                intel = gameObject.AddComponent<EarthCreature>();
                break;
            case "Metal":
                intel = gameObject.AddComponent<MetalCreature>();
                break;
        }*/
    }

    /// <summary>
    /// create matters fitting the formal matters
    /// </summary>
    /// <param name="Fel"></param>
    /// <returns> alist of the matters created </returns>
    List<Matter> Matters(List<FormalMat> Fel)
    {
        List<Matter> mats = new List<Matter>();
        foreach (FormalMat el in Fel)
        {
            switch (el.type)
            {
                case "Earth":
                    mats.Add(gameObject.AddComponent<Earth>());
                    mats[mats.Count - 1].type = "Earth";
                    break;
                case "Metal":
                    mats.Add(gameObject.AddComponent<Metal>());
                    mats[mats.Count - 1].type = "Metal";
                    break;
                case "Wood":
                    mats.Add(gameObject.AddComponent<Wood>());
                    mats[mats.Count - 1].type = "Wood";
                    break;
            }
            mats[mats.Count - 1].Mass = Fel[mats.Count - 1].mass;
        }
        return mats;
    }


    void Update()
    {
        Ai.Act();
    }

}
