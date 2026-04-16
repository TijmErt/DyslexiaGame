using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LetterExchangeTable : MonoBehaviour, IInteractable
{
    [Header("Where this player should stand")]
    [SerializeField] private Transform _playerStandPoint;

    [Header("Shared exchange logic")]
    [SerializeField] private LetterExchangeTableLogic _sharedLogic;

    public Vector3 GetPlayerPosPoint(PlayerInteraction player)
    {
        if (_playerStandPoint != null)
            return _playerStandPoint.position;

        return transform.position;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player == null || player.Hand == null || _sharedLogic == null)
            return;

        _sharedLogic.InteractWith(player.Hand);
    }

    public float InteractionDistance { get; set; }
}
