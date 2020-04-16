using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FreezePowerUp : PowerUp
{
    private float rotateMultiplier = 0f;
    private float speedMultiplier = 0f;
    private float Damage = 5.0f;
    private bool isFreezePowerup = true;
    private Player player;
    private PhotonView photonView;
    [Tooltip("Value must not exceed 200!")]
    public float speed = 140f;
    private float dropRate = 0f;

    protected override void Start()
    {
        base.Start();
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) { return; }
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        dropRate += 0.06f * Time.deltaTime;
        transform.position += -transform.up * (dropRate);
    }
    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
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
        playerBrain.SetSpeedBoostOff();
        playerBrain.SetSpeedBoostOn(speedMultiplier, rotateMultiplier, isFreezePowerup);
        playerBrain.SetHealthBoost(Damage);
        StartCoroutine(FreezeTimer());
    }

    IEnumerator FreezeTimer()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetSpeedBoostOff();
        playerBrain.FreezeTank(false);
        base.PowerUpHasExpired();
    }
}
