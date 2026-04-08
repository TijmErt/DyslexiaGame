using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "WordCollection", menuName = "Scriptable Objects/WordCollection")]
public class WordCollection : ScriptableObject
{
    public List<string> words;
    
    public List<string> GetRandomUniqueWords(int count)
    {
        return words
            .OrderBy(x => Random.value)
            .Take(count)
            .ToList();
    }
}

public class NewWord 
{
    public string word;
    public int syllables;
}
