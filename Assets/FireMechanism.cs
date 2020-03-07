/************************************************************************/
/* Author:             Corey Good                                     */
/* Date Created:       3/5/2020                                       */
/* Last Modified Date: 3/6/2020                                        */
/* Modified By:        Corey Good                                     */
/************************************************************************/
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
}
