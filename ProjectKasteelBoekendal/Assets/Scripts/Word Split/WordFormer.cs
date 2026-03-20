using UnityEngine;
using System.Collections.Generic;

public class WordFormer : MonoBehaviour
{
    List<string> bomen;
    List<string> wordParts;
    List<List<char>> lettersPerSyllable;

   

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


                if (i == wordParts.Count - 1 && j == syllable.Length - 1)
                {
                    Debug.Log("End");
                }
                else if (j == syllable.Length - 1)
                {
                    Debug.Log("Yay");
                }
                else
                {
                    Debug.Log("Nay");
                }
            }   
        }
    }
}
