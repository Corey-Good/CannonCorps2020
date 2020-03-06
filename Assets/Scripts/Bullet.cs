using Photon.Pun;
using UnityEngine;

/************************************************************************/
/* Author: Eddie Habal */
/* Date Created: 1/29/2020 */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

public class Bullet : MonoBehaviour
{
    public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    //public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
    public float m_MaxDamage = 25f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
    public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
    public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.

    private Player player;
    private PhotonView photonView;
    public GameObject bullet;
    private TmManager tm;
    float speed = 150f;

    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if(!photonView.IsMine) { return; }
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        //Destroy(gameObject, m_MaxLifeTime);
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (photonView.IsMine && collision.gameObject.tag == "PlayerGO")
        {
            player.ScoreCurrent += 10;
            if (player.gameState == Player.GameState.TB)
            {
                //TmManager.UpdateTeamScores(player.teamCode, 10);
            }
        }
        else
        {
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        PhotonNetwork.Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{


    //    Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

    //    foreach (Collider collider in colliders)
    //    {
    //        PlayerController tankController = collider.GetComponentInParent<PlayerController>();

    //        if (!tankController)
    //            continue;

    //        if (photonView.IsMine) 
    //        {
    //            player.ScoreCurrent += 10; 
    //        }
    //        else
    //        {
    //            //float damage = CalculateDamage(collider.GetComponent<Rigidbody>().position);

    //            //tankController.DealDamage(damage);
    //            Debug.Log("Taking away health!!!");
    //        }


    //    }

    // // Go through all the colliders...
    // for (int i = 0; i < colliders.Length; i++)
    // {
    //     // ... and find their rigidbody.
    //     Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

    //     // If they don't have a rigidbody, go on to the next collider.
    //     if (!targetRigidbody)
    //         continue;

    //     // Add an explosion force.
    //     targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

    //     // Find the TankHealth script associated with the rigidbody.
    //     PlayerController targetHealth = targetRigidbody.GetComponent<PlayerController>();

    //     // If there is no TankHealth script attached to the gameobject, go on to the next collider.
    //     if (!targetHealth)
    //         continue;

    //     // Calculate the amount of damage the target should take based on it's distance from the shell.
    //     float damage = CalculateDamage(targetRigidbody.position);

    //     // Deal this damage to the tank.
    //     targetHealth.DealDamage(damage);
    // }

    // // Unparent the particles from the shell.
    // //m_ExplosionParticles.transform.parent = null;

    // // Play the particle system.
    //// m_ExplosionParticles.Play();

    // // Play the explosion sound effect.
    // if(m_ExplosionAudio != null)
    // { m_ExplosionAudio.Play(); }


    // // Once the particles have finished, destroy the gameobject they are on.
    //// Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

    // // Destroy the shell.

}
