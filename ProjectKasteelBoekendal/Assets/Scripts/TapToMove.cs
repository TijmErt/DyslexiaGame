using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TapToMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Camera _mainCamera;

    private Ray _ray;

    private void Awake()
    {
        //_navMeshAgent = GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ProcessTap(touch.position);
            }
        }

        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            ProcessTap(Input.mousePosition);
        }
    }

    private void ProcessTap(Vector2 screenPosition)
    {
        _ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(_ray, out RaycastHit hitInfo))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Move to the item's interaction point instead of the tap point
                _navMeshAgent.SetDestination(interactable.interactionPoint.position);
                StartCoroutine(WaitForArrival(interactable));
            }
            else
            {
                _navMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }

    private IEnumerator WaitForArrival(Interactable interactable)
    {
        // Wait until the NavMeshAgent is close enough
        while (Vector3.Distance(transform.position, _navMeshAgent.destination) > _navMeshAgent.stoppingDistance + 0.1f)
        {
            yield return null;
        }

        // Interact with the object
        interactable.Interact(); // or however you define it
    }
}
