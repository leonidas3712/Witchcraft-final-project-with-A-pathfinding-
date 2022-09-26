using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellcaster : MonoBehaviour
{
    bool casting = false;
    public List<FormalEl> el = new List<FormalEl>();
    public Weapon wep;

    private void Awake()
    {
        GameObject spellFeed = (GameObject)Instantiate(Resources.Load("SpellFeed"), GameObject.Find("Canvas").transform);
        spellFeed.GetComponent<SpellFeed>().sp = this;
    }
    void Update()
    {
        Rotate();
        Pick();
        /*if (el.Count > 0)
            wep.Action(el);*/
    }
    void Rotate()
    {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(mouse.x - transform.position.x, mouse.z - transform.position.z) * Mathf.Rad2Deg, 0);
        
    }
    /// <summary>
    /// use player input to construct a list of elements to make a spell with
    /// </summary>
    void Pick()
    {
        string t;
        if (Input.GetMouseButtonDown(0) && !casting)
            casting = true;
        if (Input.GetKeyDown("a"))
        {
            t = "Fire";
            if (casting)
            {
                el.Clear();
                casting = false;
            }
            foreach (FormalEl fel in el)
            {
                if (fel.type == t)
                {
                    fel.level += 1;
                    return;
                }
            }
            el.Add(new FormalEl(t, 1));
        }
        if (Input.GetKeyDown("s"))
        {
            t = "Electricity";
            if (casting)
            {
                el.Clear();
                casting = false;
            }
            foreach (FormalEl fel in el)
            {
                if (fel.type == t)
                {
                    fel.level += 1;
                    return;
                }
            }
            el.Add(new FormalEl(t, 1));
        }
        if (Input.GetKeyDown("d"))
        {
            t = "Wind";
            if (casting)
            {
                el.Clear();
                casting = false;
            }
            foreach (FormalEl fel in el)
            {
                if (fel.type == t)
                {
                    fel.level += 1;
                    return;
                }
            }
            el.Add(new FormalEl(t, 1));
        }
        if (Input.GetKeyDown("f"))
        {
            t = "Frost";
            if (casting)
            {
                el.Clear();
                casting = false;
            }
            foreach (FormalEl fel in el)
            {
                if (fel.type == t)
                {
                    fel.level += 1;
                    return;
                }
            }
            el.Add(new FormalEl(t, 1));
        }
        if (Input.GetKeyDown("e"))
        {
            t = "Life";
            if (casting)
            {
                el.Clear();
                casting = false;
            }
            foreach (FormalEl fel in el)
            {
                if (fel.type == t)
                {
                    fel.level += 1;
                    return;
                }
            }
            el.Add(new FormalEl(t, 1));
        }

    }

}
//Input.mousePosition.z - cam.transform.position.z)


