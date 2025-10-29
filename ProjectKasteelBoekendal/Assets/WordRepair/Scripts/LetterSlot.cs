using Unity.Mathematics;
using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public LetterTile currentTile;
    private char correctLetter;

    public void SetTile(LetterTile tile)
    {
        currentTile = tile;
        tile.transform.SetParent(transform);
        tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private bool CheckLetter()
    {
        return currentTile.letterChar == correctLetter;
    }
}
