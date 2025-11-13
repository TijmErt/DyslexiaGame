using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TileController : MonoBehaviour
{
    [SerializeField] InputReader inputReader = default;
    private Camera _mainCamera;
    private Vector2 currentMousePos;
    private LetterTile selectedTile;
    private LetterSlot destinationSlot;
    private Transform slot;
    private WordManager wordManager;
    private Canvas canvas;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        inputReader.touchEvent += OnTouch;
        inputReader.mousePosEvent += OnMouseMove;
        inputReader.leftMouseButtonEvent += OnMouseTap;
    }

    private void OnDisable()
    {
        inputReader.touchEvent -= OnTouch;
        inputReader.mousePosEvent -= OnMouseMove;
        inputReader.leftMouseButtonEvent -= OnMouseTap;
    }
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnMouseMove(Vector2 position)
    {
        currentMousePos = position;
    }

    private void OnTouch(Vector2 screenPosition)
    {
        ProcessTap(screenPosition);
    }

    private void OnMouseTap()
    {
        ProcessTap(currentMousePos);
    }

    private void ProcessTap(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            LetterTile tile = hitInfo.collider.GetComponent<LetterTile>();
            if (tile != null)
            {
                selectedTile = tile;
                // Additional logic for selecting the tile can be added here
                return;
            }

            LetterSlot slot = hitInfo.collider.GetComponent<LetterSlot>();
            if (slot != null && selectedTile != null)
            {
                destinationSlot = slot;
                // Logic to move the selected tile to the destination slot
                selectedTile.transform.SetParent(destinationSlot.transform, false);
                selectedTile = null; // Deselect after moving
            }
        }
    }

    private Transform FindSlotUnderPointer(PointerEventData eventData)
    {
        foreach (Transform slot in wordManager.letterParent)
        {
            if (slot is not RectTransform slotRt) continue;

            Camera cam = (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null;
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRt, eventData.position, cam))
            {
                return slot;
            }
        }
        return null;
    }

    private void PlaceTileInSlot(Transform targetSlot)
    {
        if (targetSlot != null)
        {
            LetterTile other = targetSlot.GetComponentInChildren<LetterTile>();

            transform.SetParent(targetSlot, false);
            rectTransform.anchoredPosition = Vector2.zero;

            if (other != null && other != this)
            {
                other.transform.SetParent(slot, false);
                RectTransform otherRt = other.GetComponent<RectTransform>();
                if (otherRt != null)
                    otherRt.anchoredPosition = Vector2.zero;

                other.slot = slot;
                slot = targetSlot;
            }
        }
        else
        {
            transform.SetParent(slot, false);
            if (rectTransform != null)
                rectTransform.anchoredPosition = selectedTile.lastAnchoredPos;
        }
    }

}
