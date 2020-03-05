using Photon.Pun;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private PhotonView photonView;
    private Tank tank;
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
            tank.healthCurrent -= 10f;
        }
    }
}
