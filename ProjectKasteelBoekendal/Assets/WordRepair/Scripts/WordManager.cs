using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    [System.Serializable]
    public class WordData
    {
        public string word;
        public Sprite image;
    }

    public List<WordData> words;
    public Transform letterParent;
    public GameObject letterTilePrefab;
    public GameObject letterSlotPrefab;
    public GameObject customer;

    private string currentWord;
    private List<LetterTile> currentTiles = new List<LetterTile>();
    private int currentIndex = 0;
    public RoundType currentRoundType { get; private set; }

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

        customer.GetComponent<Customer>().orderImage = w.image;
        customer.GetComponent<Customer>().NewOrder();

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

            lt.Setup(c, this);
            currentTiles.Add(lt);
        }
    }

    public string GetCurrentWord() => currentWord;

    public string GetPlayerAnswer()
    {
        string result = "";
        foreach (Transform child in letterParent)
        {
            // child is the slot; find the tile inside it
            LetterTile tile = child.GetComponentInChildren<LetterTile>();
            if (tile == null) continue;
            result += tile.letterChar;
        }

        Debug.Log("Player Answer: " + result);

        return result;
    }

    public bool CheckAnswer()
    {
        string playerAnswer = GetPlayerAnswer();
        bool correct = playerAnswer == currentWord;
        return correct;
    }

    public void Next()
    {
        currentIndex++;
        currentRoundType = currentRoundType == RoundType.Preparation ? RoundType.Repair : RoundType.Preparation;
        Debug.Log("Next Round Type: " + currentRoundType);
        LoadNextWord();
    }

    private GameObject CreateSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, letterParent);

        RectTransform slotRt = slot.GetComponent<RectTransform>();
        if (slotRt != null)
        {
            slotRt.anchoredPosition = Vector2.zero;
        }
        else
        {
            slot.transform.localPosition = Vector3.zero;
        }
        return slot;
    }

    private GameObject CreateTile(Transform slot)
    {
        GameObject tile = Instantiate(letterTilePrefab, slot);

        RectTransform tileRt = tile.GetComponent<RectTransform>();
        if (tileRt != null)
        {
            tileRt.anchoredPosition = Vector2.zero;
        }
        else
        {
            tile.transform.localPosition = Vector3.zero;
        }
        return tile;
    }
}

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
