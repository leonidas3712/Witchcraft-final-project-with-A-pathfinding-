using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realfuckingSize : MonoBehaviour
{
    public static float hight, width,OriginX,OriginZ,ArrayHight,ArrayWidth;
    void Awake()
    {
        width = GetComponent<Collider>().bounds.size.x;
        hight = GetComponent<Collider>().bounds.size.z;
        OriginX = transform.position.x - width / 2;
        OriginZ = transform.position.z - hight / 2;
        Collider sample = GameObject.Find("cubicsample").GetComponent<Collider>();
        ArrayWidth = width / sample.bounds.size.x;
        ArrayHight = hight / sample.bounds.size.z;
    }

    public static Vector3 WorldToMapPosition(Vector3 location)
    {
        return new Vector3(location.x-OriginX,0,location.z-OriginZ);
    }
    public static Vector2 WorldToArrayPostion(Vector3 location)
    {
        Vector3 mapPos = WorldToMapPosition(location);
        return new Vector2((int)(mapPos.x/width*ArrayWidth),(int)(mapPos.z/hight*ArrayHight));
    }
    public static Vector2 WorldToArrayOriginPos(GameObject go)
    {
        float width = go.GetComponent<Collider>().bounds.size.x;
        float hight = go.GetComponent<Collider>().bounds.size.z;
        return WorldToArrayPostion(new Vector3(go.transform.position.x - width / 2,0,go.transform.position.z - hight / 2));
    }

    public static Vector3 ArrayToWorldPosition(int x,int z)
    {
        Vector3 mapPosition = new Vector3(x*width/ArrayWidth,0,z*hight/ArrayHight);
        return new Vector3(mapPosition.x + OriginX, 0, mapPosition.z + OriginZ);
    }
}
