using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WordFormer : MonoBehaviour
{
    List<string> bomen;
    List<string> wordParts;
    List<List<char>> lettersPerSyllable;

    public GameObject letterPrefab;
    public GameObject slicePrefab;
    public Transform canvas;

    public GameObject textMeshObj;

    float xOffset = 0;
    float spacing = 30f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bomen = new List<string>{"bo", "men"};
        wordParts = bomen;
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
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                }
                else
                {
                    GameObject sliceObj = Instantiate(slicePrefab, canvas);
                    RectTransform sliceRect = sliceObj.GetComponent<RectTransform>();
                    sliceRect.anchoredPosition = new Vector2(xOffset, 0);
                    xOffset += spacing;
                }
            }   
        }
    }
}
