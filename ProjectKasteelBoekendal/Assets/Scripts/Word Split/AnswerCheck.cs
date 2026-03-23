using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswerCheck : MonoBehaviour
{
    public GameObject slicePrefab;
    public WordFormer wordFormer;

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

    }

    public void WrongAnswer()
    {
        Debug.Log("Wrong Answer");
        wordFormer.WrongWord();
    }
}
