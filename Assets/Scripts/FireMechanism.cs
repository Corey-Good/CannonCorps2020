using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireMechanism : MonoBehaviour
{
    public float bulletForce;
    public Camera tankCamera;

    public bool ReceivePlayerControllerClick(bool readyToFire, PlayerController.BulletType currentBulletType)
    {
        if(readyToFire)
        {
            switch (currentBulletType)
            {
                case PlayerController.BulletType.Normal:
                    return FireBullet();
                case PlayerController.BulletType.FreezeBullet:
                    return FireFreezeBullet();
                default:
                    return true;
            }
        }
        else
        {
            return true;
        }

    }

    public bool FireBullet()
    { 
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", gameObject.transform.position, rotation);
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool FireFreezeBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("FreezeBullet", gameObject.transform.position, rotation);
            return false;
        }
        else
        {
            return true;
        }
    }
}
