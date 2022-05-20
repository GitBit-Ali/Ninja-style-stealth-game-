using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class Guard : MonoBehaviour
{
    public static event Action OnPlayerSpotted;

    [SerializeField] private float timeToSpotPlayer = .5f;
    [SerializeField] private float viewDistance;
    [SerializeField] private Light2D light2D;
    [SerializeField] private Color lightNormalColor = Color.green;
    [SerializeField] private Color lightPlayerSpottedColor = Color.red;
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
        OnPlayerSpotted += () =>
        {
            GetComponent<WaypointMovement>().SetSpeed(0);
        };

        GetComponent<WaypointMovement>().SetSpeed(3);
    }

    private void Update ()
    {
        if (CanSeePlayer())
        {
            _playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            _playerVisibleTimer -= Time.deltaTime;
        }
        _playerVisibleTimer = Mathf.Clamp(_playerVisibleTimer, 0, timeToSpotPlayer);
        light2D.color = Color.Lerp(lightNormalColor, lightPlayerSpottedColor, _playerVisibleTimer / timeToSpotPlayer);

        if (_playerVisibleTimer >= timeToSpotPlayer)
        {
            OnPlayerSpotted?.Invoke();
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
