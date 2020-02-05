using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCharacter : MonoBehaviour
{
    private Tank tankInstance;
    // Start is called before the first frame update
    void Start()
    {
        tankInstance = GetTankInstance();
    }

    // Update is called once per frame
    void Update()
    {

        //tankInstance.CreateTank("boxTank", Color.red);

        //tankInstance.damageTaken(10.0f);
    }
    public Tank GetTankInstance()
    {
        return GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }
}
