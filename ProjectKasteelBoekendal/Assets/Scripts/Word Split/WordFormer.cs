using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WordFormer : MonoBehaviour
{
    
    public Slicing slicing;

    List<string> wordParts;
    public List<GameObject> buttons;
    public List<GameObject> letters;
    public List<string> letterChars;
    public List<string> splits;
    public List<GameObject> splitParts;
    public char letter;
    public string split1;
    public string split2;
    
    public GameObject letterPrefab;
    public GameObject slicePrefab;
    public Transform canvas;

    public GameObject textMeshObj;
    public AnswerCheck answerCheck;

    float xOffset = 0;
    float spacing = 30f;
    float duration = 3f;

    int colorIndex = 0;    

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
        int splitIndex = 0;

        // Go through each syllable
        for (int i = 0; i < wordParts.Count; i++)
        {
            string syllable = wordParts[i];

            // Go through each letter in each syllable
            for (int j = 0; j < syllable.Length; j++)
            {
                letter = syllable[j];

                // Get the TextMeshPro component and set the letter
                TextMeshProUGUI textMesh = textMeshObj.GetComponentInChildren<TextMeshProUGUI>();
                string character = letter.ToString();
                textMesh.text = character;
                letterChars.Add(character);

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
                    int capturedSplitPoint = splitIndex; // Capture the current value of j
                    sliceObj.GetComponent<Button>().onClick.AddListener(() => SetSplits(capturedSplitPoint));
                    sliceObj.GetComponent<Button>().onClick.AddListener(() => answerCheck.ConfirmAnswer(true));
                }
                // Checks if this isn't the split, triggers wrong answer method
                else
                {
                    GameObject sliceObj = Instantiate(slicePrefab, canvas);
                    buttons.Add(sliceObj);
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                    int capturedSplitPoint = splitIndex; // Capture the current value of j
                    sliceObj.GetComponent<Button>().onClick.AddListener(() => SetSplits(capturedSplitPoint));
                    sliceObj.GetComponent<Button>().onClick.AddListener(() => answerCheck.ConfirmAnswer(false));
                }

                splitIndex++;
            }   
        }

        slicing.slicingEnabled = true;
        
    }

    public void SetSplits(int splitPoint)
    {
        for (int i = 0; i <= splitPoint; i++)
        {
            string newChar = letterChars[i];
            split1 = split1 + newChar;
        }
        for (int j = splitPoint + 1; j < letters.Count; j++)
        {
            string newChar = letterChars[j];
            split2 = split2 + newChar;
        }
        splits.Add(split1);
        splits.Add(split2);
    }

    // If its the correct answer, it splits into the syllables and triggers the animations
    public void SplitWord()
    {
        HidePreviousPrefabs();

        xOffset = 0;

        // de juiste "syllables" maken

        for (int i = 0; i < splits.Count; i++)
        {
            string syllable = splits[i];

            TextMeshProUGUI textMesh = textMeshObj.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = syllable;
            
            GameObject syllableObj = Instantiate(letterPrefab, canvas);

            splitParts.Add(syllableObj);

            RectTransform syllableRect = syllableObj.GetComponent<RectTransform>();
            syllableRect.anchoredPosition = new Vector2(xOffset, 0);

            xOffset += 2*spacing;
        }
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

    public void CorrectWord()
    {
        foreach(GameObject part in splitParts)
        {
            Rigidbody2D rb = part.AddComponent<Rigidbody2D>();
            rb.gravityScale = 20f;

            BoxCollider2D col = part.AddComponent<BoxCollider2D>();
        }
        
        StartCoroutine(DeleteFallingParts());
    }

    // If its the wrong answer, turn everything red (and later triggers animation)
    public void WrongWord()
    {
        foreach(GameObject parts in splitParts)
        {
            Image partsImage = parts.GetComponent<Image>();
            if (partsImage != null)
            {
                partsImage.color = Color.red;
                StartCoroutine(SwitchColorBackButton(partsImage));
            }
        }
    }

    public void ResetSplits()
    {
        split1 = "";
        split2 = "";
        splits.Clear();
    }

    public void RestitchWord()
    {
        foreach(GameObject part in splitParts)
        {
            Destroy(part);
        }
        splitParts.Clear();
        ResetSplits();
        FormWord();
        answerCheck.DeleteConfirmationPanel();
        slicing.slicingEnabled = true;
    }

    // IEnumerators used for the delay between wrong answer and switching the color back to white and deleting the falling parts
    private IEnumerator SwitchColorBackButton(Image image)
    {
        yield return new WaitForSeconds(duration);
        image.color = Color.white;
        colorIndex++;
        if (colorIndex >= splitParts.Count)
        {
            colorIndex = 0;
            RestitchWord();
        }
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
