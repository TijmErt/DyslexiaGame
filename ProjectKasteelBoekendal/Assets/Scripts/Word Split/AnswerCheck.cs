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

    public void CorrectAnswer()
    {
        Debug.Log("Correct Answer");
        wordFormer.SplitWord();
        wordFormer.RightWord();
        wordSplitProgression.Scores();
        StartCoroutine(CountdownNewWord());
    }

    public void WrongAnswer()
    {
        Debug.Log("Wrong Answer");
        wordFormer.WrongWord();
        livesWordSplit.DecreaseLives();
    }
    private IEnumerator CountdownNewWord()
    {
        yield return new WaitForSeconds(duration);
        wordSplitProgression.GetNewWord();
    }
}
