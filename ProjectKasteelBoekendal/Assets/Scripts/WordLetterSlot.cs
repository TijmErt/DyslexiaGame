using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WordLetterSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerPos;

    private int _boardId;
    private int _indexInWord;

    private CwW_WordManager _wordManager;
    private LetterItem _currentLetter;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    /// <summary>
    /// Called by WordManager when setting up the current word.
    /// </summary>
    public void Init(CwW_WordManager manager, int boardId, int index)
    {
        _wordManager = manager;
        _boardId = boardId;
        _indexInWord = index;
        ClearSlot();
    }

    public void ClearSlot()
    {
        _currentLetter = null;
    }

    public Vector3 GetPlayerPosPoint(PlayerInteraction player)
    {
        if (_playerPos != null)
            return _playerPos.position;

        return transform.position;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player == null || player.Hand == null) return;

        // Player is trying to place held letter into this slot
        player.Hand.PlaceHeldIntoSlot(this);
    }

    public bool TryPlaceLetter(LetterItem letter)
    {
        if (letter == null) return false;
        if (_wordManager == null)
        {
            Debug.LogWarning("WordLetterSlot: No WordManager assigned.");
            return false;
        }

        if (_currentLetter != null)
        {
            // Slot already occupied
            return false;
        }

        char required = _wordManager.GetRequiredLetter(_boardId, _indexInWord);
        if (required == '\0') return false;

        char provided = char.ToUpperInvariant(letter.Letter);

        if (provided != required)
        {
            // wrong letter; could add feedback here
            return false;
        }

        _currentLetter = letter;

        // Snap the letter into the slot
        letter.transform.SetParent(transform);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;

        _wordManager.OnSlotFilledChanged(_boardId);
        return true;
    }

    public bool IsCorrect()
    {
        if (_currentLetter == null || _wordManager == null) return false;

        char required = _wordManager.GetRequiredLetter(_boardId, _indexInWord);
        if (required == '\0') return false;

        char provided = char.ToUpperInvariant(_currentLetter.Letter);
        return provided == required;
    }
}
