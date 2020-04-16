using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaserPowerUp : PowerUp
{
    private float damage = 5f;
    private Player player;
    private PhotonView photonView;
    [Tooltip("Value must not exceed 200!")]
    private float speed = 300;

    protected override void Start()
    {
        base.Start();
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) { return; }
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    // Move the bullet forward until it hits an object
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetHealthBoost(damage);
        if (photonView.IsMine)
        {
            // Update the player's score
            player.ScoreCurrent += 10;
            player.gotPoints = true;

            // Show the points to the player
            //UIManager.ShowPoints();

            // Add points to the player's team score
            if (player.gameState == Player.GameState.TB)
            {
                //TmManager.UpdateTeamScores(player.teamCode, 10);
            }
        }
        PowerUpHasExpired();
    }


    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        base.PowerUpHasExpired();
    }
}
