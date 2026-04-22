using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class AnswerCheck : MonoBehaviour
{
    public GameObject slicePrefab;
    public GameObject confirmationPanelPrefab; 
    private GameObject yesButton;
    private GameObject noButton;
    private GameObject confirmationObj;
    public Transform canvas;

    public WordFormer wordFormer;
    public WordSplitProgression wordSplitProgression;
    public LivesWordSplit livesWordSplit;
    public WordSplitUI wordSplitUI;

    public float duration = 3f;

    public void ConfirmAnswer(bool isCorrect)
    {
        wordFormer.SplitWord();
        confirmationObj = Instantiate(confirmationPanelPrefab, canvas);
        yesButton = confirmationObj.transform.Find("YesButton").gameObject;
        noButton = confirmationObj.transform.Find("NoButton").gameObject;
        yesButton.GetComponent<Button>().onClick.AddListener(wordFormer.ResetSplits);
        if (isCorrect == true)
        {
            yesButton.GetComponent<Button>().onClick.AddListener(CorrectAnswer);
        }
        else if (isCorrect == false)
        {
            yesButton.GetComponent<Button>().onClick.AddListener(WrongAnswer);
        }
        
        noButton.GetComponent<Button>().onClick.AddListener(wordFormer.RestitchWord);
    }

    public void DeleteConfirmationPanel()
    {
        Destroy(confirmationObj);
    }

    // This method is called in the WordFormer script which triggers the animations for the correct answer
    public void CorrectAnswer()
    {
        wordFormer.CorrectWord();
        wordSplitProgression.Scores();
        wordSplitUI.UpdateSlicedCounter();
        StartCoroutine(CountdownNewWord());
        DeleteConfirmationPanel();
    }

    // This method is called in the WordFormer script which triggers the animations for the wrong answer

    public void WrongAnswer()
    {
        wordFormer.WrongWord();
        wordSplitUI.BreakKnives();
        livesWordSplit.DecreaseLives();
        DeleteConfirmationPanel();
    }

    // IEnumerator used for the delay between correct answer and next word
    private IEnumerator CountdownNewWord()
    {
        yield return new WaitForSeconds(duration);
        wordSplitProgression.GetNewWord();
    }
}
