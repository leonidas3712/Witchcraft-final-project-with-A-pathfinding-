using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spell : MonoBehaviour
{
    public List<Element> sec = new List<Element>(), domEl = new List<Element>();
    public Element dom;
    public int Level;
    //[NonSerialized]
    public List<FormalEl> Felements;
    public bool hasLife = false;
    GameObject spellDetail;

   /* private void Start()
    {
        spellDetail = (GameObject)Instantiate(Resources.Load("SpellDetail"), GameObject.Find("Canvas").transform);
        spellDetail.GetComponent<SpellDetail>().spell = this;
        spellDetail.GetComponent<SpellDetail>().parent = transform;
    }
    private void OnDestroy()
    {
        Destroy(spellDetail);
    }*/

    /// <summary>
    /// sort the spells given to create the spell
    /// </summary>
    /// <param name="Felements"> a list of formal elements the will determine what elements will construct the spell</param>
    public void Create(List<FormalEl> Felements)
    {
        //duplicates the formal el list so it wont destroy the orinial one
        this.Felements = new List<FormalEl>();
        for (int i = 0; i < Felements.Count; i++)
        {
            this.Felements.Add(Felements[i]);
        }
        List<Element> elements = Elements(Felements);

        //transform.position = GetComponentInParent<Transform>().position;
        //sums the general level of the spell and finds the dominant element
        foreach (Element el in elements)
        {
            if (el.type == "Life") hasLife = true;
            Level += el.level;
            if (!dom) dom = el;
            else
            {
                if (el.level > dom.level)
                    dom = el;
            }
        }
        foreach (Element el in elements)
        {
            if (el.level == dom.level) domEl.Add(el);
            else sec.Add(el);
        }
        if (sec.Count == 0 && domEl.Count == 1)
            sec.Add(dom);

    }

    //turns formal element list into real elements and return them in a list
    List<Element> Elements(List<FormalEl> Fel)
    {
        List<Element> elements = new List<Element>();
        foreach (FormalEl el in Fel)
        {
            switch (el.type)
            {
                case "Fire":
                    elements.Add(gameObject.AddComponent<Fire>());
                    elements[elements.Count - 1].type = "Fire";
                    break;
                case "Frost":
                    elements.Add(gameObject.AddComponent<Frost>());
                    elements[elements.Count - 1].type = "Frost";
                    break;
                case "Wind":
                    elements.Add(gameObject.AddComponent<Wind>());
                    elements[elements.Count - 1].type = "Wind";
                    break;
                case "Electricity":
                    elements.Add(gameObject.AddComponent<Electricity>());
                    elements[elements.Count - 1].type = "Electricity";
                    break;
                case "Life":
                    elements.Add(gameObject.AddComponent<Life>());
                    elements[elements.Count - 1].type = "Life";
                    break;
            }
            elements[elements.Count - 1].level = Fel[elements.Count - 1].level;
        }
        return elements;
    }


    /// <summary>
    /// execute all the effects/secondery qualities of the secondery elements consisting the spell
    /// </summary>
    /// <param name="victem"> the traget the spell hit</param>
    /// <param name="dam">the damage caused by the spell </param>
    public void Effect(Life victem, float dam)
    {
        if (hasLife && sec.Contains(GetComponent<Life>()))
        {
            dom = GetComponent<Life>();
            //sec containes only life
            if (sec.Count == 1)
            {
                foreach (Element el in domEl)
                {
                    sec.Add(el);
                }
                domEl.Clear();
            }
            sec.Remove(GetComponent<Life>());
            domEl.Add(dom);
            //if the life level is higher then the damage that was made then use the damage as the healing factor and not the life level
            if (dom.level > dam) dom.level = (int)dam;
        }
        foreach (Element el in sec)
        {
            el.Effect(victem);
        }
    }

    /// <summary>
    /// execute all damages of the dominant elements
    /// </summary>
    /// <param name="victem"></param>
    public void Damage(Life victem)
    {
        float damage = 0;
        string side;
        if (GetComponent<Projectile>().side == "Player")
            side = "Friendly";
        else side = "Foe";
        if (victem.tag ==side )
            foreach (Element el in domEl)
                victem.FriendlyFire(el.level, el.type);
        else
            foreach (Element el in domEl)
                damage = victem.TakeDamage(el.level, el.type);

        Level -= (int)victem.Hp;
    }

}
