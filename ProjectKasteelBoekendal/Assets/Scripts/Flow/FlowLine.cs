using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class FlowLine : MonoBehaviour
{

    public FlowAnswerCheck flowAnswerCheck;
    public FlowControls flowControls;
    public FlowList flowList;

    [SerializeField] private Image emptyColor;
    [SerializeField] private Image startPoint;

    public bool lineActive = false;
    public int connectionIndex = 0;
    
    public List<Image> coloredSquares = new List<Image>();
    public List<Image> currentLine = new List<Image>();
    public Dictionary<int, List<Image>> correctLines = new Dictionary<int, List<Image>>();
    public Dictionary<Image, int> searchingDictionary = new Dictionary<Image, int>();

    public float lastConnectionTime;
    public float connectionCooldown = 0.2f;


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
        var randomColor = flowList.colorList[Random.Range(0, flowList.colorList.Count)];
        foreach (var square in coloredSquares)
        {
            square.color = randomColor;
        }
        coloredSquares.Clear();
        lineActive = false;

        List<Image> lineParts = new List<Image>();
        foreach (var part in currentLine)
        {
            lineParts.Add(part);
        }
        correctLines.Add(connectionIndex, lineParts);
        foreach (var part in lineParts)
        {
            if (!searchingDictionary.ContainsKey(part))
            {
                searchingDictionary.Add(part, connectionIndex);
            }
        }

        connectionIndex++;
        lastConnectionTime = Time.time;
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

    public void DestroyConnection(int connectionId)
    {
        if (correctLines.TryGetValue(connectionId, out List<Image> lineParts))
        {
            foreach (var part in lineParts)
            {
                if (part.name.Contains("Empty"))
                {
                    part.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                }
                searchingDictionary.Remove(part);
            }
            correctLines.Remove(connectionId);
            flowAnswerCheck.correctWords--;
        }
    }
}
