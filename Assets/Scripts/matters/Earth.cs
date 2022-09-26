using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Matter
{
    
    void Start()
    {
        type = "Earth";
        rig = GetComponent<Rigidbody>();
    }

    public override void creatureIntial()
    {
        Life life = GetComponent<Creature>().life;
        switch (life.level)
        {
            case 1:
                picker = gameObject.AddComponent<EarthPicker1>();
                break;
            case 2:
                picker = gameObject.AddComponent<EarthPicker2>();
                break;
            case 3:
                picker = gameObject.AddComponent<EarthPicker3>();
                break;
            case 4:
                picker = gameObject.AddComponent<EarthPicker4>();
                break;
        }
        DPS = GetComponent<Spell>().Level/GetComponent<CreatureAi>().intervals;
        AttackRange = life.Hp/2;
        GetComponent<CreatureAi>().sightRange = life.Hp * life.level;
    }


    public override void Damage(Vector3 targetPos, Spell spell)
    {
        Collider[] victems;
        float radius = AttackRange / 2;
        //attack all enemies within the attack hit box
        victems = Physics.OverlapSphere(transform.position+targetPos*radius,radius);
        foreach (Collider victem in victems)
        {
            if (victem.GetComponent<Life>()&&victem.gameObject!=gameObject)
                foreach (Element el in spell.domEl)
                    victem.GetComponent<Life>().TakeDamage(el.level, el.type);
        }
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg, 0);
    }


    public override void Move(float speed,Vector3 location)
    {
        rig.velocity = HelpfulFuncs.Norm1(location - transform.position) * speed;
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(location.x - transform.position.x, location.z - transform.position.z) * Mathf.Rad2Deg, 0);
    }

    public override void Stop()
    {
        rig.velocity = Vector3.zero;
    }

}
