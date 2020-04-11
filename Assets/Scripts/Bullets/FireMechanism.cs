/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using UnityEngine;

public class FireMechanism : MonoBehaviour
{
    public float bulletForce;
    public Camera tankCamera;
    private Tank tank;

    private void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }

    public void ReceivePlayerControllerClick(PlayerController.BulletType currentBulletType)
    {
        RaycastHit hit;
        string bulletType = GetBullet(currentBulletType);

        if (Physics.Raycast(tankCamera.transform.position, tankCamera.transform.forward, out hit) && ValidAim())
        {
            Vector3 direction = hit.point - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            PhotonNetwork.Instantiate(bulletType, gameObject.transform.position, rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(bulletType, gameObject.transform.position, gameObject.transform.rotation);
        }
    } 

    private string GetBullet(PlayerController.BulletType currentBulletType)
    {
        string bullet;
        switch (currentBulletType)
        {
            case PlayerController.BulletType.Normal:
                bullet = tank.tankProjectile;
                break;

            case PlayerController.BulletType.FreezeBullet:
                bullet = "FreezeBullet";
                break;

            case PlayerController.BulletType.DynamiteBullet:
                bullet = "DynamiteBullet";
                break;

            case PlayerController.BulletType.LaserBullet:
                bullet = "LaserBullet";
                break;
            default:
                bullet = tank.tankProjectile;
                break;
        }

        return bullet;
    }

    private bool ValidAim()
    {
        float angle = tankCamera.transform.rotation.eulerAngles.x;
        return angle < 6.0f || (angle > 180.0f && angle < 360.0f);
    }
}