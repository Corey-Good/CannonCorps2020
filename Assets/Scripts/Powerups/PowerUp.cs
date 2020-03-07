using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpName;
    public string powerUpExplanation;
    public string powerUpQuote;
    [Tooltip("Tick true for power ups that are instant use, eg a health addition that has no delay before expirint")]
    public bool expiresImmediately;
    public GameObject specialEffect;
    public AudioClip soundEffect;
    Vector3 powerUpOffset = new Vector3(0.0f, -20.0f, 0.0f);
    ///<summary>
    ///Keep a reference to the player that collected
    ///</summary>
    protected PlayerController playerBrain;
    //Should PlayerBrain just be mashed in with Player?

    public MeshRenderer powerUpMeshRenderer;

    protected enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }

    protected PowerUpState powerUpState;

    protected virtual void Awake()
    {
        powerUpMeshRenderer = GetComponent<MeshRenderer>();
    }

    protected virtual void Start()
    {
        powerUpState = PowerUpState.InAttractMode;
    }

    ///<summary>
    /// 3D Support
    /// </summary>

    protected virtual void OnTriggerEnter (Collider other)
    {
        PowerUpCollected(other.gameObject);
    }

    protected virtual void PowerUpCollected (GameObject gameObjectCollectingPowerUp)
    {
        // We only care if we've been collected by the player
        if (gameObjectCollectingPowerUp.tag != "PlayerGO")
        {
            return;
        }

        // We only care if we've not been collected before
        if (powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }
        powerUpState = PowerUpState.IsCollected;

        // We must have been collected by a player, store handle to player for later use
        playerBrain = gameObjectCollectingPowerUp.GetComponent<PlayerController>();


        // We move the power up game object to be under the player that collected it, this isn't essential for functionality
        // but is neater in the gameObject hierarchy
        gameObject.transform.parent = playerBrain.gameObject.transform;
        gameObject.transform.position = playerBrain.gameObject.transform.position + powerUpOffset;

        // Collection effects
        PowerUpEffects();

        // Deliver the payload
        PowerUpPayload();

        // Send message to any listeners
        //foreach (GameObject go in EventListeners.main.listeners)
        //{
        //    ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpCollected(this, playerBrain));
        //}

        // Now the power up visuals can go away
        powerUpMeshRenderer.enabled = false;
    }

    protected virtual void PowerUpEffects()
    {
        if (specialEffect != null)
        {
            Instantiate(specialEffect, transform.position, transform.rotation, transform);
        }

        //if (soundEffect != null)
        //{
        //    MainGameController.main.PlaySound(soundEffect);
        //}
    }

    protected virtual void PowerUpPayload()
    {
        Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);
        
        // If we're instant use we also expire self immediately
        if(expiresImmediately)
        {
            PowerUpHasExpired();
        }
    }

    protected virtual void PowerUpHasExpired()
    {
        if(powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }
        powerUpState = PowerUpState.IsExpiring;

        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners)
        //{
        //    ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpExpired(this, playerBrain));
        //}
        Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
        DestroySelfAfterDelay();
    }

    protected virtual void DestroySelfAfterDelay()
    {
        // Arbitrary delay of some seconds to allow particle, audio is all done
        Destroy(gameObject, 10f);
    }

    protected void StartListening(GameObject gameObjectListen)
    {
        EventSystemListeners.main.AddListener(gameObjectListen);
    }

}
