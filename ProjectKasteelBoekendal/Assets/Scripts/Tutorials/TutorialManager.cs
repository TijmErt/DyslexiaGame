using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    
    [field: SerializeField] public GameObject Dialogue { get; set; }
    [field: SerializeField] public GameObject Canvas { get; set; }
    
    public int CurrentDialogue { private set; get; } = 0;
    public TutorialDialogueList TutorialDialogueList { private set; get; }

    public GameObject DialogueText { private set; get; }
    public GameObject TopButton { private set; get; }
    public GameObject TopButtonText { private set; get; }
    public GameObject BottomButton { private set; get; }
    public GameObject BottomButtonText { private set; get; }
    public GameObject CoachSprite { private set; get; }
    public GameObject AreaButton {  private set; get; }
    
    void Start()
    {
        // Pause game
        Time.timeScale = 0;
        
        // Check if dialogue gameobject has a dialogue list
        var dialogueList = this.Dialogue.GetComponent<TutorialDialogueList>();
        if (dialogueList == null) {
            throw new System.Exception("Tutorial Dialogue List (Script) is missing from linked dialogue game object.");
        }
        this.TutorialDialogueList = dialogueList;
        var tutorialPanel = this.Canvas.transform.Find("TutorialPanel").gameObject;
        
        // Get Top Button gameobject
        this.TopButton = tutorialPanel.transform.Find("TopButton").gameObject;
        this.TopButtonText = this.TopButton.transform.Find("TopButtonText").gameObject;
        
        // Get Bottom Button gameobject
        this.BottomButton = tutorialPanel.transform.Find("BottomButton").gameObject;
        this.BottomButtonText = this.BottomButton.transform.Find("BottomButtonText").gameObject;
        
        // Get Coach Sprite gameobject
        this.CoachSprite = tutorialPanel.transform.Find("CoachSprite").gameObject;
        
        // Get DialogueText gameobject
        this.DialogueText = tutorialPanel.transform.Find("DialogueText").gameObject;
        
        // Get Area Button gameobject
        this.AreaButton = this.Canvas.transform.Find("AreaButton").gameObject;
        
        // Show first dialogue
        ShowNextDialogue();
    }

    private void ShowNextDialogue() {
        // If no more dialogue, set the canvas inactive
        if (this.CurrentDialogue == this.TutorialDialogueList.dialogueList.Count) {
            this.Canvas.SetActive(false);
            
            // Resume game
            Time.timeScale = 1;
        }
        
        // Get dialogue for current frame
        var dialogue = this.TutorialDialogueList.dialogueList[this.CurrentDialogue];

        // Update canvas with current dialogue
        this.DialogueText.GetComponent<TextMeshProUGUI>().text = dialogue.dialogue;
        this.CoachSprite.GetComponent<Image>().sprite = dialogue.dialogueSprite;

        // Check if top button needs to be displayed
        if (string.IsNullOrEmpty(dialogue.topButtonText)) {
            this.TopButton.SetActive(false);
        }
        else {
            // Update top button with new option
            this.TopButton.SetActive(true);
            this.TopButton.GetComponent<Button>().onClick = dialogue.onTopButtonClick;
            this.TopButtonText.GetComponent<TextMeshProUGUI>().text = dialogue.topButtonText;
        }

        // Check if bottom button needs to be displayed
        if (string.IsNullOrEmpty(dialogue.bottomButtonText)) {
            this.BottomButton.SetActive(false);
        }
        else {
            // Update bottom button with new option
            this.BottomButton.SetActive(true);
            this.BottomButton.GetComponent<Button>().onClick = dialogue.onBottomButtonClick;
            this.BottomButtonText.GetComponent<TextMeshProUGUI>().text = dialogue.bottomButtonText;
        }
        
        // Check if area button needs to be displayed
        if (!dialogue.areaButtonActive) {
            this.AreaButton.SetActive(false);
        }
        else {
            // Update area button with new location and size
            this.AreaButton.SetActive(true);
            this.AreaButton.GetComponent<Button>().onClick = dialogue.onAreaButtonClick;
            var areaButtonRectTransform = this.AreaButton.GetComponent<RectTransform>();
            areaButtonRectTransform.anchoredPosition = dialogue.areaButtonPosition;
            areaButtonRectTransform.sizeDelta = dialogue.areaButtonSize;
        }
        
        // Increase dialogue counter
        this.CurrentDialogue++;
    }


    public void GoToNextDialog(int skip = 0) {
        this.CurrentDialogue += skip;
        this.ShowNextDialogue();
    }

    public void GoToPreviousDialogue(int skip = 0) {
        this.CurrentDialogue -= skip;
        this.ShowNextDialogue();
    }
}
