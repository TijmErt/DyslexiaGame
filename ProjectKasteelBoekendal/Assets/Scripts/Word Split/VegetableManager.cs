using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Community.Libraries.Humility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VegetableManager : MonoBehaviour
{

	private GameObject LetterSpacer { get; set; }
	private List<GameObject> Letters { get; set; }
	private List<GameObject> Buttons { get; set; }
	[field: SerializeField] public Sprite Checkmark { get; set; }
	[field: SerializeField] public Sprite MissingCheckmark { get; set; }
	[field: SerializeField] public Sprite WrongCheckmark { get; set; }
	[field: SerializeField] public TMP_FontAsset Font { get; set; }

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
		this.Clear();
		
		for (var i = 0; i < word.Length; i++) {
			var letter = word[i];
			var textBox = CreateTextBox(letter, this.LetterSpacer.gameObject);

			if (i != word.Length - 1) {
				CreateButton(this.LetterSpacer.gameObject, (button) => {
					var rectmask = button.GetComponent<RectMask2D>();
					rectmask.padding = rectmask.padding == Vector4.zero ? Vector4.one * 100 : Vector4.zero;
				});
			}
		}
	}

	
	/// <summary>
	/// Clears the class and vegetable to be ready for the next word
	/// </summary>
	private void Clear() {
		this.Letters.Clear();
		this.Buttons.Clear();
		foreach (Transform child in this.LetterSpacer.transform) {
			Destroy(child.gameObject);
		}
	}

	/// <summary>
	/// Creates a new TMP textbox with a fixed size
	/// </summary>
	/// <param name="text">Text inside the textbox</param>
	/// <param name="parent">The gameobject to parent the textbox to</param>
	/// <returns>The created Gameobject</returns>
	private GameObject CreateTextBox(char text, GameObject parent) {
		
		// Create new gameobject
		var textBox = new GameObject(text.ToString(), typeof(RectTransform), typeof(TextMeshProUGUI));
		this.Letters.Add(textBox);
		textBox.transform.SetParent(parent.transform, false);
			
		// Add TMP and set configuration
		var textComponent = textBox.GetComponent<TextMeshProUGUI>();
		textComponent.text = text.ToString();
		textComponent.enableAutoSizing = true;
		textComponent.fontSizeMax = 255;
		textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
		textComponent.alignment = TextAlignmentOptions.Center;
		textComponent.font = this.Font;
		textComponent.outlineWidth = 0.1f;
		textComponent.outlineColor = new Color32(66, 71, 27, 255);
		
		// Rescale textbox
		textBox.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 200);

		// Add a letter marker to this letter
		this.CreateLetterMarker(text, textBox);
			
		return textBox;
	}

	/// <summary>
	/// Creates the checkmark box above each cut to indicate if a cut was wrong, correct or missing
	/// </summary>
	/// <param name="parent">The cut object</param>
	/// <returns>The created Gameobject</returns>
	private GameObject CreateCheckmark(GameObject parent) {
		var letterMarker = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
		var image = letterMarker.GetComponent<Image>();
		
		letterMarker.transform.SetParent(parent.transform, false);
		letterMarker.transform.localPosition =  new Vector3(0, 200, 0);
		
		image.sprite = this.Checkmark;
		image.maskable = false;
		image.enabled = false;
		
		letterMarker.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

		return letterMarker;
	}
	
	/// <summary>
	/// Creates the marker beneath each letter, indicating if it's a vowel or a consonant
	/// </summary>
	/// <param name="letter">The letter shown above the marker</param>
	/// <param name="parent">The textbox of the letter</param>
	/// <returns>The created Gameobject</returns>
	private GameObject CreateLetterMarker(char letter, GameObject parent) {
		var letterMarker = new GameObject(letter.ToString(), typeof(RectTransform), typeof(Image));
		var image = letterMarker.GetComponent<Image>();
		
		// Position the marker underneath the letter
		letterMarker.transform.SetParent(parent.transform, false);
		letterMarker.transform.localPosition =  new Vector3(0, -200, 0);
		
		// Hide the marker and set the color to a consonant
		image.enabled = false;
		image.color = new Color(0f, 0.6f, 1f, 1);
		letterMarker.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponentInParent<RectTransform>().sizeDelta.x * 1f, 50);
		
		// Check if the letter is a vowel, if yes update the color
		if ("aeiou".Contains(letter)) {
			image.color = new Color(1, 1, 1, 1);
		}

		return letterMarker;
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
		
		// Add indication checkmark
		this.CreateCheckmark(button);
		
		return button;
	}

	/// <summary>
	/// Checks if the current cuts on the vegetable are correct
	/// </summary>
	/// <returns>True if correct</returns>
	public (bool check, string answer, string correct) CheckIfCorrect(List<string> correct) {
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

		// Transform the lists into strings for easier comparison
		var answerString = string.Join(" ", answer);
		var correctString = string.Join(" ", correct);
		
		return (answerString == correctString, answerString, correctString);
	}

	/// <summary>
	/// Change the scene into Feedback mode, showing the checkmarks, lettermarkers, etc.
	/// </summary>
	/// <param name="correctCheck">Return values of <see cref="CheckIfCorrect"/></param>
	public void ShowFeedback((bool check, string answer, string correct) correctCheck) {
		
		// Enable the lettermarker for each letter
		this.Letters.ForEach(letter => letter.GetComponentInChildren<Image>().enabled = true);
		
		// Get the validity of each cut
		var (correct, wrong, missing) = this.GetCutPositions(correctCheck.answer, correctCheck.correct);

		// Mark all correct cuts
		foreach (var i in correct) {
			var checkmark = this.Buttons[i].transform.GetChild(1).GetComponent<Image>();
			checkmark.sprite = this.Checkmark;
			checkmark.enabled = true;
		}
		
		// Mark all wrong cuts
		foreach (var i in wrong) {
			var checkmark = this.Buttons[i].transform.GetChild(1).GetComponent<Image>();
			checkmark.sprite = this.WrongCheckmark;
			checkmark.enabled = true;
		}
		
		// Mark all missing cuts
		foreach (var i in missing) {
			var checkmark =  this.Buttons[i].transform.GetChild(1).GetComponent<Image>();
			checkmark.sprite = this.MissingCheckmark;
			checkmark.enabled = true;
		}

		// If there should be no cuts and there are no cuts, mark the entire word as correct
		if (correct.Length == 0 && wrong.Length == 0 && missing.Length == 0) {
			var checkmark = this.Buttons[Math.Abs(this.Buttons.Count / 2)].transform.GetChild(1).GetComponent<Image>();
			checkmark.sprite = this.Checkmark;
			checkmark.enabled = true;
		}
	}

	/// <summary>
	/// Gets all the correct, wrong and missing cuts their indexes in the buttons list
	/// </summary>
	/// <param name="answerString">String of the answer, with spaces where there are cuts</param>
	/// <param name="correctString">String of what the answer should look like, with spaces where there should be cuts</param>
	/// <returns>arrays of correct, wrong and missing indexes</returns>
	private (int[] correct, int[] wrong, int[] missing) GetCutPositions(string answerString, string correctString) {
		
		// Get cut positions
		var answerGaps = GetDotGaps(answerString);
		var correctGaps = GetDotGaps(correctString);

		// Compare sets to get correct, wrong and missing indexes
		return (
			correct: correctGaps.Intersect(answerGaps).OrderBy(n => n).ToArray(), // Cut present in both sets
			wrong: answerGaps.Except(correctGaps).OrderBy(n => n).ToArray(), // Cut only present in answer
			missing: correctGaps.Except(answerGaps).OrderBy(n => n).ToArray() // Cut only present in correct answer
		);
		
		// Gets the index of each letter before a space. Spaces do not increase the index.
		//
		// "A VON TUUR"
		//  1 234 5678
		//  ^   ^
		//
		// This function will return the indexes of A and N.
		HashSet<int> GetDotGaps(string str)
		{
			var gaps = new HashSet<int>();
			var letterCount = -1;

			foreach (var c in str)
			{
				if (c == ' ')
				{
					// If character is a space, add the last recorded letter index and continue
					gaps.Add(letterCount);
				}
				else
				{
					letterCount++;
				}
			}
			return gaps;
		}
	}

	public void HideFeedback() {
		foreach (var letter in this.Letters) {
			letter.GetComponentInChildren<Image>().enabled = false;
		}
		
		foreach (var button in this.Buttons) {
			button.GetComponentInChildren<Image>().enabled = false;
		}
		
	}
}
