using Photon.Pun;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private PhotonView photonView;
    private Tank tank;
    private UIManager ui;
    public bool shieldBoostOn;

    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }

    private void OnCollisionEnter (Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(photonView.IsMine && collision.gameObject.tag == "Bullet")
        {
            if (shieldBoostOn)
            {
                shieldBoostOn = false;
                return;
            }
            else
            {
                tank.damageTaken(10f);
                tank.tankHit = true;
            }
            

        }
    }
}
