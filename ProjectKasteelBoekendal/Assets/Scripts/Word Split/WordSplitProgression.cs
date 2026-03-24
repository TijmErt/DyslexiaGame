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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNewWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            Debug.Log("You won, YAYYYY!!!!");
        }
    }

    public void Scores()
    {
        score++;
        Debug.Log("Current Score: " + score);
    }
}
