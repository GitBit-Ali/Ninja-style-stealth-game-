using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private bool invertZoom;

    private float _zoom;

    private Camera _cam;

    private void Awake ()
    {
        _cam = GetComponent<Camera>();
    }

    private void LateUpdate ()
    {
        float zoomChangeAmount = Input.GetAxisRaw("Mouse ScrollWheel");
        float zoomChange = zoomChangeAmount * zoomSensitivity * Time.deltaTime;
        _zoom += invertZoom ? zoomChange : -zoomChange;
        _zoom = Mathf.Clamp(_zoom, zoomMin, zoomMax);
        _cam.orthographicSize = _zoom;
        transform.position = new(target.position.x, target.position.y, -10);
    }
}
