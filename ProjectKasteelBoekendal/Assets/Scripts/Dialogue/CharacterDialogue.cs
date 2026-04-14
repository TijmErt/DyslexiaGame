using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of all the dialogue for a single interaction
/// </summary>
public class CharacterDialogue : MonoBehaviour
{
	/// <summary>
	/// List of dialogue
	/// </summary>
	[field: SerializeField] public List<Dialogue> Dialogue { get; set; }
}
