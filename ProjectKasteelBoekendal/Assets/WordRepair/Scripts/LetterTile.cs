using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// UI component representing a single letter tile used in the word repair gameplay.
// Handles displaying the character, reporting clicks to the controller, and
// visual selection state (color + scale).
public class LetterTile : MonoBehaviour, IPointerClickHandler
{
    // Text element that shows the tile's letter (assign in inspector).
    [SerializeField] private TextMeshProUGUI letterText;

    // Background image used to tint the tile when selected/empty (assign in inspector).
    [SerializeField] private Image tileBackground;

    // Color used to indicate the selected state (greenish by default).
    [SerializeField] private Color selectedColor = new Color(0.56f, 0.93f, 0.56f);

    // The character this tile represents (public so other systems can read it).
    public char letterChar;

    private TileController controller;

    // The original background color, stored so we can restore it when unselected.
    private Color originalColor;

    // Initialize the tile with a character and a controller reference.
    // Updates the visible text and saves the
    // background's original color for later restoration.
    public void Setup(char c, TileController tc)
    {
        letterChar = c;
        controller = tc;

        if (letterText != null)
            letterText.text = c.ToString();

        if (tileBackground != null)
            originalColor = tileBackground.color;
    }

    // Configure the tile as an empty placeholder (shows underscore) and
    // ensure it has a controller reference. Useful for slots with no letter.
    public void SetUpEmpty(TileController tc)
    {
        if (controller == null)
            controller = tc;

        if (letterText != null)
            letterText.text = "_";

        if (tileBackground != null)
            originalColor = tileBackground.color;
    }

    // IPointerClickHandler implementation. When the tile is clicked, forward
    // the event to the TileController so it can handle selection and game logic.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller != null)
        {
            controller.OnTileClicked(this);
        }
    }

    // Set the visual selected state of the tile. When selected we tint the
    // background and scale the tile up slightly; when unselected we restore
    // the original color and scale.
    public void SetSelectedState(bool isSelected)
    {
        if (tileBackground != null)
        {
            // change color to the selected color when selected, otherwise restore.
            tileBackground.color = isSelected ? selectedColor : originalColor;

            // scale up the tile to give a visual cue when selected.
            transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
