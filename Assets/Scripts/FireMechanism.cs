using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireMechanism : MonoBehaviour
{
    public float bulletForce;
    public Camera tankCamera;

    public void FireBullet()
    { 
        RaycastHit hit;

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit))
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", gameObject.transform.position, rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //GameObject bullet = PhotonNetwork.Instantiate("FreezeBullet", gameObject.transform.position, gameObject.transform.rotation);
        //bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bulletForce);
        //tankCamera.transform.Rotate(Input.mouseScrollDelta[1] * -50 * Time.deltaTime, 0, 0f);

    }
}
