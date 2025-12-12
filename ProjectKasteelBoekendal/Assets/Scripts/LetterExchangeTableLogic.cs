using System.Collections.Generic;
using UnityEngine;

public class LetterExchangeTableLogic : MonoBehaviour
{
    [Header("Slots where letters are displayed")]
    [SerializeField] private List<Transform> _slots = new List<Transform>();

    private LetterItem[] _lettersOnSlots;

    private void Awake()
    {
        _lettersOnSlots = new LetterItem[_slots.Count];
    }

    public void InteractWith(PlayerHand inventory)
    {
        if (inventory == null) return;

        if (inventory.HasItem)
        {
            PlaceFromPlayer(inventory);
        }
        else
        {
            GiveToPlayer(inventory);
        }
    }

    private void PlaceFromPlayer(PlayerHand hand)
    {
        // Find first free slot
        int freeIndex = CheckForLettersOnSlot();

        if (freeIndex == -1)
        {
            Debug.Log("ExchangeTable: all slots are full, can't place more letters.");
            return;
        }

        LetterItem letter = hand.RemoveHeldWithoutDropping();
        if (letter == null) return;

        Transform slot = _slots[freeIndex];

        SetLetterLocation(letter);

        // New home is here
        letter.SetHomeToCurrentTransform();

        _lettersOnSlots[freeIndex] = letter;
    }

    private void SetLetterLocation(LetterItem letter)
    {
        letter.transform.SetParent(transform);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;
    }

    private void GiveToPlayer(PlayerHand hand)
    {
        // Find first occupied slot
        int occupiedIndex = CheckForLettersOnSlot();

        if (occupiedIndex == -1)
        {
            Debug.Log("ExchangeTable: no letters available to pick up.");
            return;
        }

        LetterItem letter = _lettersOnSlots[occupiedIndex];
        _lettersOnSlots[occupiedIndex] = null;

        hand.TryPickUp(letter);
    }

    private int CheckForLettersOnSlot()
    {
        for (int i = 0; i < _lettersOnSlots.Length; i++)
        {
            if (_lettersOnSlots[i] != null)
            {
                return i;
            }
        }
        return -1;
    }
}
