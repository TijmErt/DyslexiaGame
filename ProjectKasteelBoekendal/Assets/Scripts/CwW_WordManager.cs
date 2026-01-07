using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CwW_WordManager : MonoBehaviour
{
    [Header("Word List (shared by all boards)")]
    [SerializeField] private List<string> _possibleWords = new List<string>();

    [Header("Boards (one per player/section)")]
    [SerializeField] private List<WordBoard> _boards = new List<WordBoard>();

    [Header("Shared spawn points (used by all boards)")]
    [SerializeField] private List<Transform> _sharedSpawnPoints = new List<Transform>();
    
    [Header("Letter Prefab")]
    [SerializeField] private LetterItem _letterPrefab;

    [Header("UI (TMP)")]
    [SerializeField] private TMP_Text _p1WordText;
    [SerializeField] private TMP_Text _p2WordText;
    [SerializeField] private TMP_Text _totalSolvedText;

    [Header("End Game UI")]
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TMP_Text _endSolvedText;
    [SerializeField] private string _nextSceneName;

    [Header("Win Condition")]
    [SerializeField] private int _targetSolvedWords = 10;

    private int _totalSolvedWords = 0;
    private bool _gameEnded = false;

    private void Start()
    {
        if (_endPanel != null)
            _endPanel.SetActive(false);

        _totalSolvedWords = 0;
        UpdateTotalSolvedUI();

        for (int i = 0; i < _boards.Count; i++)
        {
            StartNextWord(i);
            UpdateWordUI(i);
        }
    }

    public void OnSlotFilledChanged(int boardId)
    {
        if (_gameEnded) return;

        if (IsWordCompleted(boardId))
        {
            _totalSolvedWords++;
            UpdateTotalSolvedUI();

            if (_totalSolvedWords >= _targetSolvedWords)
            {
                EndGame();
                return;
            }

            StartNextWord(boardId);
            UpdateWordUI(boardId);
        }
    }

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
        if (_gameEnded) return;

        ClearBoard(boardId);
        string word = ChooseNextWord(boardId);
        while (_boards[0].CurrentWord == _boards[1].CurrentWord)
        {
            word = ChooseNextWord(boardId);
        }

        SetupSlotsForWord(boardId, word);
        SpawnLettersForWord(boardId, word);

        UpdateWordUI(boardId);
    }
    private string ChooseNextWord(int boardId)
    {
        string word = PickRandomWord();
        if (string.IsNullOrEmpty(word))
        {
            return ChooseNextWord(boardId);
        }

        word = word.ToUpperInvariant();

        var board = _boards[boardId];
        board.CurrentWord = word;
        return word;
    }

    private string PickRandomWord()
    {
        if (_possibleWords == null || _possibleWords.Count == 0)
            return null;

        List<string> candidates = new List<string>();
        foreach (string w in _possibleWords)
        {
            if (string.IsNullOrEmpty(w)) continue;
            candidates.Add(w);
        }

        if (candidates.Count == 0) return null;

        int index = Random.Range(0, candidates.Count);
        return candidates[index];
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

        if (_sharedSpawnPoints == null || _sharedSpawnPoints.Count == 0)
        {
            Debug.LogError("WordManager: No shared spawn points assigned.");
            return;
        }

        // 1. Collect FREE spawnpoints (no LetterItem as child)
        List<int> freeIndices = GetFreeSpawnPoints();

        if (!CheckFreeSpawnPoints(freeIndices, len, boardId, word)) return;

        // 2. Shuffle the free indices
        for (int i = 0; i < freeIndices.Count; i++)
        {
            int swapIndex = Random.Range(i, freeIndices.Count);
            int temp = freeIndices[i];
            freeIndices[i] = freeIndices[swapIndex];
            freeIndices[swapIndex] = temp;
        }

        // 3. Spawn letters on the first N free points
        int lettersToSpawn = Mathf.Min(len, freeIndices.Count);
        for (int i = 0; i < lettersToSpawn; i++)
        {
            Transform spawnPoint = _sharedSpawnPoints[freeIndices[i]];

            LetterItem letterInstance = Instantiate(
                _letterPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            letterInstance.transform.SetParent(spawnPoint);
            letterInstance.SetLetter(word[i]);

            board.SpawnedLetters.Add(letterInstance);
        }
    }

    private List<int> GetFreeSpawnPoints()
    {
        List<int> freeIndices = new List<int>();
        for (int i = 0; i < _sharedSpawnPoints.Count; i++)
        {
            Transform sp = _sharedSpawnPoints[i];
            if (sp == null)
                continue;

            bool occupied = CheckLetterItemForChildren(sp);

            if (!occupied)
            {
                freeIndices.Add(i);
            }
        }
        return freeIndices;
    }
    private bool CheckFreeSpawnPoints(List<int> fsp, int len, int boardId, string word)
    {
        if (fsp.Count == 0)
        {
            Debug.LogWarning(
                $"WordManager: No FREE spawn points available to spawn word '{word}' " +
                $"for board {boardId}."
            );
            return false;
        }
        if (fsp.Count < len)
        {
            Debug.LogWarning(
                $"WordManager: Not enough FREE spawn points ({fsp.Count}) " +
                $"to spawn full word '{word}' (length {len}) for board {boardId}. " +
                "Some letters will not be spawned."
            );
            return false;
        }
        return true;
    }

    private bool CheckLetterItemForChildren(Transform sp)
    {
        for (int c = 0; c < sp.childCount; c++)
        {
            if (sp.GetChild(c).GetComponent<LetterItem>() != null) 
                return true;
        }
        return false;
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

    // ---------- UI ----------
    private void UpdateWordUI(int boardId)
    {
        if (boardId == 0 && _p1WordText != null)
            _p1WordText.text = GetCurrentWord(0) ?? "";

        if (boardId == 1 && _p2WordText != null)
            _p2WordText.text = GetCurrentWord(1) ?? "";
    }

    private void UpdateTotalSolvedUI()
    {
        if (_totalSolvedText != null)
            _totalSolvedText.text = $"Solved: {_totalSolvedWords}/{_targetSolvedWords}";
    }

    private void EndGame()
    {
        _gameEnded = true;

        if (_endPanel != null)
            _endPanel.SetActive(true);

        if (_endSolvedText != null)
            _endSolvedText.text = $"Solved: {_totalSolvedWords}/{_targetSolvedWords}";
    }
    public void FinishMinigame()
    {
        if (!_gameEnded) return;
        
        if (!string.IsNullOrEmpty(_nextSceneName))
            SceneManager.LoadScene(_nextSceneName);
        else
            Debug.LogWarning("WordManager: Next scene name is empty.");
        
    }
}

[System.Serializable]
public class WordBoard
{
    public string Name;

    [Tooltip("Slots that belong to this board (in word order).")]
    public List<WordLetterSlot> Slots = new List<WordLetterSlot>();

    [HideInInspector] public string CurrentWord;
    [HideInInspector] public List<LetterItem> SpawnedLetters = new List<LetterItem>();
}
