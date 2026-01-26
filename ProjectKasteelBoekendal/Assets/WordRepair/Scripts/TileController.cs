using System.Collections.Generic;
using UnityEngine;

// Controls tile selection and swapping behavior in the word-repair UI.
// Receives clicks from LetterTile instances, manages a single selected tile,
// performs swaps between slots, and triggers win/repair logic when appropriate.
public class TileController : MonoBehaviour
{
    // Reference to the WordManager which validates the current assembled word.
    [SerializeField] private WordManager wordManager;
    [SerializeField] private RepairSystem repairSystem;

    // Tracks the currently selected tile (null when nothing is selected).
    private LetterTile selectedTile;

    // Entry point called by a `LetterTile` when the player taps it.
    // Handles select/deselect and swapping logic depending on current state.
    public void OnTileClicked(LetterTile clickedTile)
    {
        // If no tile is selected yet, select the clicked tile.
        if (selectedTile == null)
        {
            SelectTile(clickedTile);
        }
        // If the clicked tile is the one already selected, deselect it.
        else if (selectedTile == clickedTile)
        {
            DeselectTile();
        }
        // If a different tile was clicked while one is selected, swap them.
        else
        {
            SwapTiles(selectedTile, clickedTile);
        }
    }

    // Mark a tile as selected and update its visual state.
    private void SelectTile(LetterTile tile)
    {
        selectedTile = tile;
        selectedTile.SetSelectedState(true); // provide visual feedback (color/scale)
    }

    // Clear selection and restore the tile's visual state.
    private void DeselectTile()
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelectedState(false); // remove visual selection cue
            selectedTile = null;
        }
    }

    // Swap the positions of two tiles by swapping their parent slot transforms.
    // After swapping, re-center them in their new slots and clear selection.
    // Finally, check if the current board now forms a correct word.
    private void SwapTiles(LetterTile tileA, LetterTile tileB)
    {
        // Remember each tile's parent (the slot it currently occupies).
        Transform parentA = tileA.transform.parent;
        Transform parentB = tileB.transform.parent;

        // Swap the parent transforms so the tiles exchange slots.
        tileA.transform.SetParent(parentB, false);
        tileB.transform.SetParent(parentA, false);

        // Reset anchored positions so each tile snaps to the center of its new slot.
        tileA.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        tileB.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // Clear the selection now that the swap is complete.
        DeselectTile();

        // After swapping, verify whether the assembled tiles form the correct word.
        bool correct = wordManager.CheckAnswer();

        if (correct)
        {
            // Notify the RepairSystem to process a successful repair/word completion.
            repairSystem.CompleteWord();
        }
    }
}
