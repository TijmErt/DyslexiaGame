using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectible", menuName = "Scriptable Objects/Collectible")]
public class Collectible : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public string ownerName;
    public bool hasBeenFound = false;

}
