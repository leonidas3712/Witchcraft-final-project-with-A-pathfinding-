using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Spell spell;
    int tempLevel;
    public string side;

    void Start()
    {
        transform.parent = GameObject.Find("spells").transform;
        tag = "Projectile";
        spell = GetComponent<Spell>();
        tempLevel = spell.Level;
        Collider coll = GetComponent<Collider>();
        foreach (Collider coll2 in GameObject.Find("obstacles").GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(coll,coll2,true);
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        Spell ODom = coll.GetComponent<Spell>();
        Life victem = coll.GetComponent<Life>();
        if (coll.tag == side) return;
        //ברגע שיש לי עוד קאסטינג לחזור לפה לטפל בהבדלה בין קסם ליצור
        if (ODom != null && spell && !coll.GetComponent<Creature>())
        {
            foreach (Element el in spell.domEl)
            {
                tempLevel = el.Reaction(ODom, spell.Level);
            }
        }
        //youve hit a creature(or a wizard) or a material. do your shit!
        if (victem)
        {
            //if you have life 
            if (spell.hasLife)
            {
                //if its a material make a creature of it
                if (coll.tag == "Material")
                {
                    CreateCreature(coll.gameObject);
                    return;
                }
            }
            //either way damage 
            spell.Damage(victem);
        }
    }
    void OnTriggerStay(Collider coll)
    {
        //this happens here so each object of the collision could use the objects stats before one of them is destroyed
        if (tempLevel <= 0 || spell.Level <= 0)
        {
            Destroy(gameObject);
        }

        if (spell && spell.Level != tempLevel) spell.Level = tempLevel;
    }

    void CreateCreature(GameObject victem)
    {
        List<FormalMat> mats = new List<FormalMat>();
        //duplicate materials placed in the hited map object to the creature then destroy them
        foreach (Matter mat in victem.GetComponents<Matter>())
        {
            mats.Add(new FormalMat(mat.type, mat.Mass));
            Destroy(mat);
        }
        Creature c = victem.gameObject.AddComponent<Creature>();
        c.side = side;
        c.Create(mats, spell.Felements);
        //destroy projectile
        Destroy(gameObject);
    }
}