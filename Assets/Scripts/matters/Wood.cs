using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Matter
{
    void Start()
    {
        type = "Wood";
        rig = GetComponent<Rigidbody>();
    }

    public override void creatureIntial()
    {
        
        Life life = GetComponent<Creature>().life;
        switch (life.level)
        {
            case 1:
                picker = gameObject.AddComponent<TreePicker1>();
                break;
            case 2:
                picker = gameObject.AddComponent<TreePicker2>();
                break;
            case 3:
                picker = gameObject.AddComponent<TreePicker3>();
                break;
            case 4:
                picker = gameObject.AddComponent<TreePicker4>();
                break;
        }
        DPS = GetComponent<Spell>().Level / GetComponent<CreatureAi>().intervals;
        AttackRange = life.Hp * life.level;
        GetComponent<CreatureAi>().sightRange = AttackRange;
    }

    public override void Damage(Vector3 targetPos, Spell spell)
    {
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(targetPos.x, targetPos.z ) * Mathf.Rad2Deg, 0);

        GameObject pro = (GameObject)Instantiate(Resources.Load("Projectile"), transform.position + transform.forward, transform.rotation);
        pro.name = pro.name + Weapon.counter;
        Weapon.counter++;

        Spell newspell = pro.AddComponent<Spell>();
        newspell.Create(spell.Felements);
        pro.transform.rotation = Quaternion.Euler(90, 0, 0);

        Creature c= GetComponent<Creature>();
        if (c.tag == "Friendly")
            pro.AddComponent<Projectile>().side = "Player";
        else pro.AddComponent<Projectile>().side = "EnemyWizard";

        targetPos *= GetComponent<Creature>().life.level;
        pro.GetComponent<Rigidbody>().velocity = new Vector3(targetPos.x, 0, targetPos.z);

    }
}
