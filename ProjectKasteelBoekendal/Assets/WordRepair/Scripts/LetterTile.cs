using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LetterTile : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private Image tileBackground;

    [SerializeField] private Color selectedColor = new Color(0.56f, 0.93f, 0.56f);
    [SerializeField] private Color emptyColor = new Color(0f, 0f, 0f);


    public char letterChar;

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

    public void SetUpEmpty(TileController tc)
    {
        if (controller == null)
            controller = tc;

        if (letterText != null)
            letterText.text = "_";

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
