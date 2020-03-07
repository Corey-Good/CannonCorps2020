/************************************************************************/
/* Author: Eddie Habal */
/* Date Created: 1/29/2020 */
/* Last Modified Date: 3/6/2020*/
/* Modified By: Corey Good*/
/************************************************************************/
using Photon.Pun;
using UnityEngine;

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
    [Tooltip("Value must not exceed 200!")]
    public float speed = 150f;

    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if(!photonView.IsMine) { return; }
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    // Move the bullet forward until it hits an object
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Give the player points for landing a hit
        if (photonView.IsMine && collision.gameObject.tag == "PlayerGO")
        {
            // Update the player's score
            player.ScoreCurrent += 10;

            // Show the points to the player
            //UIManager.ShowPoints();

            // Add points to the player's team score
            if (player.gameState == Player.GameState.TB)
            {
                //TmManager.UpdateTeamScores(player.teamCode, 10);
            }
        }
        else
        {
            // Destroy the bullet if hits any other object
            PhotonNetwork.Destroy(gameObject);

            // Spawn in the particle system
        }
        
    }
}
