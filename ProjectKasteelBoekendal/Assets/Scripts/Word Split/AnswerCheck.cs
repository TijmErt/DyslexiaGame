using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class AnswerCheck : MonoBehaviour
{
    public GameObject slicePrefab;
    public WordFormer wordFormer;
    public WordSplitProgression wordSplitProgression;
    public LivesWordSplit livesWordSplit;
    public float duration = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called in the WordFormer script which triggers the animations for the correct answer
    public void CorrectAnswer()
    {
        Debug.Log("Correct Answer");
        wordFormer.SplitWord();
        wordSplitProgression.Scores();
        StartCoroutine(CountdownNewWord());
    }

    // This method is called in the WordFormer script which triggers the animations for the wrong answer

    public void WrongAnswer()
    {
        Debug.Log("Wrong Answer");
        wordFormer.WrongWord();
        livesWordSplit.DecreaseLives();
    }
    // IEnumerator used for the delay between correct answer and next word
    private IEnumerator CountdownNewWord()
    {
        yield return new WaitForSeconds(duration);
        wordSplitProgression.GetNewWord();
    }
}
