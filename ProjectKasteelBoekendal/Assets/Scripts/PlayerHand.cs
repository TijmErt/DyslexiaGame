using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public LetterItem HeldItem { get; private set; }

    public void TryPickUp(LetterItem newItem)
    {
        if (newItem == null) return;

        if (HeldItem == newItem) return;

        // Drop currently held item back to home
        if (HeldItem != null)
        {
            HeldItem.DropToHome();
        }

        HeldItem = newItem;
        newItem.OnPickedUp(this);
    }

    public void DropHeldToHome()
    {
        if (HeldItem != null)
        {
            HeldItem.DropToHome();
            HeldItem = null;
        }
    }

    public void PlaceHeldIntoSlot(WordLetterSlot slot)
    {
        if (slot == null || HeldItem == null) return;

        bool placed = slot.TryPlaceLetter(HeldItem);
        if (placed)
        {
            HeldItem = null;
        }
    }
}
