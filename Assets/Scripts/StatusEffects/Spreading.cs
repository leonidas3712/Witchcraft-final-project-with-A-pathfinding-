using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreading : StatusEffect
{
    /// <summary>
    /// this is the uniqe of the fire(the secondery quality), it spreads the main quality of the dominant element to what he touches
    /// </summary>

    public List<string> DominentEls;
    public int level;
    private void OnCollisionEnter(Collision coll)
    {

        if (!coll.gameObject.GetComponent<Spreading>() && coll.gameObject.GetComponent<Life>())
            foreach (string Dominent in DominentEls)
                if (level / 2 > 2)//if spreading level is high enough
                {
                    switch (Dominent)
                    {
                        case "Fire":
                            Spread(coll.gameObject, coll.gameObject.AddComponent<Burning>(), Dominent);
                            break;
                        case "Wind":
                            Spread(coll.gameObject, coll.gameObject.AddComponent<Bleeding>(), Dominent);
                            break;
                        case "Frost":
                            Spread(coll.gameObject, coll.gameObject.AddComponent<Freezing>(), Dominent);
                            break;
                        case "Electricity":
                            Spread(coll.gameObject, coll.gameObject.AddComponent<Shocking>(), Dominent);
                            break;
                        case "Life":
                            Spread(coll.gameObject, coll.gameObject.AddComponent<Healing>(), Dominent);
                            break;
                    }
                }
    }
    void Spread(GameObject victem, StatusEffect stat, string type)
    {
        stat.Timer += level / 2;
        Spreading spreading = victem.AddComponent<Spreading>();
        spreading.DominentEls.Add(type);
        //lower spread level each spread
        spreading.level = level / 2;
    }

}
