using System.Collections.Generic;
using UnityEngine;
public class MemoryWordData
{
    public int id;
    public string word;
    public Sprite image;

    public MemoryWordData(int id, string word, Sprite image)
    {
        this.id = id;
        this.word = word;
        this.image = image;
    }
}