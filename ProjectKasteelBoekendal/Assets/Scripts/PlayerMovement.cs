using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader = default;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    //[SerializeField] private NavMeshSurface _navMeshSurface;
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
            //if (_navMeshSurface.agentTypeID != _navMeshAgent.agentTypeID) return;

            IngredientItem interactable = hitInfo.collider.GetComponent<IngredientItem>();

            if (interactable != null)
            {
                // Move to interaction point
                _navMeshAgent.SetDestination(interactable.GetPlayerPosPoint());
                StartCoroutine(WaitForArrival(interactable));
            }
            else
            {
                // Move directly to the hit point
                _navMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }

    private void ProcessTap2(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            IngredientItem interactable = hitInfo.collider.GetComponent<IngredientItem>();

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

                if (IsPointOnThisAgentsNavMesh(target))
                {
                    _navMeshAgent.SetDestination(target);
                }
                else
                {
                    // Click is outside this agent's navmesh → do nothing
                }
            }
        }
    }

    private bool CheckRaycastHit(Ray ray)
    {
        return Physics.Raycast(ray, out RaycastHit hitInfo);
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

    private IEnumerator WaitForArrival(IngredientItem interactable)
    {
        while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        //maybe new debug.log or interaction
    }

    // --- Touch Input ---
    private void OnTouch(Vector2 screenPosition)
    {
        // Handle a touch as a tap
        ProcessTap2(screenPosition);
    }

    // --- Mouse Input ---
    private void OnMouseMove(Vector2 position)
    {
        currentMousePos = position;
    }

    private void OnMouseTap()
    {
        // Use the latest mouse position when clicked
        ProcessTap2(currentMousePos);
    }
}
