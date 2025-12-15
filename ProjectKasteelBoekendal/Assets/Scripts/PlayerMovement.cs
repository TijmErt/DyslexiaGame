using System.Collections;
using Unity.AI.Navigation;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader = default;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerInteraction _playerInteraction;

    private Camera _mainCamera;
    private Vector2 currentMousePos;

    private IInteractable _currentTargetInteractable;
    private int _currentInteractionId = 0;

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

        CheckPlayerInteraction();
        CheckNavmeshAgent();
    }

    private void CheckPlayerInteraction()
    {
        if (_playerInteraction != null) return;
        _playerInteraction = GetComponent<PlayerInteraction>();

        if (_playerInteraction != null) return;
        Debug.LogError("PlayerMovement: No PlayerInteraction reference found.");
    }
    private void CheckNavmeshAgent()
    {
        if (_navMeshAgent != null) return;
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent != null) return;
        Debug.LogError("PlayerMovement: No NavMeshAgent reference found.");
    }

    // --- Core Tap Processing ---
    private void ProcessTap(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log($"{name} (PlayerId {_playerInteraction.PlayerId}) hit {hitInfo.collider.name} on layer {LayerMask.LayerToName(hitInfo.collider.gameObject.layer)}");

            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
                GoToInteractionAndInteract(interactable);
            else
                GoToLocation(hitInfo);
        }
    }

    private void GoToInteractionAndInteract(IInteractable interactable)
    {
        Vector3 target = interactable.GetPlayerPosPoint(_playerInteraction);
        if (!IsPointOnThisAgentsNavMesh(target)) return;

        _navMeshAgent.SetDestination(target);

        ChangeTarget(interactable);
    }
    private void GoToLocation(RaycastHit hitInfo)
    {
        Vector3 target = hitInfo.point;
        if (!IsPointOnThisAgentsNavMesh(target)) return;

        _navMeshAgent.SetDestination(target);

        ChangeTarget();
    }
    private void ChangeTarget(IInteractable interactable = null)
    {
        _currentInteractionId++;
        _currentTargetInteractable = interactable;
        if (interactable != null) StartCoroutine(WaitForArrival(interactable, _currentInteractionId));
    }


    private bool IsPointOnThisAgentsNavMesh(Vector3 point, float maxDistance = 0.2f)
    {
        NavMeshHit hit;

        var filter = new NavMeshQueryFilter
        {
            agentTypeID = _navMeshAgent.agentTypeID,
            areaMask = NavMesh.AllAreas
        };

        if (!NavMesh.SamplePosition(point, out hit, maxDistance, filter))
            return false;
        if (Vector3.Distance(hit.position, point) > maxDistance)
            return false;

        return true;
    }

    private IEnumerator WaitForArrival(IInteractable interactable, int interactionId)
    {
        if (_navMeshAgent == null) yield break;

        while (_navMeshAgent.pathPending ||
               _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        if (interactionId != _currentInteractionId) yield break;

        if (_currentTargetInteractable != interactable) yield break;

        if (_playerInteraction != null && interactable != null)
            interactable.Interact(_playerInteraction);
    }


    // --- Touch Input ---
    private void OnTouch(Vector2 screenPosition)
    {
        ProcessTap(screenPosition);
    }

    // --- Mouse Input ---
    private void OnMouseMove(Vector2 position)
    {
        currentMousePos = position;
    }

    private void OnMouseTap()
    {
        ProcessTap(currentMousePos);
    }
}
