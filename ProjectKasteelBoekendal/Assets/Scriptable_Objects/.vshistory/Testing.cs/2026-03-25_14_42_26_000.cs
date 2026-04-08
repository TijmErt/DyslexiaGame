using UnityEngine;
using System.Collections.Generic;

public class Testing : MonoBehaviour
{
    [SerializeField] private WordCollection wordCollection;
    private List<NewWord> chosenWords;

    private void Start()
    {
        List<string> parts = wordCollection.GetWordParts();
        Debug.Log(parts);

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
