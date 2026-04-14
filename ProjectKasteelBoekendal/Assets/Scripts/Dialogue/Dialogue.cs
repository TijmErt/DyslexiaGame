using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to keep track of all the data required for a single frame of dialogue
/// </summary>
[Serializable]
public class Dialogue
{
	/// <summary>
	/// Name of the character speaking
	/// </summary>
	[field: SerializeField] public string Name { get; set; }
	
	/// <summary>
	/// Text that the character says
	/// </summary>
	[field: SerializeField] public string Text { get; set; }
	
	/// <summary>
	/// Sprite of character speaking
	/// </summary>
	[field: SerializeField] public Sprite Expression { get; set; }
	
	/// <summary>
	/// Actions to take when players click the screen
	/// </summary>
	[field: SerializeField] public Button.ButtonClickedEvent onClick { get; set; }
}
