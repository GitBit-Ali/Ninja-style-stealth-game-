using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform pathHolder;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private Rigidbody2D rb;

    private int _index = -1;
    private List<Vector2> _waypoints;

    private void Start ()
    {
        _waypoints = new List<Vector2>();

        for (int i = 0; i < pathHolder.childCount; i++)
        {
            _waypoints.Add(pathHolder.GetChild(i).position);
        }

        transform.position = _waypoints[0];

        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath ()
    {
        _index = CalculateNextWaypointIndex(_index);

        yield return GoToPoint(_waypoints[_index]);
        yield return new WaitForSeconds(waitTime);

        int i = _index;
        i = CalculateNextWaypointIndex(i);
        StartCoroutine(TurnToFaceIEnumerator(_waypoints[i]));

        StartCoroutine(FollowPath());
    }

    private IEnumerator GoToPoint (Vector2 wayPoint)
    {
        while (Vector2.Distance(transform.position, wayPoint) > .1f)
        {
            Vector2 dir = (wayPoint - (Vector2)transform.position).normalized;
            rb.position += speed * Time.deltaTime * dir;
            yield return null;
        }
    }

    private void TurnToFace (Vector3 lookTarget)
    {
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = (Mathf.Atan2(directionToLookTarget.y, directionToLookTarget.x) * Mathf.Rad2Deg) - 90;
        transform.eulerAngles = new(0, 0, targetAngle);
    }

    private IEnumerator TurnToFaceIEnumerator (Vector3 lookTarget)
    {
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = (Mathf.Atan2(directionToLookTarget.y, directionToLookTarget.x) * Mathf.Rad2Deg) - 90;        
        float angle = transform.eulerAngles.z;

        while (transform.eulerAngles.z != targetAngle)
        {
            angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = new(0, 0, angle);
            yield return null;
        }
    }

    private int CalculateNextWaypointIndex (int current)
    {
        return (current + 1) % _waypoints.Count;
    }

    private void OnDrawGizmos ()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
