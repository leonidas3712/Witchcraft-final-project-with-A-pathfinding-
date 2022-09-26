using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //the life that will be represented on the bar
    public Life life;
    Text text;
    //the entity the bar should follow
    Transform parent;
    void Start()
    {
        text = GetComponent<Text>();
        parent = life.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        if (life.Hp > 0)
            text.text = "HP: " + life.Hp;
        transform.position = new Vector3(parent.position.x, transform.position.y, parent.position.z - 1);
    }
}
