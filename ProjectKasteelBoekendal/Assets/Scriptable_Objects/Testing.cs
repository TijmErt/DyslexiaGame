using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Testing : MonoBehaviour
{
    [SerializeField] private WordCollection wordCollection;
    private List<NewWord> chosenWords;
    private List<string> wordParts;
    private List<MemoryWordData> memoryData;

    private void Start()
    {
    // GETWORDSYLLABLES 
        // wordParts = wordCollection.GetWordSyllables();
        // foreach (string part in wordParts)
        // { 
        //     Debug.Log(part);
        // }

    // GET MEMORY PUZZLE DATA

        memoryData = wordCollection.GetMemoryData();
        foreach (var item in memoryData)
        {
            Debug.Log($"ID: {item.id}, Word: {item.word}");
        }


    // GET 4 RANDOM UNIQUE WORDS 
        //chosenWords = wordCollection.GetRandomUniqueWords(4);

        //foreach (NewWord entry in wordCollection.words)
        //{
        //    Debug.Log(entry.word + " (" + entry.syllablesCount + ")");
        //}

        //foreach (NewWord entry in chosenWords)
        //{
        //    Debug.Log("Chosen word: " + entry.word);
        //}

        //foreach (NewWord entry in chosenWords)
        //{
        //    string parts = string.Join(", ", entry.syllablesParts);
        //    Debug.Log("syllable parts: " + parts);
        //}
    }
}
