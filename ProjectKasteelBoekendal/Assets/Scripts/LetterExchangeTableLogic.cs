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
        int freeIndex = -1;
        for (int i = 0; i < _lettersOnSlots.Length; i++)
        {
            if (_lettersOnSlots[i] == null)
            {
                freeIndex = i;
                break;
            }
        }

        if (freeIndex == -1)
        {
            Debug.Log("ExchangeTable: all slots are full, can't place more letters.");
            return;
        }

        LetterItem letter = hand.RemoveHeldWithoutDropping();
        if (letter == null) return;

        Transform slot = _slots[freeIndex];

        letter.transform.SetParent(slot);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;

        // New home is here
        letter.SetHomeToCurrentTransform();

        _lettersOnSlots[freeIndex] = letter;
    }

    private void GiveToPlayer(PlayerHand hand)
    {
        // Find first occupied slot
        int occupiedIndex = -1;
        for (int i = 0; i < _lettersOnSlots.Length; i++)
        {
            if (_lettersOnSlots[i] != null)
            {
                occupiedIndex = i;
                break;
            }
        }

        if (occupiedIndex == -1)
        {
            Debug.Log("ExchangeTable: no letters available to pick up.");
            return;
        }

        LetterItem letter = _lettersOnSlots[occupiedIndex];
        _lettersOnSlots[occupiedIndex] = null;

        hand.TryPickUp(letter);
    }
}
