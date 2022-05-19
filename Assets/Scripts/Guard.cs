using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Guard : MonoBehaviour
{
    [SerializeField] private float timeToSpotPlayer = .5f;
    [SerializeField] private float viewDistance;
    [SerializeField] private Light2D light2D;
    [SerializeField] private LayerMask detectionMask;

    private float _playerVisibleTimer;
    private float _viewAngle;
    private Transform _player;
    private const string PLAYER_TAG = "Player";

    private void Awake ()
    {
        _player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }

    private void Start ()
    {
        _viewAngle = light2D.pointLightInnerAngle;
    }

    private void Update ()
    {
        if (CanSeePlayer())
        {
            light2D.color = Color.red;
        }
        else
        {
            light2D.color = Color.green;
        }
    }

    private bool CanSeePlayer ()
    {
        if (Vector3.Distance(transform.position, _player.position) > viewDistance) return false;

        Vector3 dir = (_player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.up, dir);

        if (angle > _viewAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.position, detectionMask);
        if (!hit.collider.CompareTag(PLAYER_TAG)) return false;

        return true;
    }

    private void OnDrawGizmos ()
    {
        Vector2 position = transform.position + transform.up * viewDistance;
        Gizmos.color = Color.yellow;   
        Gizmos.DrawLine(transform.position, position);
    }
}
