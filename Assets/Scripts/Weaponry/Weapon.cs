using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float speed = 5;
    Vector3 mouse;
    public Camera cam;
    public static int counter = 0;

    /// <summary>
    /// for a single player version of the game
    /// </summary>
    /// <param name="elements"></param>
    public void Projectile(List<FormalEl> elements)
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));
            GameObject pro = (GameObject)Instantiate(Resources.Load("Projectile"), transform.position + transform.forward, transform.rotation);
            pro.transform.rotation = Quaternion.Euler(90, 0, 0);
            Spell spell = pro.AddComponent<Spell>();
            spell.Create(elements);
            pro.AddComponent<Projectile>();
            mouse = (mouse - transform.position);
            mouse = HelpfulFuncs.Norm1(mouse);
            mouse *= speed;
            pro.GetComponent<Rigidbody>().velocity = new Vector3(mouse.x, 0, mouse.z);

        }

    }
    /// <summary>
    /// shoot a projectile for the client from server
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="mouseDir"></param>
    /// <param name="side"> used to determine for which player this projectile work for</param>
    public void Projectile(List<FormalEl> elements, Vector3 mouseDir,string side)
    {
        mouse = mouseDir;
        GameObject pro = (GameObject)Instantiate(Resources.Load("Projectile"), transform.position + transform.forward, transform.rotation);
        pro.name = pro.name + counter;
        counter++;
        
        pro.transform.rotation = Quaternion.Euler(90, 0, 0);
        Spell spell = pro.AddComponent<Spell>();
        spell.Create(elements);
        pro.AddComponent<Projectile>();
        pro.GetComponent<Projectile>().side = side;
        mouse = (mouse - transform.position);
        mouse = HelpfulFuncs.Norm1(mouse);
        mouse *= speed;
        pro.GetComponent<Rigidbody>().velocity = new Vector3(mouse.x, 0, mouse.z);
    }

}
