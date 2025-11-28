using System.Collections.Generic;
using UnityEngine;

public class CwW_WordManager : MonoBehaviour
{
    [Header("Words")]
    [SerializeField] private List<string> _possibleWords = new List<string>();

    [Header("Slots & Spawns")]
    [SerializeField] private List<WordLetterSlot> _slots = new List<WordLetterSlot>();
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private LetterItem _letterPrefab;

    public string CurrentWord { get; private set; }

    private readonly List<LetterItem> _spawnedLetters = new List<LetterItem>();

    private void Start()
    {
        StartNextWord();
    }

    public void StartNextWord()
    {
        ClearCurrentLettersAndSlots();

        CurrentWord = PickRandomWord();
        if (string.IsNullOrEmpty(CurrentWord))
        {
            Debug.LogError("WordManager: No valid word could be selected.");
            return;
        }

        SetupSlotsForWord(CurrentWord);
        SpawnLettersForWord(CurrentWord);
    }

    private string PickRandomWord()
    {
        if (_possibleWords == null || _possibleWords.Count == 0)
            return null;

        // Filter words that fit into available slots and spawn points
        List<string> candidates = new List<string>();
        foreach (string w in _possibleWords)
        {
            if (string.IsNullOrEmpty(w)) continue;

            int len = w.Length;
            if (len <= _slots.Count && len <= _spawnPoints.Count)
            {
                candidates.Add(w.ToUpperInvariant());
            }
        }

        if (candidates.Count == 0)
            return null;

        int idx = Random.Range(0, candidates.Count);
        return candidates[idx];
    }

    private void SetupSlotsForWord(string word)
    {
        int len = word.Length;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < len)
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].Init(this, i);
            }
            else
            {
                _slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void SpawnLettersForWord(string word)
    {
        int len = word.Length;

        for (int i = 0; i < len; i++)
        {
            Transform spawnPoint = _spawnPoints[i];
            LetterItem letterInstance = Instantiate(_letterPrefab, spawnPoint.position, spawnPoint.rotation);
            letterInstance.SetLetter(word[i]);
            _spawnedLetters.Add(letterInstance);
        }
    }

    private void ClearCurrentLettersAndSlots()
    {
        // Clear slots
        foreach (var slot in _slots)
        {
            if (slot != null)
            {
                slot.ClearSlot();
            }
        }

        // Destroy old letters
        foreach (var letter in _spawnedLetters)
        {
            if (letter != null)
            {
                Destroy(letter.gameObject);
            }
        }
        _spawnedLetters.Clear();
    }

    public void OnSlotFilledChanged()
    {
        if (IsWordCompleted())
        {
            Debug.Log("Finished word");
            StartNextWord();
        }
    }

    private bool IsWordCompleted()
    {
        if (string.IsNullOrEmpty(CurrentWord))
            return false;

        int len = CurrentWord.Length;

        for (int i = 0; i < len; i++)
        {
            if (i >= _slots.Count) return false;

            var slot = _slots[i];
            if (slot == null || !slot.IsCorrect())
                return false;
        }

        return true;
    }
}
