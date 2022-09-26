using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matter : MonoBehaviour
{

    public string type;
    public int Mass;
    [SerializeField]
    protected Rigidbody rig;
    public TargetPicker picker;//will be created in a public function that will be called when creating a creature

    public float DPS, AttackRange;
    /// <summary>
    /// gets a game object and calculate a Target
    /// </summary>
    /// <param name="foe"></param>
    /// <returns> a Target that fit the range of the attacker</returns>
    public Target GameObjectToTarget(GameObject foe)
    {
        Vector3 targetdir = HelpfulFuncs.Norm1(foe.transform.position - transform.position);
        Vector3 targetedPosition = transform.position;
        if (Vector3.Distance(transform.position, foe.transform.position) > AttackRange)
            targetedPosition = transform.position + targetdir * (Vector3.Distance(foe.transform.position, transform.position) - AttackRange - foe.GetComponent<Collider>().bounds.size.x / 2 + 0.3f);//maybe with a little offset
        return new Target(targetdir, targetedPosition, this, foe);
    }

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// sets the creature target picker
    /// </summary>
    public virtual void creatureIntial()
    {

    }
    public virtual void Reaction()
    {

        //אולי בכלל להשתמש רק באלמנט
    }
    /// <summary>
    /// the attack of the matter. the way that a creature with this matter as a dominant matter will attack 
    /// </summary>
    /// <param name="targetPos">the target location</param>
    /// <param name="spell">the spell that is being used to attack</param>
    public virtual void Damage(Vector3 targetPos, Spell spell)
    {

    }
    /// <summary>
    /// the movment of the matter
    /// </summary>
    /// <param name="speed">the speed in which the matter will move</param>
    /// <param name="location">the location the matter will travel twards</param>
    public virtual void Move(float speed, Vector3 location)
    {

    }
    public virtual void Stop()
    {
        if (rig)
            rig.velocity = Vector3.zero;
    }
    public virtual void Damage(Vector3 targetPos, Spell spell, string side)
    { }
    public virtual void PostDeath() { }

}
