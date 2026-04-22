using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenEdit : MonoBehaviour
{
    public TextMeshProUGUI dynamicText;
    public TextMeshProUGUI dynamicNumber;
    public TextMeshProUGUI coinsCollected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEndScreen(string dynamicTextContent, string dynamicNumberContent, string coinsCollectedAmount)
    {
        dynamicText.text = dynamicTextContent;
        dynamicNumber.text = dynamicNumberContent;
        coinsCollected.text = coinsCollectedAmount;
    }

    public void Restart()
    {
        Debug.Log("Restarting...");
    }

    public void Continue()
    {
        Debug.Log("Continuing...");
    }
}
