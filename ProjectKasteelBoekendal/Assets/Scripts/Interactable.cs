using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] public Transform interactionPoint;
    [SerializeField] private float interactionDistance = 1f;

    public virtual void Interact()
    {
        Debug.Log($"{name} was interacted with by ");
    }
}
