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

    public List<string> GetWordParts()
    {
        List<NewWord> word = GetRandomUniqueWords(1);
        List<string> parts = new List<string>();
        foreach (NewWord part in word) 
        { 
            parts.Add(part.ToString());
        }
        return parts;
    }
}

[System.Serializable]
public class NewWord
{
    public string word;
    public int syllablesCount;
    public List<string> syllablesParts;
}
