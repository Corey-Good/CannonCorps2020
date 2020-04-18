/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private PhotonView photonView;
    private Tank tank;
    private PlayerController playerController;
    private UIManager ui;

    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        playerController = GameObject.FindGameObjectWithTag("PlayerGO").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine && collision.gameObject.tag == "Bullet")
        {
            ProjectileDamange d = collision.gameObject.GetComponent<ProjectileDamange>();
            if (playerController.invulnerable)
            {
                playerController.invulnerable = false;
                return;
            }
            else
            {
                if(d != null)
                {
                    tank.damageTaken(d.GetDamage());
                    Debug.Log("Doing " + d.GetDamage() + " damage");
                }
                else
                {
                    tank.damageTaken(10f);
                }
                
                tank.tankHit = true;
            }
        }
    }
}