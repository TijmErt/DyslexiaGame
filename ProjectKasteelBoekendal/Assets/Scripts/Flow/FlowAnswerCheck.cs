using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FlowAnswerCheck : MonoBehaviour
{
    public FlowLine flowLine;
    public FlowList flowList;
    
    public string name1;
    public string name2;
    public string wordpart1;
    public string wordpart2;
    public string word;
    public string reverseWord;

    public int correctWords = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAnswer(Image point1, Image point2)
    {
        name1 = point1.name;
        name2 = point2.name;

        if (name1.Contains("Start") && name2.Contains("Start") || name1.Contains("End") && name2.Contains("End"))
        {
            flowLine.ResetLine();
        }
        
        wordpart1 = point1.GetComponentInChildren<TMPro.TMP_Text>().text;
        wordpart2 = point2.GetComponentInChildren<TMPro.TMP_Text>().text;

        wordpart1 = wordpart1.Replace(".", "");
        wordpart2 = wordpart2.Replace(".", "");

        word = wordpart1 + wordpart2;
        reverseWord = wordpart2 + wordpart1;


        if (flowList.requiredList.Contains(word))
        {
            flowLine.CorrectLine(point2);
            correctWords++;
            if (correctWords == flowList.requiredList.Count)
            {
                Debug.Log("All correct!");
            }
        }
        else if (flowList.requiredList.Contains(reverseWord))
        {
            flowLine.CorrectLine(point2);
            correctWords++;
            if (correctWords == flowList.requiredList.Count)
            {
                Debug.Log("All correct!");
            }
        }
        else if (flowList.bonusList.Contains(word) && !flowList.bonusFoundList.Contains(word))
        {
            Debug.Log("Bonus!");
            flowList.bonusFoundList.Add(word);
        }
        else if (flowList.bonusList.Contains(reverseWord) && !flowList.bonusFoundList.Contains(reverseWord))
        {
            Debug.Log("Bonus!");
            flowList.bonusFoundList.Add(reverseWord);
        }
        else
        {
            flowLine.ResetLine();
        }
    }
}
