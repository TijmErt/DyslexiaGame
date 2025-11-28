using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TileController : MonoBehaviour
{
    private LetterTile selectedTile;

    // Called by LetterTile when the player taps it
    public void OnTileClicked(LetterTile clickedTile)
    {
        // 1. If nothing is selected yet, select the clicked tile
        if (selectedTile == null)
        {
            SelectTile(clickedTile);
        }
        // 2. If the player clicked the SAME tile again, deselect it
        else if (selectedTile == clickedTile)
        {
            DeselectTile();
        }
        // 3. If we have a selected tile and clicked a DIFFERENT one, swap them
        else
        {
            SwapTiles(selectedTile, clickedTile);
        }
    }

    private void SelectTile(LetterTile tile)
    {
        selectedTile = tile;
        selectedTile.SetSelectedState(true); // Visual feedback
        Debug.Log($"Selected: {tile.letterChar}");
    }

    private void DeselectTile()
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelectedState(false); // Remove visual feedback
            selectedTile = null;
        }
    }

    private void SwapTiles(LetterTile tileA, LetterTile tileB)
    {
        Debug.Log($"Swapping {tileA.letterChar} with {tileB.letterChar}");

        // 1. Get the parents (Slots)
        Transform parentA = tileA.transform.parent;
        Transform parentB = tileB.transform.parent;

        // 2. Swap the parents
        // We use SetParent(parent, false) to keep the local scale/rotation but reset position logic
        tileA.transform.SetParent(parentB, false);
        tileB.transform.SetParent(parentA, false);

        // 3. Reset positions so they snap to the center of the new slot
        // (Assumes the slots are RectTransforms)
        tileA.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        tileB.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // 4. Clear the selection
        DeselectTile();

        // Optional: Check for win condition here
        // CheckWordValidity();
    }
}
