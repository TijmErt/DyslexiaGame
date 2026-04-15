using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles all the logic related to the dialogue
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// List of all the dialogue for this interaction
    /// </summary>
    public CharacterDialogue CharacterDialogue { private set; get; }

    /// <summary>
    /// Current position in the <c>CharacterDialogue</c> List
    /// </summary>
    public int DialogueIndex { private set; get; } = 0;
    
    /// <summary>
    /// Textbox for the dialogue text
    /// </summary>
    public GameObject Dialogue { private set; get; }
    
    /// <summary>
    /// Textbox for the name of the character speaking
    /// </summary>
    public GameObject Name { private set; get; }
    
    /// <summary>
    /// Image for the sprite of the character speaking
    /// </summary>
    public GameObject Character {  private set; get; }
    
    /// <summary>
    /// Large transparent button covering the entire screen to detect clicks
    /// </summary>
    public GameObject ClickDetector { private set; get; }

    /// <summary>
    /// Initializes the dialogue manager
    /// </summary>
    void Start() {
        // Get references to all required gameobjects
        this.Dialogue = this.transform.Find("Background/Dialogue").gameObject;
        this.Name = this.transform.Find("Background/NameBackground/Name").gameObject;
        this.Character = this.transform.Find("Background/Character").gameObject;
        this.ClickDetector = this.transform.Find("ClickDetector").gameObject;
        
        this.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Shows the next item in the dialogue list, if there are no more items it ends the dialogue
    /// </summary>
    public void NextDialogue() {
        this.DialogueIndex++;

        // If there is no more dialogue, end the dialogue
        if (this.DialogueIndex > this.CharacterDialogue.Dialogue.Count - 1) {
            EndDialogue();
            return;
        }
        
        ShowDialogue();
    }

    /// <summary>
    /// Hides the dialogue gameobject and unpauses the game
    /// </summary>
    private void EndDialogue() {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the dialogue and pauses the game
    /// </summary>
    public void StartDialogue(CharacterDialogue characterDialogue) {
        this.CharacterDialogue = characterDialogue;
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        ShowDialogue();
    }

    /// <summary>
    /// Updates the gameobjects with the values of the dialogue at the current <c>DialogueIndex</c>
    /// </summary>
    private void ShowDialogue() {
        var currentDialogue = this.CharacterDialogue.Dialogue[this.DialogueIndex];

        // Get components from gameobjects
        this.Dialogue.GetComponent<TextMeshProUGUI>().text = currentDialogue.Text;
        this.Name.GetComponent<TextMeshProUGUI>().text = currentDialogue.Name;
        this.ClickDetector.GetComponent<Button>().onClick = currentDialogue.onClick;
        
        // If no character sprite is set, don't show any sprite
        if (currentDialogue.Expression == null) {
            this.Character.SetActive(false);
        }
        else {
            // Show character sprite
            this.Character.SetActive(true);
            this.Character.GetComponent<Image>().sprite = currentDialogue.Expression;
        }
    }

    /// <summary>
    /// Loads a unity scene
    /// </summary>
    /// <param name="scene">Name of the scene to load</param>
    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}
