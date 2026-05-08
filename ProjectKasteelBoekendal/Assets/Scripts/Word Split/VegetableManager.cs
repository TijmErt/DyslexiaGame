using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VegetableManager : MonoBehaviour
{

	private GameObject LetterSpacer { get; set; }
	private List<GameObject> Letters { get; set; }
	private List<GameObject> Buttons { get; set; }

	void Start() {
		this.LetterSpacer = this.transform.Find("LetterSpacer").gameObject;
		this.Letters = new List<GameObject>();
		this.Buttons = new List<GameObject>();
	}

	private void Update() {
		// Move the background images to hide the vegetable to the center of the screen
		foreach (var button in this.Buttons) {
			button.transform.GetChild(0).GetComponent<Image>().transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
		}
	}

	/// <summary>
	/// Displays a word on the vegetable
	/// </summary>
	/// <param name="word">The word to display</param>
	public void ShowWord(string word) {
		for (var i = 0; i < word.Length; i++) {
			var letter = word[i];
			var textBox = CreateTextBox(letter.ToString(), this.LetterSpacer.gameObject);

			if (i != word.Length - 1) {
				CreateButton(this.LetterSpacer.gameObject, (button) => {
					var rectmask = button.GetComponent<RectMask2D>();
					rectmask.padding = rectmask.padding == Vector4.zero ? Vector4.one * 100 : Vector4.zero;
				});
			}
		}
	}

	/// <summary>
	/// Creates a new TMP textbox with a fixed size
	/// </summary>
	/// <param name="text">Text inside the textbox</param>
	/// <param name="parent">The gameobject to parent the textbox to</param>
	/// <returns>Created gameobject</returns>
	private GameObject CreateTextBox(string text, GameObject parent) {
		
		// Create new gameobject
		var textBox = new GameObject(text, typeof(RectTransform), typeof(TextMeshProUGUI));
		this.Letters.Add(textBox);
		textBox.transform.SetParent(parent.transform, false);
			
		// Add TMP and set configuration
		var textComponent = textBox.GetComponent<TextMeshProUGUI>();
		textComponent.text = text;
		textComponent.enableAutoSizing = true;
		textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
		textComponent.alignment = TextAlignmentOptions.Center;
		
		// Rescale textbox
		textBox.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 150);
			
		return textBox;
	}

	/// <summary>
	/// Creates a new button with a fixed size
	/// </summary>
	/// <param name="parent">The gameobject to parent the textbox to </param>
	/// <param name="onClick">The function to call on button click</param>
	/// <returns>Created gameobject</returns>
	private GameObject CreateButton(GameObject parent, UnityAction<GameObject> onClick) {
		// Create new gameobject
		var button = new GameObject("SliceButton", typeof(RectTransform), typeof(RectMask2D), typeof(Image), typeof(Button), typeof(LayoutElement));
		this.Buttons.Add(button);
		button.transform.SetParent(parent.transform, false);
		
		// Hide mask using padding
		button.GetComponent<RectMask2D>().padding = Vector4.one * 100;
		
		// Add invisible image for click detection
		button.GetComponent<Image>().color = new Color(0, 0, 0, 0);
		
		// Create new gameobject for bg image
		var buttonSprite = new GameObject("SliceButtonSprite", typeof(RectTransform), typeof(Image));
		buttonSprite.transform.SetParent(button.transform, false);
		
		// Set image to background
		var imageComponent = buttonSprite.GetComponent<Image>();
		imageComponent.sprite = Resources.Load<Sprite>("Sprites/WordSplit/WordSplitBG");
		
		// Scale image to screen size
		var transformComponent = buttonSprite.GetComponent<RectTransform>();
		transformComponent.sizeDelta = new Vector2(Screen.width, Screen.height);
		
		// Link function to on-button click
		var buttonComponent = button.GetComponent<Button>();
		buttonComponent.onClick.AddListener(() => {onClick.Invoke(button);});
		
		// Rescale button
		button.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 350);
		var layoutElement = button.GetComponent<LayoutElement>();
		layoutElement.flexibleWidth = 1;
		layoutElement.minWidth = 25;
		
		return button;
	}

	/// <summary>
	/// Checks if the current cuts on the vegetable are correct
	/// </summary>
	/// <returns>True if correct</returns>
	public bool CheckIfCorrect(List<string> correct) {
		var syllable = string.Empty;
		var answer = new List<string>();
		
		foreach (var (rectMask, i) in this.Buttons.Select((cut, i) => (cut.GetComponent<RectMask2D>(), i))) {
			// Add letter to current syllable
			syllable += this.Letters[i].name;

			// If not cut is performed, continue
			if (rectMask.padding != Vector4.zero) {
				continue;
			}
			
			// If cut, add syllable to answer and set reset syllable tracker
			answer.Add(syllable);
			syllable = string.Empty;
		}
		
		// Add last letter and add last syllable to answer
		syllable += this.Letters.Last().name;
		answer.Add(syllable);

		return string.Join(",", answer) == string.Join(",", correct);
	}
}
