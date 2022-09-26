using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class WeaponTamplate : ScriptableObject {

    public new string name;
    public GameObject it;
    public List<Cast> casts;
}
