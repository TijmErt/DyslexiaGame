using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 6, -16);

    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + new Vector3(0, 10, 30);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target);
    }
}