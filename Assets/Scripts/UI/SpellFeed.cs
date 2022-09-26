using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellFeed : MonoBehaviour
{
    Text text;
    public spellcaster sp;
    void Start()
    {
        text = GetComponent<Text>();
    }
    private void Awake()
    {
        transform.parent = GameObject.Find("Canvas").transform;
    }

    void Update()
    {
        text.text = "";
        foreach(FormalEl el in sp.el)
        {
            text.text += el.type + " " + el.level + "\n";
        }
    }
}
