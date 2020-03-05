using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireMechanism : MonoBehaviour
{
    public float bulletForce;

    public void FireBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Bullet", gameObject.transform.position, gameObject.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletForce);
    }
}
