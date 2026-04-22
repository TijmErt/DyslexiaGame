using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Script to add to a gameobject with a trigger collider. Used to start the dialogue
/// </summary>
public class DialogueTrigger : MonoBehaviour, IInteractable
{
	[field: SerializeField] public float InteractionDistance { get; set; } = 2f;
	[field: SerializeField] public GameObject DialogueOverlay { get; set; }

	public Vector3 GetPlayerPosPoint(PlayerInteraction player)
	{
		return transform.position;
	}

	public void Interact(PlayerInteraction player)
	{
		// Trigger dialogue to start
		EventBus.Trigger("OnDialogueStart");
	}
}
