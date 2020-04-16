/************************************************************************/
/* Author:             Eddie Habal                                      */
/* Date Created:       2/24/2020                                        */
/* Last Modified Date: 4/13/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
#region Libraries

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#endregion
public class PowerUp : MonoBehaviour
{
    #region Variables
    public    bool                 expiresImmediately;      // Tick true for power ups that are instant use, eg a health addition that has no delay before expirint

    public    string               powerUpName;
    public    string               powerUpExplanation;
    public    string               powerUpQuote;

    public    GameObject           specialEffect;
    public    AudioClip            soundEffect;

    private   Vector3              powerUpOffset             = new Vector3(0.0f, -20.0f, 0.0f);

    private   List<string>         nameFilter                = new List<string>() { "Bullets", "Bullet", "(Clone)", "(UnityEngine.GameObject)", "PowerUp", "PU" };
                                                            // Keeps a list of words to remove from the object name

    protected PlayerController     playerBrain;             // Keeps a reference to the player that collected
    public    MeshRenderer         powerUpMeshRenderer;     // Note: Should PlayerBrain just be mashed in with Player?
    protected enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }
    protected      PowerUpState    powerUpState;
    #endregion
    protected virtual void Awake                      ()
    {
        powerUpMeshRenderer = GetComponent<MeshRenderer>();
    }
    protected virtual void Start                      ()
    {
        powerUpState = PowerUpState.InAttractMode;
    }
    protected virtual void OnTriggerEnter             (Collider other)
    {
        PowerUpCollected(other.gameObject);
    }
    protected virtual void PowerUpCollected           (GameObject gameObjectCollectingPowerUp)
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

        // Send message to any listeners
        SendPowerUpCollectedMessage();

        // We move the power up game object to be under the player that collected it, this isn't essential for functionality
        // but is neater in the gameObject hierarchy
        gameObject.transform.parent   = playerBrain.gameObject.transform;
        gameObject.transform.position = playerBrain.gameObject.transform.position + powerUpOffset;

        // Collection effects
        PowerUpEffects();

        // Deliver the payload
        PowerUpPayload();

 

        // Now the power up visuals can go away
        //powerUpMeshRenderer.enabled = false;
    }
    protected virtual void PowerUpEffects             ()
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
    protected virtual void PowerUpPayload             ()
    {
        #region Sends powerup name to on-screen prompt
        playerBrain.powerupAcquired = true;
        foreach(string unfilteredName in nameFilter)
        {
            if (gameObject.name.Contains(unfilteredName))
                gameObject.name = gameObject.name.Replace(unfilteredName, "");
            UIManager.powerupName = gameObject.name;
        }
        #endregion

        Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);

        // If we're instant use we also expire self immediately
        if (expiresImmediately)
        {
            PowerUpHasExpired();
        }
    }
    protected virtual void PowerUpHasExpired          ()
    {
        if (powerUpState == PowerUpState.IsExpiring)
            return;

        powerUpState = PowerUpState.IsExpiring;

        // Send message to any listeners
        //foreach (GameObject go in EventSystemListeners.main.listeners)
        //{ ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpExpired(this, playerBrain)); }
        Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
        DestroySelfAfterDelay();
    }
    protected virtual void DestroySelfAfterDelay      ()
    {
        // Arbitrary delay of some seconds to allow particle, audio is all done
        Destroy(gameObject, 0.01f);
    }
    protected void         StartListening             (GameObject gameObjectListen)
    {
        EventSystemListeners.main.AddListener(gameObjectListen);
    }
    private   void         SendPowerUpCollectedMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners) // 1
            {
                ExecuteEvents.Execute<IPowerUpManagerEvents>               // 2
                    (go, null,                                             // 3
                     (x, y) => x.OnPowerUpCollected(transform.position.x, transform.position.z)                      // 4
                    );
            }
        }
    }
}