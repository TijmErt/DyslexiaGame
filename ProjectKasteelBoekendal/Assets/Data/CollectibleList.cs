using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleList", menuName = "Scriptable Objects/CollectibleList")]
public class CollectibleList : ScriptableObject
{
    [SerializeField] private List<Collectible> _collectibleList = new List<Collectible>();
    public List<Collectible> Collectibles => _collectibleList;
}
