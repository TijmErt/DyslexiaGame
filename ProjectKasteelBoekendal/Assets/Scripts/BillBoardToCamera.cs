using UnityEngine;

public class BillBoardToCamera : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (_cam == null) return;
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
