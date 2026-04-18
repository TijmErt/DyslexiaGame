using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3( target.position.x, cam.transform.position.y, target.position.z);
    }
}
