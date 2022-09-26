using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Element {

    void Start()
    {
        type = "Wind";
        //parti = (GameObject)Instantiate(Resources.Load("Fire"), transform);
        //parti.transform.position = transform.position;
    }



   
    public override void Effect(Life life)
    {
        Vector3 dir = life.transform.position - transform.position;
        dir.Normalize();
        dir*=level;
        life.GetComponent<Rigidbody>().AddForce(dir);
        print("bulbul " + dir);
    }
}
