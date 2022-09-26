using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDetail : MonoBehaviour
{
    public Spell spell;
    Text text;
    string spellList;
    public Transform parent;
    void Start()
    {
        text = GetComponent<Text>();
        foreach(FormalEl el in spell.Felements)
        {
            spellList += el.type + "" + el.level + "\n";
        }
    }


    void Update()
    {
        text.text = spell.Level + "\n" + spellList;
        transform.position = new Vector3(parent.position.x-1, transform.position.y, parent.position.z);
    }
}
