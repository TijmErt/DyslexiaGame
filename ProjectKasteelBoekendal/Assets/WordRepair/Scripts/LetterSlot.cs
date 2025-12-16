using UnityEngine;

// Represents a single slot in the word-repair UI where a letter tile can be placed.
// Responsible for holding a reference to the currently placed tile and
// positioning that tile correctly under this slot's transform.
public class LetterSlot : MonoBehaviour
{
    // The LetterTile currently placed in this slot (null if empty).
    public LetterTile currentTile;

    // Place the given tile into this slot.
    // This method stores the reference, parents the tile under the slot
    // so it follows slot movement/visibility, and resets the tile's
    // anchored position so it sits centered within the slot UI.
    public void SetTile(LetterTile tile)
    {
        currentTile = tile;

        // Make the tile a child of this slot's transform without modifying scale.
        tile.transform.SetParent(transform, false);

        // Ensure the tile is positioned at the slot's origin (centered in the slot).
        tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
