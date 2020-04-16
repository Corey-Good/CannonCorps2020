using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNet : MonoBehaviour
{
    public static bool FixTankPosition = false;
    public int count = 0;

    private void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log(collisionInfo.gameObject.tag);
        if(collisionInfo.gameObject.tag == "PlayerGO")
            count++;
        if(count > 3)
        {
            FixTankPosition = true;
            count = 0;
        }      

    }

}