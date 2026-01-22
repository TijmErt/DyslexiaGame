using System.Collections.Generic;
using UnityEngine;

public class CollectibleStateHolder : MonoBehaviour
{
    [SerializeField] private List<Collectible> defaults; // drag all your assets here

    // Map: default asset -> runtime clone
    private static Dictionary<Collectible, Collectible> runtimeMap;

    public static Collectible RuntimeOf(Collectible defaultAsset)
    {
        if (runtimeMap == null) return null;
        return runtimeMap.TryGetValue(defaultAsset, out var runtime) ? runtime : null;
    }

    private void Awake()
    {
        if (runtimeMap != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        runtimeMap = new Dictionary<Collectible, Collectible>(defaults.Count);

        foreach (var d in defaults)
        {
            if (d == null) continue;
            runtimeMap[d] = Instantiate(d);
        }
    }
}
