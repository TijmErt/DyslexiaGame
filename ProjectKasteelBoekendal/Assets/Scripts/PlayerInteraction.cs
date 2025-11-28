using UnityEngine;

[RequireComponent(typeof(PlayerHand))]
public class PlayerInteraction : MonoBehaviour
{
    public PlayerHand Hand { get; private set; }

    [SerializeField] private CwW_WordManager _wordManager;
    public CwW_WordManager WordManager => _wordManager;

    private void Awake()
    {
        Hand = GetComponent<PlayerHand>();

        if (_wordManager == null)
        {
            _wordManager = FindFirstObjectByType<CwW_WordManager>();
            if (_wordManager == null)
            {
                Debug.LogWarning("PlayerInteraction: No CwW_WordManager found in scene.");
            }
        }
    }
}
