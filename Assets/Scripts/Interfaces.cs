using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Inteface defining messages sent by the player
/// </summary>
public interface IPlayerEvents : IEventSystemHandler
{
    void OnPlayerHurt(int newHealth);

    void OnPlayerReachedExit(GameObject exit);
}

public interface IPowerUpManagerEvents : IEventSystemHandler
{
    void OnSpawnPowerup();
    void OnPowerUpCollected(float xPosition, float zPosition);
}

public interface ICarouselEvents : IEventSystemHandler
{
    void OnRotateCarousel();
}

public interface IPowerUpEvents: IEventSystemHandler
{
    void OnReloadBoostExpired();
    void ToggleReloadBoost();
    void OnSpeedBoostExpired();
    void ToggleSpeedBoost();
}
interface ITakeDamage
{
    void damageTaken(float damage);
}

interface IGivePoints
{
    void GivePoints(Player player, int points);
}



