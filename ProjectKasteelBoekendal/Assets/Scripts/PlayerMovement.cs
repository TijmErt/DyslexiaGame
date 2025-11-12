using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader = default;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Camera _mainCamera;

    private Vector2 currentMousePos;

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

    // --- Core Tap Processing ---
    private void ProcessTap(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Move to interaction point
                _navMeshAgent.SetDestination(interactable.interactionPoint.position);
                StartCoroutine(WaitForArrival(interactable));
            }
            else
            {
                // Move directly to the hit point
                _navMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }

    private IEnumerator WaitForArrival(Interactable interactable)
    {
        while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        interactable.Interact();
    }

    // --- Touch Input ---
    private void OnTouch(Vector2 screenPosition)
    {
        // Handle a touch as a tap
        ProcessTap(screenPosition);
    }

    // --- Mouse Input ---
    private void OnMouseMove(Vector2 position)
    {
        currentMousePos = position;
    }

    private void OnMouseTap()
    {
        // Use the latest mouse position when clicked
        ProcessTap(currentMousePos);
    }
}
