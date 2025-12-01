using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class LetterItem : MonoBehaviour, IInteractable
{
    [SerializeField] private char _letter = 'A';
    [SerializeField] private Transform _playerStandPoint;
    [SerializeField] private TMP_Text _letterText;

    private Vector3 _homePosition;
    private Quaternion _homeRotation;
    private Collider _collider;

    public char Letter => _letter;

    private void Awake()
    {
        _homePosition = transform.position;
        _homeRotation = transform.rotation;
        _collider = GetComponent<Collider>();

        UpdateVisual();
    }

    public void SetLetter(char newLetter)
    {
        _letter = newLetter;
        UpdateVisual();
    }

    private void UpdateVisual() 
    {
        if (_letterText != null) 
        {
            _letterText.text = _letter.ToString();
        }
        else
        {
            Debug.LogWarning("LetterItem doesn't have a textbox assigned");
        }
    }

    public void SetHomeToCurrentTransform()
    {
        _homePosition = transform.position;
        _homeRotation = transform.rotation;
    }

    public Vector3 GetPlayerPosPoint(PlayerInteraction player)
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
