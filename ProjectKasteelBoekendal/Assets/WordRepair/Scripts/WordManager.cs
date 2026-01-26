using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

// Manages loading words, preparing letter tiles/slots, shuffling letters,
// and providing answer validation used by the gameplay systems.
public class WordManager : MonoBehaviour
{
    // Internal serializable container for word + image pairs defined in the inspector.
    [System.Serializable]
    private class WordData
    {
        public string word;
        public Sprite image;
    }

    // List of words (with optional images) configured in the inspector.
    [SerializeField] private List<WordData> words;

    // References to other systems and prefabs used to create tiles/slots.
    [SerializeField] private TileController tileController;
    [SerializeField] private Transform answerParent;     // parent transform for answer slots
    [SerializeField] private Transform letterParent;     // parent transform for scrambled letter slots
    [SerializeField] private GameObject letterTilePrefab; // prefab for a populated letter tile
    [SerializeField] private GameObject emptyTilePrefab;  // prefab for an empty/placeholder tile
    [SerializeField] private GameObject letterSlotPrefab; // prefab for a slot (used for both answer and letter areas)
    [SerializeField] private GameObject customer;         // optional customer object to show order images

    // State for the currently active word and tiles.
    private string currentWord;
    private string[] otherWords = new string[2]; // two additional words used to mix up letters
    private List<LetterTile> currentTiles = new List<LetterTile>();
    private int currentIndex = 0; // index into the `words` list

    void Start()
    {
        // Shuffle the word list to present words in a random order each run.
        words.Shuffle();
        LoadNextWord();
    }

    // Load the next word's tiles/slots into the scene.
    public void LoadNextWord()
    {
        if (currentIndex >= words.Count) currentIndex = 0;

        // remove old slots/tiles from previous word
        foreach (Transform child in letterParent)
            Destroy(child.gameObject);
        foreach (Transform child in answerParent)
            Destroy(child.gameObject);
        currentTiles.Clear();

        // get the current word data and normalize to lowercase for comparisons
        WordData currentWordData = words[currentIndex];
        currentWord = currentWordData.word.ToLower();

        // pick two other words to use when creating scrambled letters
        GetOtherWords();

        // set the customer's requested image (if a customer GameObject is assigned)
        SetCustomerRequestImage(currentWordData);

        // create slots and letter tiles for the current round
        MixUpTiles();
    }

    // If a `customer` GameObject is set, assign the request image and trigger its UI.
    private void SetCustomerRequestImage(WordData currentWordData)
    {
        if (customer != null)
        {
            Customer customerScript = customer.GetComponent<Customer>();
            if (customerScript != null)
            {
                customerScript.orderImage = currentWordData.image;
                customerScript.NewOrder();
            }
        }
    }

    // Prepare the pool of scrambled letters and create tiles/slots.
    private void MixUpTiles()
    {
        int otherWordCount = 2;

        // Start with letters from the current word and the first other word.
        List<char> scrambled = new List<char>(currentWord.ToCharArray());
        scrambled.AddRange(otherWords[0].ToLower().ToCharArray());

        int max = GetLongestWordLength();

        // Ensure there are enough scrambled tiles by padding with random letters
        // until we have `max * otherWordCount` letters available.
        if (scrambled.Count < max * otherWordCount)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            System.Random rand = new System.Random();
            while (scrambled.Count < max * otherWordCount)
            {
                char randomChar = alphabet[rand.Next(alphabet.Length)];
                scrambled.Add(randomChar);
            }
        }

        // Randomize the scrambled letter order.
        scrambled.Shuffle();

        // Create the empty answer slots (placeholders) and the scrambled letter tiles.
        CreateEmptyTile();
        CreateLetterTile(scrambled);

    }

    // Instantiate letter tile prefabs for each character in the provided list.
    private void CreateLetterTile(List<char> charList)
    {
        foreach (char c in charList)
        {
            GameObject slot = CreateSlot();                // create a visual slot under the letter area
            GameObject tile = CreateTile(slot.transform);  // create a tile as child of that slot

            LetterTile letterTile = tile.GetComponent<LetterTile>();
            if (letterTile == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            // Initialize the tile with its character and the tile controller.
            letterTile.Setup(c, tileController);

            currentTiles.Add(letterTile);
        }
    }

    // Create empty answer slots and populate them with empty tile placeholders.
    private void CreateEmptyTile()
    {
        foreach (char c in currentWord)
        {
            GameObject slot = CreateAnswerSlot();                 // slot in the answer area
            GameObject tile = CreateEmptyTile(slot.transform);    // empty tile prefab as placeholder

            LetterTile letterTile = tile.GetComponent<LetterTile>();
            if (letterTile == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            // Configure the tile as an empty placeholder and give it the controller.
            letterTile.SetUpEmpty(tileController);

            currentTiles.Add(letterTile);
        }
    }

    // Determine the longest length between the current word and the other two words.
    private int GetLongestWordLength()
    {
        int maxLength = currentWord.Length;
        for (int i = 0; i < otherWords.Length; i++)
        {
            if (otherWords[i].Length > maxLength)
                maxLength = otherWords[i].Length;
        }

        return maxLength;
    }

    // Select two other words to use when generating scrambled tiles.
    // NOTE: the current implementation of GetRandomIndex is deterministic
    // (it just picks the next index), so this will not be fully random.
    private void GetOtherWords()
    {
        int wordIndex = GetRandomIndex(currentIndex);

        otherWords[0] = words[wordIndex].word;
        otherWords[1] = words[GetRandomIndex(wordIndex)].word;
    }

    // Returns a random index different from `index`. If there's only one word, return the same index.
    private int GetRandomIndex(int index)
    {
        if (words == null || words.Count <= 1)
            return index;

        int randIndex = index;
        // Keep picking until we get a different index
        while (randIndex == index)
        {
            randIndex = UnityEngine.Random.Range(0, words.Count);
        }
        return randIndex;
    }

    // Read the player's currently assembled answer by reading the letters
    // placed in the answerParent slots from left-to-right.
    public string GetPlayerAnswer()
    {
        string result = "";

        foreach (Transform slotTransform in answerParent)
        {
            LetterTile tile = slotTransform.GetComponentInChildren<LetterTile>();

            if (tile != null)
            {
                result += tile.letterChar;
            }
        }
        return result;
    }

    // Return true when the player's assembled answer exactly matches `currentWord`.
    // The answer is read via `GetPlayerAnswer()` (letters from the answer slots).
    public bool CheckAnswer()
    {
        string playerAnswer = GetPlayerAnswer();

        return playerAnswer == currentWord;
    }

    // Advance to the next word (increments index and reloads).
    public void Next()
    {
        string playerAnswer = GetPlayerAnswer();

        Debug.Log($"Player answers: {playerAnswer} for word {currentWord}");

        currentIndex++;
        LoadNextWord();
    }

    // Helper methods to instantiate slots and tiles, ensuring they are centered.
    private GameObject CreateSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, letterParent);
        RectTransform slotRt = slot.GetComponent<RectTransform>();
        if (slotRt != null) slotRt.anchoredPosition = Vector2.zero;
        return slot;
    }

    private GameObject CreateAnswerSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, answerParent);
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

    private GameObject CreateEmptyTile(Transform slot)
    {
        GameObject tile = Instantiate(emptyTilePrefab, slot);
        RectTransform tileRt = tile.GetComponent<RectTransform>();
        if (tileRt != null) tileRt.anchoredPosition = Vector2.zero;
        return tile;
    }
}

// Simple list shuffler extension used to randomize words/letters.
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