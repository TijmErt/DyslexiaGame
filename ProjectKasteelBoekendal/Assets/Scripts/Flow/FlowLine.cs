using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class FlowLine : MonoBehaviour
{

    [SerializeField] private Image emptyColor;
    [SerializeField] private Image startPoint;

    public bool lineActive = false;
    
    public List<Image> coloredSquares = new List<Image>();


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
        //check if empty is empty
        //check if empty is next to previous object
    }

    public void ColorEmpty (Image emptyBox)
    {
        Debug.Log("Color Empty");
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
            foreach (var square in coloredSquares)
            {
                square.color = Color.blue;
            }
            coloredSquares.Clear();
            lineActive = false;
        }
        else if (lineActive == false)
        {
            startPoint = endPoint; 
            lineActive = true;
        }

        Debug.Log("EndPoint Hit");
    }

    public void ResetLine()
    {
        lineActive = false;
        emptyColor.color = Color.clear;
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
