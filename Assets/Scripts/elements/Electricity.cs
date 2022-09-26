using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : Element
{

    void Awake()
    {
        type = "Electricity";
        // parti = (GameObject)Instantiate(Resources.Load("Electricity"), transform);
        // parti.transform.position = transform.position;
    }
    public override void Effect(Life life)
    {
        Spell spell = GetComponent<Spell>();
        if (spell.Level <= 1)
            return;
        float range = 8 + (level * 4);
        int foes = 0;
        List<GameObject> enemys = new List<GameObject>();
        if (GameObject.FindGameObjectWithTag("EnemyWizard"))
            enemys.Add(GameObject.FindGameObjectWithTag("EnemyWizard"));
        //adds friendly targets
        if (spell.dom.type == "Life")
            //if the dominent element is life, means that this is a healing spell, it should hit freindly troops as well as enemies
            foreach (GameObject friend in GameObject.FindGameObjectsWithTag("Friendly"))
            {
                if (friend != life.gameObject && Vector3.Distance(friend.transform.position, life.gameObject.transform.position) <= range)
                {
                    foes++;
                    enemys.Add(friend);
                    if (foes >= level / 2) break;
                }
            }
        print("foes"+foes);
        // adds all enemies that are in range
        foreach (GameObject foe in enemys)
        {
            //dont add them if they are already electrtuted(ill change that in the future)
            if (Vector3.Distance(foe.transform.position, life.gameObject.transform.position) <= range && !foe.GetComponent<Electricity>())
            {
                foes++;
                enemys.Add(foe);
                //limits the amount of enemies to chain to
                if (foes >= level / 2) break;
            }
        }
        //enemys.Remove(life.gameObject);
        if (foes != 0)
        {
            spell.Level /= foes + 1;
            foreach (GameObject foe in enemys)
            {
                //creaats a projectile that repesents the lightning chaining
                Vector3 target = foe.transform.position;
                GameObject pro = (GameObject)Instantiate(Resources.Load("Projectile"), life.transform.position + HelpfulFuncs.Norm1(target - life.transform.position), life.transform.rotation);
                pro.transform.rotation = Quaternion.Euler(90, 0, 0);
                Projectile lightningbolt = pro.AddComponent<Projectile>();
                //creats formal list for the new spell in the chainning 
                List<FormalEl> newFEL = new List<FormalEl>();
                foreach (Element el in spell.domEl)
                {
                    newFEL.Add(new FormalEl(el.type, el.level / (foes + 1)));
                }
                lightningbolt.spell = pro.AddComponent<Spell>();
                lightningbolt.spell.Create(newFEL);

                //shoot towards target
                target = HelpfulFuncs.Norm1(target - life.transform.position) * 50;
                pro.GetComponent<Rigidbody>().velocity = target;
            }
        }
    }
}
