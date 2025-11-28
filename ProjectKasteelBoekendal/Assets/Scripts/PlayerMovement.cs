using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader = default;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerInteraction _playerInteraction;

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

        if (_playerInteraction == null)
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
            if (_playerInteraction == null)
            {
                Debug.LogError("PlayerMovement: No PlayerInteraction reference found.");
            }
        }

        if (_navMeshAgent == null)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                Debug.LogError("PlayerMovement: No NavMeshAgent reference found.");
            }
        }
    }

    // --- Core Tap Processing ---
    private void ProcessTap(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Vector3 target = interactable.GetPlayerPosPoint();

                if (!IsPointOnThisAgentsNavMesh(target)) return;

                _navMeshAgent.SetDestination(target);
                StartCoroutine(WaitForArrival(interactable));
            }
            else
            {
                Vector3 target = hitInfo.point;
                if (!IsPointOnThisAgentsNavMesh(target)) return;

                _navMeshAgent.SetDestination(target);
            }
        }
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

    private IEnumerator WaitForArrival(IInteractable interactable)
    {
        if (_navMeshAgent == null)
            yield break;

        while (_navMeshAgent.pathPending ||
               _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        if (_playerInteraction != null && interactable != null)
        {
            interactable.Interact(_playerInteraction);
        }
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
