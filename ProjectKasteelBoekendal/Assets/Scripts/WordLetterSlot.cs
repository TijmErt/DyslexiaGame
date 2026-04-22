using UnityEngine;
using UnityEngine.Rendering;

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

        var hand = player.Hand;

        if (hand.HasItem)
        {
            hand.PlaceHeldIntoSlot(this);
        }
        else
        {
            if (_currentLetter == null) return;
            LetterItem letter = _currentLetter;
            _currentLetter = null;

            hand.TryPickUp(letter);

            if (_wordManager == null) return;
            _wordManager.OnSlotFilledChanged(_boardId);
        }
    }

    public float InteractionDistance { get; set;}

    public bool TryPlaceLetter(LetterItem letter)
    {
        if (letter == null) return false;
        if (_wordManager == null)
        {
            Debug.LogWarning("WordLetterSlot: No WordManager assigned.");
            return false;
        }

        if (_currentLetter != null)
            return false;

        char required = _wordManager.GetRequiredLetter(_boardId, _indexInWord);
        if (required == '\0') return false;

        char provided = char.ToUpperInvariant(letter.Letter);

        _currentLetter = letter;

        SetLetterLocation(letter);

        _wordManager.OnSlotFilledChanged(_boardId);
        return true;
    }

    private void SetLetterLocation(LetterItem letter)
    {
        letter.transform.SetParent(transform);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;
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
