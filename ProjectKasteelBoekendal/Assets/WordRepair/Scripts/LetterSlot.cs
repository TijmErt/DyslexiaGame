using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public LetterTile currentTile;

    public void SetTile(LetterTile tile)
    {
        currentTile = tile;
        tile.transform.SetParent(transform, false);
        tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
