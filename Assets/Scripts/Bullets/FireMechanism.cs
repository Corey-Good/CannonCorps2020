﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireMechanism : MonoBehaviour
{
    public float bulletForce;
    public Camera tankCamera;
    private Tank tank;

    private void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }
    public void ReceivePlayerControllerClick(bool readyToFire, PlayerController.BulletType currentBulletType)
    {
        if(readyToFire)
        {
            switch (currentBulletType)
            {
                case PlayerController.BulletType.Normal:
                    FireBullet();
                    break;
                case PlayerController.BulletType.FreezeBullet:
                    FireFreezeBullet();
                    break;
                case PlayerController.BulletType.DynamiteBullet:
                    FireDynamiteBullet();
                    break;
                case PlayerController.BulletType.LaserBullet:
                    FireLaserBullet();
                    break;                    
            }
        }


    }

    public void FireBullet()
    { 
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate(tank.tankProjectile, gameObject.transform.position, rotation);
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate(tank.tankProjectile, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    public void FireDynamiteBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("DynamiteBullet", gameObject.transform.position, rotation);
            
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate("DynamiteBullet", gameObject.transform.position, gameObject.transform.rotation);
        }
    }
    public void FireLaserBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("LaserBullet", gameObject.transform.position, rotation);
            
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate("LaserBullet", gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    public void FireFreezeBullet()
    {
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("FreezeBullet", gameObject.transform.position, rotation);
            
        }
        else
        {
            GameObject bullet = PhotonNetwork.Instantiate("FreezeBullet", gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
