using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform t;
    Vector3 mousePosition;
    public Camera cam;
    float a;
    Quaternion rot;

    void Update()
    {

        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));

        //Rotates toward the mouse
        a = Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        if (a < 50 && a > -50)
            GetComponent<Transform>().eulerAngles = new Vector3(0, 0, a);
        else
        {
            if (a > 50 || a < -180)
                GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 50);

            if (a < -50 && a > -180)
                GetComponent<Transform>().eulerAngles = new Vector3(0, 0, -50);

        }







    }

}

