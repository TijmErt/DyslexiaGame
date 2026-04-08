using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WordFormer : MonoBehaviour
{
    List<string> wordParts;
    public List<GameObject> buttons;
    public List<GameObject> letters;
    public List<GameObject> splitParts;

    public GameObject letterPrefab;
    public GameObject slicePrefab;
    public Transform canvas;

    public GameObject textMeshObj;
    public AnswerCheck answerCheck;

    float xOffset = 0;
    float spacing = 30f;
    float duration = 3f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called in the WordSplitProgression script which receives the new word and triggers the code to form the new word
    public void ReceiveWord(List<string> receivedWord)
    {
        wordParts = receivedWord;
        buttons = new List<GameObject>();
        letters = new List<GameObject>();
        FormWord();
    }

    // This method goes through the syllables and letters of the word and spawns the prefabs for the letters and buttons in the correct order
    public void FormWord()
    {
        // Go through each syllable
        for (int i = 0; i < wordParts.Count; i++)
        {
            string syllable = wordParts[i];

            // Go through each letter in each syllable
            for (int j = 0; j < syllable.Length; j++)
            {
                char letter = syllable[j];

                // Shows letter in debug log
                Debug.Log(letter);

                // Get the TextMeshPro component and set the letter
                TextMeshProUGUI textMesh = textMeshObj.GetComponentInChildren<TextMeshProUGUI>();
                string character = letter.ToString();
                textMesh.text = character;

                GameObject letterObj = Instantiate(letterPrefab, canvas);
                letters.Add(letterObj);

                RectTransform letterRect = letterObj.GetComponent<RectTransform>();
                letterRect.anchoredPosition = new Vector2(xOffset, 0);

                xOffset += spacing;

                // Check if it's the last letter of the last syllable
                if (i == wordParts.Count - 1 && j == syllable.Length - 1)
                {
                    Debug.Log("End");
                }
                // Checks if this is where the split needs to be, triggers correct answer method
                else if (j == syllable.Length - 1)
                {
                    GameObject sliceObj = Instantiate(slicePrefab, canvas);
                    buttons.Add(sliceObj);
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                    sliceObj.GetComponent<Button>().onClick.AddListener(answerCheck.CorrectAnswer);
                }
                // Checks if this isn't the split, triggers wrong answer method
                else
                {
                    GameObject sliceObj = Instantiate(slicePrefab, canvas);
                    buttons.Add(sliceObj);
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                    sliceObj.GetComponent<Button>().onClick.AddListener(answerCheck.WrongAnswer);
                }
            }   
        }
    }

    // If its the correct answer, it splits into the syllables and triggers the animations
    public void SplitWord()
    {
        HidePreviousPrefabs();

        xOffset = 0;

        for (int i = 0; i < wordParts.Count; i++)
        {
            string syllable = wordParts[i];

            TextMeshProUGUI textMesh = textMeshObj.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = syllable;
            
            GameObject syllableObj = Instantiate(letterPrefab, canvas);

            splitParts.Add(syllableObj);

            RectTransform syllableRect = syllableObj.GetComponent<RectTransform>();
            syllableRect.anchoredPosition = new Vector2(xOffset, 0);

            Rigidbody2D rb = syllableObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 20f;

            BoxCollider2D col = syllableObj.AddComponent<BoxCollider2D>();

            xOffset += 2*spacing;
        }

        StartCoroutine(DeleteFallingParts());
        xOffset = 0;
    }

    // Hides the previous prefabs for the letters and buttons
    public void HidePreviousPrefabs()
    {
        //Destroy all the button and letter gameobjects and clear the lists
        foreach(GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();

        foreach(GameObject letter in letters)
        {
            Destroy(letter);
        }
        letters.Clear();
    }

    //Hides the falling split parts, so they don't clog up the scene
    public void HidePreviousSplits()
    {
        foreach(GameObject part in splitParts)
        {
            Destroy(part);
        }
        splitParts.Clear();
    }

    // If its the wrong answer, turn everything red (and later triggers animation)
    public void WrongWord()
    {
        foreach(GameObject button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.red;
                StartCoroutine(SwitchColorBackButton(buttonImage));
            }
        }
        
        foreach(GameObject letter in letters)
        {
            Image letterImage = letter.GetComponent<Image>();
            if (letterImage != null)
            {
                letterImage.color = Color.red;
                StartCoroutine(SwitchColorBackLetter(letterImage));
            }
        }
        

    }
    // IEnumerators used for the delay between wrong answer and switching the color back to white and deleting the falling parts
    private IEnumerator SwitchColorBackButton(Image image)
    {
        yield return new WaitForSeconds(duration);
        image.color = Color.white;
    }
    private IEnumerator SwitchColorBackLetter(Image image)
    {
        yield return new WaitForSeconds(duration);
        image.color = Color.white;
    }
    private IEnumerator DeleteFallingParts()
    {
        yield return new WaitForSeconds(duration);
        HidePreviousSplits();
    }
}
