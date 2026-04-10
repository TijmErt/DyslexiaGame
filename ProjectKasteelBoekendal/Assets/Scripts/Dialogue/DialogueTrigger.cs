using UnityEngine;

/// <summary>
/// Script to add to a gameobject with a trigger collider. Used to start the dialogue
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
	/// <summary>
	/// Dialogue manager for the dialogue that needs to trigger
	/// </summary>
	[field: SerializeField] public DialogueManager DialogueManager { get; set; }
	
	/// <summary>
	/// When an object with the <c>Player1</c> tag enters the trigger, start the dialogue
	/// </summary>
	/// <param name="col">Gameobject that entered the trigger area</param>
	private void OnTriggerEnter(Collider col) {
		if (!col.CompareTag("Player1")) return;
		
		this.DialogueManager.StartDialogue();
	}
}
