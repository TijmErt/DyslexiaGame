using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DJ_WordData", menuName = "Doodle_Jump/WordData")]
public class SO_DJ_WordData : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string correct;
        public List<string> wrongVariants = new List<string>();
        public int tier = 1; // optional difficulty tier
    }

    public List<Entry> entries = new List<Entry>();

    public Entry GetRandomEntry(int minTier, int maxTier)
    {
        List<Entry> pool = entries.FindAll(e => e.tier >= minTier && e.tier <= maxTier);
        if (pool.Count == 0) pool = entries;

        return pool[UnityEngine.Random.Range(0, pool.Count)];
    }
}
