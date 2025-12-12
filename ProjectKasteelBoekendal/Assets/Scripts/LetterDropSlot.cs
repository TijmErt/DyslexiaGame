using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class LetterDropSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _standPoint;

    public Vector3 GetPlayerPosPoint(PlayerInteraction player)
    {
        if (_standPoint != null) return _standPoint.position;
        return transform.position;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player == null || player.Hand == null) return;
        if (!player.Hand.HasItem) return;

        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<LetterItem>() != null)
                return;
        }

        LetterItem letter = player.Hand.RemoveHeldWithoutDropping();
        if (letter == null) return;

        MoveItem(letter);

        letter.SetHomeToCurrentTransform();

        var col = letter.GetComponent<Collider>();
        if (col != null) col.enabled = true;
    }

    private void MoveItem(LetterItem letter)
    {
        letter.transform.SetParent(transform);
        letter.transform.localPosition = Vector3.zero;
        letter.transform.localRotation = Quaternion.identity;
    }
}
