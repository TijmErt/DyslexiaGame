using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader = default;
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector2 movementInput;
    private Vector3 finalMoveDirection;

    private void OnEnable()
    {
        inputReader.moveEvent += OnMove;
    }

    private void OnDisable()
    {
        inputReader.moveEvent -= OnMove;
        OnMove(Vector2.zero);
    }

    private void Awake()
    {
        CheckRigidbody();

        // IMPORTANT: ensure physics behaves correctly
        rb.freezeRotation = true;

        // If you still have NavMeshAgent on the object, fully disable it
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;
    }

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(
            movementInput.x,
            0f,
            movementInput.y
        );

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Camera cam = Camera.main;

            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            finalMoveDirection =
                cameraForward * moveDirection.z +
                cameraRight * moveDirection.x;

            rb.MovePosition(rb.position +
                finalMoveDirection * movementSpeed * Time.fixedDeltaTime);
        }
        else
        {
            finalMoveDirection = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (finalMoveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(finalMoveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void OnMove(Vector2 input)
    {
        movementInput = input;
    }
    

    private void CheckRigidbody()
    {
        if (rb != null) return;

        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("PlayerMovement: No Rigidbody found on Player.");
    }
}