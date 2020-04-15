using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNet : MonoBehaviour
{
    public static bool FixTankPosition = false;
    public static Vector3 Location;

    private void OnCollisionEnter(Collision collisionInfo)
    {
        //collisionInfo.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 1) * 250, ForceMode.Impulse);
        FixTankPosition = true;
        Location = collisionInfo.gameObject.transform.position;

    }

}