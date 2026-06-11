using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

using Debug = UnityEngine.Debug;

public class FlowControls : MonoBehaviour
{
    public Image previousHit;

    public FlowLine flowLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        Vector2 inputPosition = Vector2.zero;
        bool isInputActive = false;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            inputPosition = Mouse.current.position.ReadValue();
            isInputActive = true;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isInputActive = true;
        }
        else 
        {
            previousHit = null;
            if (flowLine.lineActive == true)
            {
                flowLine.ResetLine();
            }
        }

        if (isInputActive == true)
        {
            DetectHit(inputPosition);
        }
    }

    public void DetectHit(Vector2 mousePosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Image hit = result.gameObject.GetComponent<Image>();

            if (hit != null)
            {
                string hitName = hit.name;

                if (hitName.Contains("ColorFill"))
                {
                    return;
                }
                
                if (hitName.Contains("Empty"))
                {
                    if (flowLine.searchingDictionary.ContainsKey(hit))
                    {
                        flowLine.ResetLine();
                    }
                    else
                    {
                        flowLine.EmptyHit(hit);
                    }
                }
                else if (hitName.Contains("End") || hitName.Contains("Start"))
                {
                    if (hit == previousHit)
                    {
                        return;
                    }
                    if (flowLine.searchingDictionary.TryGetValue(hit, out int connectionId))
                    {
                        flowLine.DestroyConnection(connectionId);
                    }
                    else if (flowLine.currentLine.Contains(hit))
                    {
                        return;
                    }
                    else
                    {
                        flowLine.EndPointHit(hit);
                    }
                }
                else
                {
                    Debug.Log("Unknown Hit");
                }

                previousHit = hit;
            }
        }
    }
}
