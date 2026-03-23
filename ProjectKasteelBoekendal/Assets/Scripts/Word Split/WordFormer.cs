using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WordFormer : MonoBehaviour
{
    List<string> bomen;
    List<string> wordParts;
    List<List<char>> lettersPerSyllable;
    List<GameObject> buttons;
    List<GameObject> letters;

    public GameObject letterPrefab;
    public GameObject slicePrefab;
    public Transform canvas;

    public GameObject textMeshObj;
    public AnswerCheck answerCheck;

    float xOffset = 0;
    float spacing = 30f;
    float duration = 5f;

    public Color oldButtoncolor;
    public Color oldLetterColor;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bomen = new List<string>{"bo", "men"};
        wordParts = bomen;
        buttons = new List<GameObject>();
        letters = new List<GameObject>();
        FormWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

                // Spawn letter
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

                if (i == wordParts.Count - 1 && j == syllable.Length - 1)
                {
                    Debug.Log("End");
                }
                else if (j == syllable.Length - 1)
                {
                    GameObject sliceObj = Instantiate(slicePrefab, canvas);
                    buttons.Add(sliceObj);
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                    sliceObj.GetComponent<Button>().onClick.AddListener(answerCheck.CorrectAnswer);
                }
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

            RectTransform syllableRect = syllableObj.GetComponent<RectTransform>();
            syllableRect.anchoredPosition = new Vector2(xOffset, 0);

            xOffset += 2*spacing;
        }
    }
    public void HidePreviousPrefabs()
    {
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
        buttons.Clear();
        foreach(GameObject letter in letters)
        {
            Image letterImage = letter.GetComponent<Image>();
            if (letterImage != null)
            {
                letterImage.color = Color.red;
                StartCoroutine(SwitchColorBackLetter(letterImage));
            }
        }
        letters.Clear();

    }
    public void RightWord()
    {
        foreach(GameObject button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.green;
            }
        }
        buttons.Clear();

        foreach(GameObject letter in letters)
        {
            Image letterImage = letter.GetComponent<Image>();
            if (letterImage != null)
            {
                letterImage.color = Color.green;
            }
        }
        letters.Clear();
    }
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
}
