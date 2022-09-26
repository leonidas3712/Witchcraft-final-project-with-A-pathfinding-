using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour
{
    TcpClient client1, client2;
    Thread thread;
    //a binary reader for each client
    BinaryReader reader1, reader2;
    //a binary writer for each client
    BinaryWriter writer1, writer2;
    //game objects that stand for the players
    GameObject caster1, caster2;
    bool gamestarted;

    private void Start()
    {
        caster1 = GameObject.Find("Caster");
        caster2 = GameObject.Find("Caster2");

        thread = new Thread(new ThreadStart(conectionThread));
        thread.IsBackground = true;
        thread.Start();
    }
    /// <summary>
    /// the thread creates a listner(server) wait for th econnection of two clients
    /// </summary>
    void conectionThread()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 123);
        listener.Start();

        client1 = listener.AcceptTcpClient();
        reader1 = new BinaryReader(client1.GetStream());
        writer1 = new BinaryWriter(client1.GetStream());
        print("client 1 has connected");

        client2 = listener.AcceptTcpClient();
        reader2 = new BinaryReader(client2.GetStream());
        writer2 = new BinaryWriter(client2.GetStream());
        print("client 2 has connected");
        //give the getgo to the game here!!!!
    }
    /// <summary>
    /// update will run the active part of the server in data transmisions
    /// </summary>
    void Update()
    {
        //stalls the game until both player are connected
        if (client1 != null && client1.Connected && !gamestarted&&writer1!=null)
        {
            writer1.Write(false);
        }
        if (client2 != null && client2.Connected && !gamestarted && writer2 != null)
        {
            writer2.Write(false);
        }
        
        if (client1 != null && client2 != null && client1.Connected && client2.Connected&&reader1!=null&&reader2!=null)
        {
            //when both clients are connected the game will start
            if (!gamestarted)
            {
                //tells the clients that the game is starting
                writer1.Write(true);
                writer2.Write(true);
                gamestarted = true;
                //tells the first client where should he locate himself
                writer1.Write(caster1.transform.position.x);
                writer1.Write(caster1.transform.position.y);
                writer1.Write(caster1.transform.position.z);
                //tells the fist client where his enemy should be located
                writer1.Write(caster2.transform.position.x);
                writer1.Write(caster2.transform.position.y);
                writer1.Write(caster2.transform.position.z);
                //tells the first client how his camera should be located
                writer1.Write(0f);

                // vise versa
                writer2.Write(caster2.transform.position.x);
                writer2.Write(caster2.transform.position.y);
                writer2.Write(caster2.transform.position.z);

                writer2.Write(caster1.transform.position.x);
                writer2.Write(caster1.transform.position.y);
                writer2.Write(caster1.transform.position.z);
                writer2.Write(180f);
                return;
            }
            //cheacks if the game has ended and someone has died
            if (caster1 == null)
            {
                //the first bool is for whether the game had finished or not
                writer1.Write(true);
                //the second bool is for telling whether the player has won or lost
                writer1.Write(false);

                writer2.Write(true);
                writer2.Write(true);
                SceneManager.LoadScene("mainMenu");
            }
            if(caster2 == null)
            {
                writer1.Write(true);
                writer1.Write(true);

                writer2.Write(true);
                writer2.Write(false);
                SceneManager.LoadScene("mainMenu");
            }
            //if the game hasent finished then only the first bool is to be sent
            writer1.Write(false);
            writer2.Write(false);

            syncClient(writer1, 1);
            syncClient(writer2, 2);
            castSpell(reader1, caster1);
            castSpell(reader2, caster2);
            
        }
        else
        {
            if (gamestarted)
            {
                thread.Abort();
                SceneManager.LoadScene("mainMenu");
            }
        }
    }
    /// <summary>
    /// cast a spell when the client clicks the left mouse button
    /// </summary>
    /// <param name="reader">the reader of the client that casts the spell</param>
    /// <param name="caster">the caster in the scene the stands for the client that casts the spell</param>
    void castSpell(BinaryReader reader, GameObject caster)
    {
        List<FormalEl> elList = new List<FormalEl>();
        int length = reader.ReadInt32();
        for (int i = 0; i < length; i++)
        {
            elList.Add(new FormalEl(reader.ReadString(), reader.ReadInt32()));
        }
        Vector3 mouseDir = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        //cast!!
        if (reader.ReadBoolean()) caster.GetComponent<Weapon>().Projectile(elList, mouseDir, caster.tag);
    }
    /// <summary>
    /// sends the client information about the dynamic parts of the game:
    /// map objects, creatures , hp,spells
    /// </summary>
    /// <param name="writer">the writer of the client the information is beining sent to </param>
    /// <param name="whichcaster">which client between the two the function refer two beacues itll change the oeder some of the information will be sent in</param>
    void syncClient(BinaryWriter writer, int whichcaster)
    {
        if (whichcaster == 1)
        {
            writer.Write(caster1.GetComponent<Life>().Hp);
            writer.Write(caster2.GetComponent<Life>().Hp);
        }
        else
        {
            writer.Write(caster2.GetComponent<Life>().Hp);
            writer.Write(caster1.GetComponent<Life>().Hp);
        }
        GameObject mo = GameObject.Find("mapobjects");
        Transform[] mapObjects = mo.GetComponentsInChildren<Transform>();
        //-2 because them casters are also in mapobjects but is being treated seperatly
        if (mapObjects.Length - 3 < 0)
            writer.Write(0);
        else
            writer.Write(mapObjects.Length - 3);
        foreach (Transform ob in mapObjects)
        {
            if (ob.gameObject != caster1 && ob.gameObject != caster2 && ob.gameObject != mo)
            {
                //the name the of the object
                writer.Write(ob.name);
                //the prefab that will be used to represent the object in the client side
                writer.Write(ob.GetComponent<Life>().clientPreFabName);
                //the size of the object
                writer.Write(ob.localScale.x);
                //the postion of the object in three floats
                writer.Write(ob.transform.position.x);
                writer.Write(ob.transform.position.y);
                writer.Write(ob.transform.position.z);
                //
                //the y angle of the object
                writer.Write(ob.transform.eulerAngles.y);
                //the current health points of the object
                writer.Write(ob.GetComponent<Life>().Hp);
                //the tag of the object
                writer.Write(ob.tag);
                //the animation state of the object
                writer.Write(0);//animation state
            }
        }

        GameObject sp = GameObject.Find("spells");
        Transform[] spells = sp.GetComponentsInChildren<Transform>();
        writer.Write(spells.Length - 1);
        foreach (Transform spell in spells)
        {
            if (spell.gameObject != sp)
            {
                //the name of the spell
                writer.Write(spell.name);
                //the prefab that will be used to represent the spell in the client side
                writer.Write("Projectile");//notice meeeee senpaiiii!!!!
                //the size of the spell
                writer.Write(spell.localScale.x);
                //the position of the spell
                writer.Write(spell.transform.position.x);
                writer.Write(spell.transform.position.y);
                writer.Write(spell.transform.position.z);
                //
                //the angle of the spell
                writer.Write(spell.transform.eulerAngles.x);
            }
        }
        GameObject cr = GameObject.Find("creatures");
        Transform[] creartures = cr.GetComponentsInChildren<Transform>();
        writer.Write(creartures.Length - 1);
        foreach (Transform creature in creartures)
        {
            if (creature.gameObject != cr)
            {
                writer.Write(creature.name);
                writer.Write(creature.GetComponent<Life>().clientPreFabName);//notice meeeee senpaiiii!!!!
                writer.Write(creature.localScale.x);
                writer.Write(creature.transform.position.x);
                writer.Write(creature.transform.position.y);
                writer.Write(creature.transform.position.z);
                writer.Write(creature.transform.eulerAngles.y);
                writer.Write(creature.GetComponent<Life>().Hp);
                writer.Write(creature.tag);
                writer.Write(0);//animation state
            }
        }
    }
}
