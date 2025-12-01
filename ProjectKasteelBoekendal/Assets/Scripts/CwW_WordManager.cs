using System.Collections.Generic;
using UnityEngine;

public class CwW_WordManager : MonoBehaviour
{
    [Header("Word List (shared by all boards)")]
    [SerializeField] private List<string> _possibleWords = new List<string>();

    [Header("Boards (one per player/section)")]
    [SerializeField] private List<WordBoard> _boards = new List<WordBoard>();

    [Header("Letter Prefab")]
    [SerializeField] private LetterItem _letterPrefab;

    private void Start()
    {
        // Start a word for each board
        for (int i = 0; i < _boards.Count; i++)
        {
            StartNextWord(i);
        }
    }

    // Called by slots when they change state
    public void OnSlotFilledChanged(int boardId)
    {
        if (IsWordCompleted(boardId))
        {
            StartNextWord(boardId);
        }
    }

    // Get required letter for a board & index (used by slots)
    public char GetRequiredLetter(int boardId, int indexInWord)
    {
        if (boardId < 0 || boardId >= _boards.Count) return '\0';

        var board = _boards[boardId];
        if (string.IsNullOrEmpty(board.CurrentWord)) return '\0';
        if (indexInWord < 0 || indexInWord >= board.CurrentWord.Length) return '\0';

        return char.ToUpperInvariant(board.CurrentWord[indexInWord]);
    }

    public string GetCurrentWord(int boardId)
    {
        if (boardId < 0 || boardId >= _boards.Count) return null;
        return _boards[boardId].CurrentWord;
    }

    // ------------ Internal board management ------------

    private void StartNextWord(int boardId)
    {
        if (boardId < 0 || boardId >= _boards.Count) return;

        ClearBoard(boardId);

        string word = PickRandomWord();
        if (string.IsNullOrEmpty(word))
        {
            Debug.LogError($"WordManager: No valid word for board {boardId}.");
            return;
        }

        word = word.ToUpperInvariant();

        var board = _boards[boardId];
        board.CurrentWord = word;

        SetupSlotsForWord(boardId, word);
        SpawnLettersForWord(boardId, word);
    }

    private string PickRandomWord()
    {
        if (_possibleWords == null || _possibleWords.Count == 0)
            return null;

        // You could filter here based on max length etc. For now, pick any non-empty.
        List<string> candidates = new List<string>();
        foreach (string w in _possibleWords)
        {
            if (string.IsNullOrEmpty(w)) continue;
            candidates.Add(w);
        }

        if (candidates.Count == 0) return null;

        int idx = Random.Range(0, candidates.Count);
        return candidates[idx];
    }

    private void SetupSlotsForWord(int boardId, string word)
    {
        var board = _boards[boardId];
        int len = word.Length;

        for (int i = 0; i < board.Slots.Count; i++)
        {
            if (i < len)
            {
                board.Slots[i].gameObject.SetActive(true);
                board.Slots[i].Init(this, boardId, i);
            }
            else
            {
                board.Slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void SpawnLettersForWord(int boardId, string word)
    {
        var board = _boards[boardId];
        int len = word.Length;

        if (board.SpawnPoints.Count < len)
        {
            Debug.LogWarning($"WordManager: Board {boardId} doesn't have enough spawn points for word '{word}'.");
        }

        for (int i = 0; i < len; i++)
        {
            if (i >= board.SpawnPoints.Count) break;

            Transform spawnPoint = board.SpawnPoints[i];
            LetterItem letterInstance = Instantiate(_letterPrefab, spawnPoint.position, spawnPoint.rotation);
            letterInstance.SetLetter(word[i]);

            board.SpawnedLetters.Add(letterInstance);
        }
    }

    private void ClearBoard(int boardId)
    {
        var board = _boards[boardId];

        // Clear slots
        foreach (var slot in board.Slots)
        {
            if (slot != null)
                slot.ClearSlot();
        }

        // Destroy old letters
        foreach (var letter in board.SpawnedLetters)
        {
            if (letter != null)
                Destroy(letter.gameObject);
        }

        board.SpawnedLetters.Clear();
        board.CurrentWord = null;
    }

    private bool IsWordCompleted(int boardId)
    {
        var board = _boards[boardId];
        if (string.IsNullOrEmpty(board.CurrentWord)) return false;

        int len = board.CurrentWord.Length;

        for (int i = 0; i < len; i++)
        {
            if (i >= board.Slots.Count) return false;

            var slot = board.Slots[i];
            if (slot == null || !slot.IsCorrect())
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class WordBoard
{
    public string Name;

    [Tooltip("Slots that belong to this board (in word order).")]
    public List<WordLetterSlot> Slots = new List<WordLetterSlot>();

    [Tooltip("Spawn points in this board's area for its letters.")]
    public List<Transform> SpawnPoints = new List<Transform>();

    [HideInInspector] public string CurrentWord;
    [HideInInspector] public List<LetterItem> SpawnedLetters = new List<LetterItem>();
}
