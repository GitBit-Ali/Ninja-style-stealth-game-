using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private string targetTag;

    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private bool invertZoom;

    private float _zoom;
    private Camera _cam;
    private Transform _target;

    private void Awake ()
    {
        _cam = GetComponent<Camera>();
        _target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    private void LateUpdate ()
    {
        float zoomChangeAmount = Input.GetAxisRaw("Mouse ScrollWheel");
        float zoomChange = zoomChangeAmount * zoomSensitivity * Time.deltaTime;
        _zoom += invertZoom ? zoomChange : -zoomChange;
        _zoom = Mathf.Clamp(_zoom, zoomMin, zoomMax);
        _cam.orthographicSize = _zoom;
        transform.position = new(_target.position.x, _target.position.y, -10);
    }

    private void OnDestroy ()
    {
        _target = null;
    }
}
