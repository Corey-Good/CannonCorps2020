using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamange : MonoBehaviour
{
    Tank tankInfo;
    public static float damage;
    // Start is called before the first frame update
    void Start()
    {
        tankInfo = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        damage = tankInfo.bulletDamage;
    }

    public float GetDamage()
    {
        return damage;
    }
}
