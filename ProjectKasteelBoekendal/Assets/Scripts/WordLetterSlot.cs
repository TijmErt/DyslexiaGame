using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WordLetterSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private int _indexInWord = 0;
    [SerializeField] private Transform targetPos;

    private CwW_WordManager _wordManager;
    private LetterItem _currentLetter;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Init(CwW_WordManager manager, int index)
    {
        _wordManager = manager;
        _indexInWord = index;
        ClearSlot();
    }

    public void ClearSlot()
    {
        _currentLetter = null;
    }

    public Vector3 GetPlayerPosPoint()
    {
        return targetPos.position;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player == null || player.Hand == null) return;

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

        string currentWord = _wordManager.CurrentWord;
        if (string.IsNullOrEmpty(currentWord) ||
            _indexInWord < 0 ||
            _indexInWord >= currentWord.Length)
        {
            return false;
        }

        char required = char.ToUpperInvariant(currentWord[_indexInWord]);
        char provided = char.ToUpperInvariant(letter.Letter);

        if (provided != required)
        {
            // wrong letter
            return false;
        }

        _currentLetter = letter;

        // Snap the letter into the slot
        letter.transform.SetParent(transform);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;

        //disable its collider so it’s not interactable anymore
        Collider letterCol = letter.GetComponent<Collider>();
        if (letterCol != null)
            letterCol.enabled = false;

        _wordManager.OnSlotFilledChanged();
        return true;
    }

    public bool IsCorrect()
    {
        if (_currentLetter == null || _wordManager == null) return false;

        string word = _wordManager.CurrentWord;
        if (string.IsNullOrEmpty(word) ||
            _indexInWord < 0 ||
            _indexInWord >= word.Length)
        {
            return false;
        }

        char required = char.ToUpperInvariant(word[_indexInWord]);
        char provided = char.ToUpperInvariant(_currentLetter.Letter);
        return provided == required;
    }
}
