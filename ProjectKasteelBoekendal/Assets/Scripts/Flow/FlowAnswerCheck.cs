using UnityEngine;

public class FlowAnswerCheck : MonoBehaviour
{
    public string point1;
    public string point2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAnswer()
    {
        if (point1 == point2)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }
}
