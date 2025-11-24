using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for Layout updates if you use them

public class WordManager : MonoBehaviour
{
    [System.Serializable]
    public class WordData
    {
        public string word;
        public Sprite image;
    }

    [Header("Configuration")]
    public List<WordData> words;
    public RoundType currentRoundType { get; private set; }

    [Header("References")]
    // 1. NEW: Reference to the TileController so we can pass it to new tiles
    public TileController tileController; 
    public Transform letterParent;
    public GameObject letterTilePrefab;
    public GameObject letterSlotPrefab;
    public GameObject customer;

    private string currentWord;
    private List<LetterTile> currentTiles = new List<LetterTile>();
    private int currentIndex = 0;

    void Start()
    {
        currentRoundType = RoundType.Preparation;
        LoadNextWord();
    }

    public void LoadNextWord()
    {
        if (currentIndex >= words.Count) currentIndex = 0;

        // remove old slots/tiles
        foreach (Transform child in letterParent)
            Destroy(child.gameObject);
        currentTiles.Clear();

        // load new word
        WordData w = words[currentIndex];
        currentWord = w.word.ToUpper();

        if (customer != null)
        {
            Customer custScript = customer.GetComponent<Customer>();
            if (custScript != null)
            {
                custScript.orderImage = w.image;
                custScript.NewOrder();
            }
        }

        // mix up letters in word
        List<char> scrambled = new List<char>(currentWord.ToCharArray());
        scrambled.Shuffle();

        foreach (char c in scrambled)
        {
            GameObject slot = CreateSlot();
            GameObject tile = CreateTile(slot.transform);

            LetterTile lt = tile.GetComponent<LetterTile>();
            if (lt == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            // 2. UPDATED: We now pass the TileController to the tile
            lt.Setup(c, tileController);
            
            currentTiles.Add(lt);
        }
    }

    // ... (GetCurrentWord and GetPlayerAnswer remain the same) ...
    public string GetCurrentWord() => currentWord;

    public string GetPlayerAnswer()
    {
        string result = "";
        foreach (Transform child in letterParent)
        {
            // child is the slot; find the tile inside it
            LetterTile tile = child.GetComponentInChildren<LetterTile>();
            if (tile != null)
            {
                result += tile.letterChar;
            }
        }
        return result;
    }

    public bool CheckAnswer()
    {
        string playerAnswer = GetPlayerAnswer();
        bool correct = playerAnswer == currentWord;
        
        Debug.Log($"Checking: {playerAnswer} vs {currentWord} = {correct}");

        if (correct)
        {
            // Optional: Auto-advance if correct?
            Debug.Log("Word Complete!");
            // You might want to trigger a win animation here or call Next() after a delay
        }

        return correct;
    }

    public void Next()
    {
        currentIndex++;
        // Toggle round type logic
        currentRoundType = currentRoundType == RoundType.Preparation ? RoundType.Repair : RoundType.Preparation;
        Debug.Log("Next Round Type: " + currentRoundType);
        LoadNextWord();
    }

    // ... (CreateSlot and CreateTile remain the same) ...
    private GameObject CreateSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, letterParent);
        RectTransform slotRt = slot.GetComponent<RectTransform>();
        if (slotRt != null) slotRt.anchoredPosition = Vector2.zero;
        return slot;
    }

    private GameObject CreateTile(Transform slot)
    {
        GameObject tile = Instantiate(letterTilePrefab, slot);
        RectTransform tileRt = tile.GetComponent<RectTransform>();
        if (tileRt != null) tileRt.anchoredPosition = Vector2.zero;
        return tile;
    }
}

// Keep your extension class
public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}