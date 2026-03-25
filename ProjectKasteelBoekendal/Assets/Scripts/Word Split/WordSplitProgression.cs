using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WordSplitProgression : MonoBehaviour
{
    public WordFormer wordFormer;
    public AnswerCheck answerCheck;

    List<string> bomen;
    List<string> wordParts;

    int wordIndex = 0;

    public int score = 0;


    // First word of the game
    void Start()
    {
        GetNewWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks the amount of previous words if under the required 10, triggers the code for the next word
    public void GetNewWord()
    {
        if (wordIndex < 10)
        {
            bomen = new List<string>{"bo", "men"};
            wordParts = bomen;
            wordFormer.ReceiveWord(wordParts);
            wordIndex++;
        }
        else
        {
            Debug.Log("You won!");
        }
    }

    // Calculates and show the score
    public void Scores()
    {
        score++;
        Debug.Log("Current Score: " + score);
    }
}
