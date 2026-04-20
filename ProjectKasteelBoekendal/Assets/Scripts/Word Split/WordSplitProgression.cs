using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WordSplitProgression : MonoBehaviour
{
    public EndScreenManager endScreenManager;
    public WordFormer wordFormer;
    public AnswerCheck answerCheck;
    public WordSplitUI wordSplitUI;

    public List<string> bomen  = new List<string>{"cra", "zy"};
    List<string> wordParts;

    int wordIndex = 0;
    public int totalWords;
    public bool isInfinite;

    public int score = 0;


    // First word of the game
    void Start()
    {
        SetTotalWords(totalWords);
        GetNewWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTotalWords(int total)
    {
        totalWords = total;
        wordSplitUI.UpdateSlicedCounter();
    }

    // Checks the amount of previous words if under the required 10, triggers the code for the next word
    public void GetNewWord()
    {
        if (wordIndex < totalWords)
        {
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

    public void EndGame()
    {
        string dynamicText = "Groente gesneden";
        int coins = score * 100;
        endScreenManager.InstantiateEndScreen(dynamicText, score, coins);
    }
}
