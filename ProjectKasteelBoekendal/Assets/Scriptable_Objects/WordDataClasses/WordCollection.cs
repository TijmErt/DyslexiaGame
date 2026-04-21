using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "WordCollection", menuName = "Scriptable Objects/WordCollection")]
public class WordCollection : ScriptableObject
{
    public List<NewWord> words;

    public List<NewWord> GetRandomUniqueWords(int count)
    {
        return words
            .OrderBy(x => UnityEngine.Random.value)
            .Take(count)
            .ToList();
    }

    public List<string> GetRandomWordPartsBySyllableCount(int syllablesCount)
    {
        List<NewWord> matchingWords = words
            .Where(w => w.syllablesCount == syllablesCount)
            .ToList();

        if (matchingWords.Count == 0)
            return new List<string>();

        NewWord selected = matchingWords[Random.Range(0, matchingWords.Count)];

        return selected.syllablesParts;
    }

    public List<MemoryWordData> GetMemoryData(int count)
    {
        List<NewWord> selectedWords = GetRandomUniqueWords(count);
        List<MemoryWordData> wordData = new List<MemoryWordData>();

        int counter = 0;

        foreach (NewWord entry in selectedWords)
        {
            wordData.Add(new MemoryWordData(counter, entry.word, entry.image));
            counter++;
        }

        return wordData;
    }  
}
