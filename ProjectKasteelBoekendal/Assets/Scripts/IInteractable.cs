using UnityEngine;

public interface IInteractable
{
    Vector3 GetPlayerPosPoint(PlayerInteraction player);

    void Interact(PlayerInteraction player);
    
}