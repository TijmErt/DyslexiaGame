using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "WordCollection", menuName = "Scriptable Objects/WordCollection")]
public class WordCollection : ScriptableObject
{
    [SerializeField]
    public List<NewWord> words;

    
    public List<string> GetRandomUniqueWords(int count)
    {
        return words
            .OrderBy(x => Random.value)
            .Take(count)
            .ToList();
    }
}

[Serializable]
public class NewWord 
{
    public string word;
    public int syllables;
}
