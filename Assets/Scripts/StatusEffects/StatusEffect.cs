using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{


    public float Timer = 1, InterTimer = 0, Intervals = 0.5f;
    public Life life;

    void Awake()
    { 
        life = GetComponent<Life>();
        InterTimer = Time.time + Intervals;
        Timer += Time.time;
        Begin();
    }
    void Update()
    {
        if (InterTimer <= Time.time)
        {
            Effect();
            InterTimer = Time.time + Intervals;
        }

        if (Timer < Time.time) End();
    }

    /// <summary>
    /// what happens in the beginning of the effect
    /// </summary>
    virtual public void Begin() { }

    /// <summary>
    /// what happens in the end of the effect
    /// </summary>
    virtual public void End()
    {
        Destroy(this);
    }

    /// <summary>
    /// what happens during the effect in intervals 
    /// </summary>
    virtual public void Effect() { }


}
