using UnityEngine;
using System.Collections.Generic;

public class Testing : MonoBehaviour
{
    [SerializeField] private WordCollection wordCollection;
    private List<NewWord> chosenWords;

    private void Start()
    {
        chosenWords = wordCollection.GetRandomUniqueWords(4);

        foreach (string word in wordCollection.words)
        {
            Debug.Log(word);
        }

        foreach (string word in chosenWords)
        {
            Debug.Log("Chosen word: " + word);
        }
    }
}
