using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : Element
{

    public float Hp, Vel,maxHp;
    //amount of ammunity for each element
    public float Cfire, Cfrost, Cwind, Celec;
    //the prefab of the object that will be used to represent the 
    public string clientPreFabName;
    GameObject hpBar;

    private void Start()
    {
        type = "Life";
        maxHp = Hp;
        hpBar = (GameObject)Instantiate(Resources.Load("HpBar"), GameObject.Find("Canvas").transform);
        hpBar.GetComponent<HealthBar>().life = this;
    }
    private void OnDestroy()
    {
        Destroy(hpBar);
    }
    /// <summary>
    /// called when the life takes damage. reduce hp and starts status effects
    /// </summary>
    /// <param name="dam"></param>
    /// <param name="type"></param>
    /// <returns>the damage that has been done for the use of the life uniqe </returns>
    public float TakeDamage(float dam, string type)
    {
        /*if (type!="Life")
            Hp -= dam;*/
        float damage = 0;
        if (Hp <= 0) Destroy(gameObject);
        switch (type)
        {
            case "Fire":
                Cfire -= dam;
                damage = -Cfire;
                if (Cfire <= 0)
                {
                    Hp += Cfire;
                    print(Cfire+" fire damage to "+name);
                    StatusEffect stat = gameObject.AddComponent<Burning>();
                    stat.Timer += -Cfire;
                    Cfire = 0;
                }
                break;
            case "Wind":

                Cwind -= dam;
                if (Cwind <= 0)
                {
                    Hp += Cwind;
                    print(Cwind + " wind damage to " + name);
                    damage = -Cwind;
                    StatusEffect stat = gameObject.AddComponent<Bleeding>();
                    stat.Timer += -Cwind;
                    Cwind = 0;
                }

                break;
            case "Frost":

                Cfrost -= dam;
                if (Cfrost <= 0)
                {
                    Hp += Cfrost;
                    print(Cfrost + " frost damage to " + name);
                    damage = -Cfrost;
                    StatusEffect stat = gameObject.AddComponent<Freezing>();
                    stat.Timer += -Cfrost;
                    Cfrost = 0;
                }

                break;
            case "Electricity":

                Celec -= dam;
                if (Celec <= 0)
                {
                    Hp += Celec;
                    print(Celec + " electicity damage to " + name);
                    damage = -Celec;
                    StatusEffect stat = gameObject.AddComponent<Shocking>();
                    stat.Timer += -Celec;
                    Celec = 0;
                }
                break;

            case "Life":
                print(name + " healed by " + dam);
                if (Hp + dam < maxHp)
                    Hp += dam;
                else
                    Hp = maxHp;
                break;
        }
        if (Hp <= 0) Destroy(gameObject);
        return damage;
    }
    /// <summary>
    /// takes damage with no elements
    /// </summary>
    /// <param name="dam"></param>
    public void TakeDamage(float dam)
    {
        print(dam + " damage to " + name);
        if (Hp - dam < maxHp)
            Hp -= dam;
        else
            Hp = maxHp;
        if (Hp <= 0) Destroy(gameObject);
    }
    /// <summary>
    /// for spells that hits freindly sodiers, only status effects will take effect with no damage
    /// </summary>
    /// <param name="dam"></param>
    /// <param name="type"></param>
    public void FriendlyFire(float dam, string type)
    {
        
        switch (type)
        {
            case "Life":
                print("lifeeeeee");
                if (Hp + dam < maxHp)
                    Hp += dam;
                else
                    Hp = maxHp;
                break;
            case "Fire":
                Cfire -= dam;
                if (Cfire <= 0)
                {
                    StatusEffect stat = gameObject.AddComponent<Burning>();
                    stat.Timer += -Cfire;
                    Cfire = 0;
                }

                break;
            case "Wind":

                Cwind -= dam;
                if (Cwind <= 0)
                {
                    StatusEffect stat = gameObject.AddComponent<Bleeding>();
                    stat.Timer += -Cwind;
                    Cwind = 0;
                }

                break;
            case "Frost":

                Cfrost -= dam;
                if (Cfrost <= 0)
                {
                    StatusEffect stat = gameObject.AddComponent<Freezing>();
                    stat.Timer += -Cfrost;
                    Cfrost = 0;
                }

                break;
            case "Electricity":

                Celec -= dam;
                if (Celec <= 0)
                {
                    StatusEffect stat = gameObject.AddComponent<Shocking>();
                    stat.Timer += -Celec;
                    Celec = 0;
                }
                break;
        }
    }

}