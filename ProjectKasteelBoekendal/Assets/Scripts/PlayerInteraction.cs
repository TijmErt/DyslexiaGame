using UnityEngine;

[RequireComponent(typeof(PlayerHand))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private int _playerId = 0; // 0 for player 1, 1 for player 2, etc.
    public int PlayerId => _playerId;

    public PlayerHand Hand { get; private set; }

    [SerializeField] private CwW_WordManager _wordManager;
    public CwW_WordManager WordManager => _wordManager;

    private void Awake()
    {
        Hand = GetComponent<PlayerHand>();

        if (_wordManager != null) return;
        _wordManager = FindFirstObjectByType<CwW_WordManager>();

        if (_wordManager != null) return;
        Debug.LogWarning("PlayerInteraction: No CwW_WordManager found in scene.");
    }
}
