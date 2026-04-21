using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Slicing : MonoBehaviour
{

    public Image knife;
    public bool slicingEnabled = true;
    private Collider2D sliceCollider;
    public float rayDistance = 0.1f;
    public LayerMask sliceLayer;

    public Vector2 mousePosition;
    public Vector2 worldPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        if (isInputActive && slicingEnabled == true)
        {
            UpdatePosition(inputPosition);
            DetectSlice(inputPosition);
        }
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        mousePosition = screenPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            knife.canvas.transform as RectTransform,
            screenPosition,
            knife.canvas.worldCamera,
            out var localPoint
        );

        knife.rectTransform.localPosition = localPoint;
    }
    
    // public void UpdatePosition(Vector2 screenPosition)
    // {
    //     mousePosition = screenPosition;
    //     worldPos = Camera.main.ScreenToWorldPoint(screenPosition);

    //     knife.transform.position = screenPosition;
    // }

    public void DetectSlice(Vector2 screenPosition)
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Button btn = result.gameObject.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.Invoke();
                Debug.Log("Sliced UI Button: " + btn.name);
                slicingEnabled = false; // disable slicing after a successful slice
                break; // prevents multiple triggers per frame
            }
        }
    }
}
