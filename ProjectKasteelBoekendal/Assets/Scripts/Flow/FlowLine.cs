using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class FlowLine : MonoBehaviour
{

    public FlowAnswerCheck flowAnswerCheck;
    [SerializeField] private Image emptyColor;
    [SerializeField] private Image startPoint;

    public bool lineActive = false;
    
    public List<Image> coloredSquares = new List<Image>();
    public List<Image> currentLine = new List<Image>();


    public Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmptyHit(Image emptyBox)
    {
        if (lineActive == true)
        {
            ColorEmpty(emptyBox);
        }
    }

    public void ColorEmpty (Image emptyBox)
    {
        currentLine.Add(emptyBox);
        emptyColor = emptyBox.transform.GetChild(0).GetComponent<Image>();
        emptyColor.color = Color.white;
        coloredSquares.Add(emptyColor);
    }

    public void EndPointHit (Image endPoint)
    {
        if (endPoint == startPoint)
        {
            return;
        }

        if (lineActive == true)
        {
            currentLine.Add(endPoint);
            flowAnswerCheck.CheckAnswer(startPoint, endPoint);
        }

        else if (lineActive == false)
        {
            startPoint = endPoint; 
            currentLine.Add(endPoint);
            lineActive = true;
        }
    }

    public void CorrectLine(Image endPoint)
    {
        foreach (var square in coloredSquares)
        {
            square.color = Color.blue;
        }
        coloredSquares.Clear();
        lineActive = false;
        ResetLine();
    }
    public void ResetLine()
    {
        lineActive = false;
        startPoint = null;
        currentLine.Clear();

        if (coloredSquares.Count == 0)
        {
            return;
        }
        
        foreach (var square in coloredSquares)
        {
            square.color = Color.clear;
        }

        coloredSquares.Clear();
    }
}
