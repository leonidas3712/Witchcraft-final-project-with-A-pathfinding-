using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Matter
{

    void Start()
    {
        type = "Metal";
        rig = GetComponent<Rigidbody>();
    }

    public override void creatureIntial()
    {
        Life life = GetComponent<Creature>().life;
        switch (life.level)
        {
            case 1: picker = gameObject.AddComponent<MetalPicker1>();
                break;
            case 2:
                picker = gameObject.AddComponent<MetalPicker2>();
                break;
            case 3:
                picker = gameObject.AddComponent<MetalPicker3>();
                break;
            case 4:
                picker = gameObject.AddComponent<MetalPicker4>();
                break;
        }
        DPS = GetComponent<Spell>().Level;
        AttackRange = life.Hp;
        GetComponent<CreatureAi>().sightRange = AttackRange * life.level;
        GetComponent<CreatureAi>().movmentSpeed = life.Vel * 2;
    }

    public override void Damage(Vector3 targetPos, Spell spell)
    {
        foreach (Life victem in GameObject.FindObjectsOfType<Life>())
        {
            if (Vector3.Distance(victem.transform.position, transform.position) <= GetComponent<Metal>().AttackRange)
            {
                Vector3 dir = victem.transform.position - transform.position;
                dir = HelpfulFuncs.Norm1(dir);
                foreach (Element el in spell.domEl)
                   victem.TakeDamage(el.level, el.type);
                victem.GetComponent<Rigidbody>().AddForce(dir * (spell.Level + 100) * 4);
            }
        }
        Destroy(gameObject);

    }
    public override void Move(float speed,Vector3 location)
    {
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(location.x - transform.position.x, location.z - transform.position.z) * Mathf.Rad2Deg, 0);
        //print("boll "+HelpfulFuncs.Norm1(transform.forward).Equals(HelpfulFuncs.Norm1(rig.velocity)));
        float curSpeed = Mathf.Sqrt(Mathf.Pow(rig.velocity.x, 2) + Mathf.Pow(rig.velocity.z, 2));
        if (speed == 0)
            rig.velocity = Vector3.zero;
        if (curSpeed > speed)
        {
            rig.AddForce(transform.forward * -(curSpeed - speed));
        }
        if (curSpeed <= speed || !HelpfulFuncs.Norm1(transform.forward).Equals(HelpfulFuncs.Norm1(rig.velocity)))
        {
            Vector3 vectorS = HelpfulFuncs.Norm1Turnc(rig.velocity, 1), vectorF = HelpfulFuncs.Norm1Turnc(transform.forward, 1);
            float a = Mathf.Atan2((vectorS.x - vectorF.x), (vectorS.z - vectorF.z)) * Mathf.Rad2Deg;

            if ((a > 70 || a < -70) && curSpeed >= speed / 3)
            {
                rig.AddForce(-rig.velocity * speed);
            }
            rig.AddForce(transform.forward * speed*3);
            print(speed);
        }
    }


    public override void Stop()
    {
        rig.AddForce(-rig.velocity*20);
    }
}
