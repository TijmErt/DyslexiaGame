using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FlowAnswerCheck : MonoBehaviour
{
    public FlowLine flowLine;
    
    public string name1;
    public string name2;
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
        name1 = point1.transform.name;
        name2 = point2.transform.name;
        Debug.Log(name1);
        Debug.Log(name2);

        if (name1 == name2)
        {
            flowLine.CorrectLine(point2);
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }
}
