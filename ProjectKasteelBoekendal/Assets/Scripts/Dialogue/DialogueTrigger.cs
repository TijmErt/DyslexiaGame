using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// Script to add to a gameobject with a trigger collider. Used to start the dialogue
/// </summary>
public class DialogueTrigger : MonoBehaviour, IInteractable
{
	[field: SerializeField] public float InteractionDistance { get; set; } = 2f;
	[field: SerializeField] public GameObject DialogueOverlay { get; set; }
	private bool playerInRange;
	public Vector3 GetPlayerPosPoint()
	{
		return transform.position;
	}

	public void Interact()
	{
		// Trigger dialogue to start
		EventBus.Trigger(new EventHook("OnDialogueStart", this.gameObject));
	}
	private void Update()
	{
		if (!playerInRange)
			return;

		if (Keyboard.current.eKey.wasPressedThisFrame)
		{
			EventBus.Trigger(new EventHook("OnDialogueStart", gameObject));
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player1"))
			playerInRange = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player1"))
			playerInRange = false;
	}
}
