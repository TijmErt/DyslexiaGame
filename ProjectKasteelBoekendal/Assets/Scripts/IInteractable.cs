using UnityEngine;

public interface IInteractable
{
    Vector3 GetPlayerPosPoint();
    Vector3 GetPlayerPosPoint(PlayerInteraction player);

    void Interact();
    void Interact(PlayerInteraction player);
    
}