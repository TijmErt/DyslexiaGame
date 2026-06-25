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
	private bool playerInRange;
	public Vector3 GetPlayerPosPoint()
	{
		return transform.position;
	}

	public void Interact()
	{
		if (!playerInRange)
			return;
		// Trigger dialogue to start
		StartDialogue();
	}

	private void StartDialogue()
	{
		EventBus.Trigger(new EventHook("OnDialogueStart", gameObject));
	}
	
	private void Update()
	{
		if (!playerInRange)
			return;


		StartDialogue(); // This should get a method that allows the player to start the Dialogue manually
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
