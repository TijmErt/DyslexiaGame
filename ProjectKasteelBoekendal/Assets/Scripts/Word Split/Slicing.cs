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
        if (Mouse.current.leftButton.isPressed && slicingEnabled == true)
        {
            UpdatePosition();
            DetectSlice();
        }
    }

    public void UpdatePosition()
    {
        mousePosition = Mouse.current.position.ReadValue();
        worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        knife.transform.position = mousePosition;
    }

    public void DetectSlice()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Mouse.current.position.ReadValue();

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


        //RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, rayDistance, sliceLayer);
        //Debug.Log(hit);
        //if (hit.collider != null)
        //{
            //Debug.Log("Hit: " + hit.collider.name);
            //Button btn = hit.collider.GetComponent<Button>();
            //if (btn != null)
            //{
                //btn.onClick.Invoke();
            //}
        //}
    }
}
