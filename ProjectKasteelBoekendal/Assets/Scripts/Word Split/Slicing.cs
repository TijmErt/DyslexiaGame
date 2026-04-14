using UnityEngine;
using UnityEngine.UI;

public class Slicing : MonoBehaviour
{

    public Image knife;
    public bool slicingEnabled = true;

    public Vector2 mousePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetMouseButton(0) && slicingEnabled == true)
        {
            UpdatePosition();
            if (isCollidingWithButton())
            {
                //run script
            }
        }
    }

    public void UpdatePosition()
    {
        mousePosition = Input.mousePosition;
        knife.transform.position = mousePosition;
    }

    bool isCollidingWithButton()
    {
        
    }
}
