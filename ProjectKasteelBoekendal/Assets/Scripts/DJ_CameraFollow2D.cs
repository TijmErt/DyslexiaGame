using UnityEngine;

public class DJ_CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    public Vector2 yOffsetRange = new Vector2(0f, 0f); // optional

    private Vector3 vel = Vector3.zero;

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        float targetY = target.position.y;

        Vector3 desired = new Vector3(pos.x, targetY, pos.z);
        transform.position = Vector3.SmoothDamp(pos, desired, ref vel, smoothTime);
    }
}
