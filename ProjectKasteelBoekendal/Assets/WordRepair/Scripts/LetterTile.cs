using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public TextMeshProUGUI letterText;
    public Image tileBackground;
    [Header("Appearance")]
    public Color selectedColor = new Color(0.56f, 0.93f, 0.56f);

    [Header("Data")]
    public char letterChar;
    
    // Internal References
    private TileController controller;
    private RectTransform rectTransform;
    private Color originalColor;

    public void Setup(char c, TileController tc)
    {
        letterChar = c;
        controller = tc;
        
        if (letterText != null) 
            letterText.text = c.ToString();

        rectTransform = GetComponent<RectTransform>();
        
        if (tileBackground != null) 
            originalColor = tileBackground.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller != null)
        {
            controller.OnTileClicked(this);
        }
    }

    public void SetSelectedState(bool isSelected)
    {
        if (tileBackground != null)
        {
            // change color to green when selected
            tileBackground.color = isSelected ? selectedColor : originalColor;
            
            // make bigger when selected
            transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
