using UnityEngine;

public interface IInteractable
{
    Vector3 GetPlayerPosPoint();

    void Interact();
    float InteractionDistance { get; set; }
    
}