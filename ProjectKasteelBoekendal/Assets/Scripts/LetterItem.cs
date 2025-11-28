using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LetterItem : MonoBehaviour, IInteractable
{
    [SerializeField] private char _letter = 'A';
    [SerializeField] private Transform _playerStandPoint;

    private Vector3 _homePosition;
    private Quaternion _homeRotation;
    private Collider _collider;

    public char Letter => _letter;

    private void Awake()
    {
        _homePosition = transform.position;
        _homeRotation = transform.rotation;
        _collider = GetComponent<Collider>();
    }

    public void SetLetter(char newLetter)
    {
        _letter = newLetter;
        // TODO: Update visuals here (TextMesh / TMP / mesh, etc.)
    }

    public Vector3 GetPlayerPosPoint()
    {
        return _playerStandPoint != null ? _playerStandPoint.position : transform.position;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player == null) return;
        if (player.Hand == null) return;

        player.Hand.TryPickUp(this);
        Debug.Log(_letter);
    }

    public void OnPickedUp(PlayerHand hand)
    {
        if (_collider != null)
            _collider.enabled = false;

        transform.SetParent(hand.transform);
        transform.localPosition = new Vector3(0f, 1f, 0.5f);
        transform.localRotation = Quaternion.identity;
    }

    public void DropToHome()
    {
        transform.SetParent(null);
        transform.position = _homePosition;
        transform.rotation = _homeRotation;

        if (_collider != null)
            _collider.enabled = true;
    }
}
