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
            if(CollectibleStateHolder.RuntimeOf(collectible).hasBeenFound)
            {
                FoundCollectibles.Add(collectible);
                GetCrown();
                GameObject go = Instantiate(itemPrefab, contentParent);
                go.GetComponentInChildren<TextMeshProUGUI>().text = collectible.ItemName;
                Debug.Log("HERE IS THE POINT OF COLLECTIBLES BEING ADDED TO THE FOUND LIST");
            }
            Debug.Log(collectible.ItemName);
        }
    }

    private void GetCrown()
    {
        if (FoundCollectibles.Count >= 5)
        {
            CollectibleStateHolder.RuntimeOf(collectibleList.Collectibles[2]).hasBeenFound = true;
            //collectibleList.Collectibles[2].hasBeenFound = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
