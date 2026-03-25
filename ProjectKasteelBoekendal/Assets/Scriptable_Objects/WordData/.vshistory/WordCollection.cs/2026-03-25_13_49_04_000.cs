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
}

[System.Serializable]
public class NewWord
{
    public string word;
    public int syllables;
}
