using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class ClientHandler : MonoBehaviour
{
    //the client
    TcpClient client;
    //the thread in which the connection will start
    Thread thread;
    BinaryReader reader;
    BinaryWriter writer;
    //elements to send the server for a spell
    public List<FormalEl> el;
    Camera cam;
    //the object that represents the player, the enemy wizard, the empty objects that contain dynamic objects
    GameObject caster, enemy, mapobjects, spells, creatures;
    bool gamestarted;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mapobjects = GameObject.Find("mapobjects");
        spells = GameObject.Find("spells");
        creatures = GameObject.Find("creatures");

        thread = new Thread(new ThreadStart(conectionThread));
        thread.IsBackground = true;
        thread.Start();
    }

    /// <summary>
    /// the thread that handles staarting the connection
    /// </summary>
    void conectionThread()
    {
        client = new TcpClient();
        client.Connect("127.0.0.1", 123);
        reader = new BinaryReader(client.GetStream());
        writer = new BinaryWriter(client.GetStream());
    }

    private void Update()
    {
        if (client != null && client.Connected)
        {
            if (!gamestarted)
            {
                //waites for the game to start
                gamestarted = reader.ReadBoolean();
                if (gamestarted)
                {
                    Vector3 location;
                    location = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    caster = (GameObject)Instantiate(Resources.Load("Caster"), location, mapobjects.transform.rotation, mapobjects.transform);
                    caster.name = "Caster";
                    location = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    enemy = (GameObject)Instantiate(Resources.Load("Caster_Enemy"), location, mapobjects.transform.rotation, mapobjects.transform);
                    enemy.name = "Caster2";
                    cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, reader.ReadSingle(), cam.transform.eulerAngles.z);
                    el = caster.GetComponent<spellcaster>().el;
                }
            }
            else
            {
                //read a boolean that will point whether the game was over or not
                if (reader.ReadBoolean())
                {
                    //if the last read was true then read another boolean that will point whether the player has won or not
                    if (reader.ReadBoolean())
                    {
                        SceneManager.LoadScene("youWon");
                    }
                    else
                    {
                        SceneManager.LoadScene("youLose");
                    }
                }
                
                syncGame();
                sendSpell();
            }
        }
        else
        {
            if(gamestarted) SceneManager.LoadScene("mainMenu");
        }
    }

    /// <summary>
    /// sends a list of formal elements and mouse postion and input to server
    /// </summary>
    void sendSpell()
    {
        //the length of the element list
        writer.Write(el.Count);
        foreach (FormalEl element in el)
        {
            writer.Write(element.type);
            writer.Write(element.level);
        }
        Vector3 mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));
        //the mouse position
        writer.Write(mouse.x);
        writer.Write(mouse.y);
        writer.Write(mouse.z);

        //writes whether the mouse was clicked or not
        if (Input.GetMouseButtonDown(0))
            writer.Write(true);
        else writer.Write(false);
    }

    /// <summary>
    /// read information about dynamic parts of the game from the server
    /// </summary>
    void syncGame()
    {
        #region mapObjectsSync
        float hp = reader.ReadSingle();
        if (hp == null) return;
        //these lists will be used to check if there are any objects that has been destroyed on the server
        List<string> objectsNames = new List<string>();
        List<string> spellsNames = new List<string>();
        List<string> creatursNames = new List<string>();
        int length;

        caster.GetComponent<Life>().Hp = hp;
        enemy.GetComponent<Life>().Hp = reader.ReadSingle();

        length = reader.ReadInt32();
        for (int i = 0; i < length; i++)
        {
            string objectName = reader.ReadString();
            objectsNames.Add(objectName);

            //if the object doesnt exist on the map then spawn it 
            GameObject mapobject = GameObject.Find(objectName);
            if (mapobject)
            {
                reader.ReadString();
                reader.ReadSingle();
            }
            else
            {
                mapobject = (GameObject)Instantiate(Resources.Load(reader.ReadString()), mapobjects.transform);//
                mapobject.name = objectName;
                float scale = reader.ReadSingle();//
                mapobject.transform.localScale = new Vector3(scale, scale, scale);
            }
            Vector3 newPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());//
            mapobject.transform.position = newPosition;
            mapobject.transform.eulerAngles = new Vector3(0, reader.ReadSingle(), 0);//
            mapobject.GetComponent<Life>().Hp = reader.ReadSingle();//
            mapobject.tag = reader.ReadString();//
            reader.ReadUInt32();//animation state
        }
        //if an object on the map doesnt exist on the list that has been sent from the server then destroy it
        Transform[] objectsInScene = mapobjects.GetComponentsInChildren<Transform>();
        for (int i = 0; i < objectsInScene.Length; i++)
        {
            bool found = false;
            if (objectsInScene[i].gameObject != caster && objectsInScene[i].gameObject != enemy && objectsInScene[i].gameObject != mapobjects)
            {
                foreach (string name in objectsNames)
                {
                    if (objectsInScene[i].name == name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Destroy(objectsInScene[i].gameObject);
                }
            }

        }
        #endregion
        //////////////////////////////////////////////////////////spells sync
        #region spellSync
        length = reader.ReadInt32();
        for (int i = 0; i < length; i++)
        {
            string objectName = reader.ReadString();
            spellsNames.Add(objectName);
            GameObject mapobject = GameObject.Find(objectName);
            if (mapobject)
            {
                reader.ReadString();
                reader.ReadSingle();
            }
            else
            {
                mapobject = (GameObject)Instantiate(Resources.Load(reader.ReadString()), spells.transform);
                mapobject.name = objectName;
                float scale = reader.ReadSingle();
                mapobject.transform.localScale = new Vector3(scale, scale, scale);
            }

            Vector3 newPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            //float dis = Vector3.Distance(newPosition, caster.transform.position);//might use velocity to interpolate
            mapobject.transform.position = newPosition;
            mapobject.transform.eulerAngles = new Vector3(reader.ReadSingle(), 0, 0);
        }
        Transform[] spellsInScene = spells.GetComponentsInChildren<Transform>();
        for (int i = 0; i < spellsInScene.Length; i++)
        {
            Transform sp = spellsInScene[i];
            bool found = false;
            if (sp.gameObject != spells)
            {
                foreach (string name in spellsNames)
                {
                    if (sp.name == name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Destroy(sp.gameObject);
                    print("projectile got destroyed");
                }
            }

        }
        #endregion
        //////////////////////////////////////////////////////////creatures sync!!!!
        #region creaturesSync
        length = reader.ReadInt32();
        for (int i = 0; i < length; i++)
        {
            string creatureName = reader.ReadString();
            creatursNames.Add(creatureName);
            GameObject creature = GameObject.Find(creatureName);
            if (creature)
            {
                reader.ReadString();//
                reader.ReadSingle();//
            }
            else
            {
                creature = (GameObject)Instantiate(Resources.Load(reader.ReadString()), creatures.transform);//
                creature.name = creatureName;
                float scale = reader.ReadSingle();//
                creature.transform.localScale = new Vector3(scale, scale, scale);
            }
            Vector3 newPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());//
            //float dis = Vector3.Distance(newPosition, caster.transform.position);//might use velocity to interpolate
            creature.transform.position = newPosition;
            creature.transform.eulerAngles = new Vector3(0, reader.ReadSingle(), 0);//
            creature.GetComponent<Life>().Hp = reader.ReadSingle();//
            creature.tag = reader.ReadString();//
            reader.ReadUInt32();//animation state
        }
        Transform[] creatursInScene = creatures.GetComponentsInChildren<Transform>();
        for (int i = 0; i < creatursInScene.Length; i++)
        {
            Transform cr = creatursInScene[i];
            bool found = false;
            if (cr.gameObject != creatures)
            {
                foreach (string name in creatursNames)
                {
                    if (cr.name == name)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Destroy(cr.gameObject);
                    print("projectile got destroyed");
                }
            }

        }
        #endregion
    }
}
