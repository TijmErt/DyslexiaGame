using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerInventoryControl : MonoBehaviour
{
    [SerializeField]
    private List<Collectible> FoundCollectibles;
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private CollectibleList collectibleList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FoundCollectibles = new List<Collectible>();

       foreach (Collectible collectible in collectibleList.Collectibles)
       {
         if(collectible.hasBeenFound)
         {
                FoundCollectibles.Add(collectible);
                GameObject go = Instantiate(itemPrefab, contentParent);
               go.GetComponentInChildren<TextMeshProUGUI>().text = collectible.ItemName;
         }
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
