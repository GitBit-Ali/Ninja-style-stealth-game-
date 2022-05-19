using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guard : MonoBehaviour
{
    [SerializeField] private Transform pathHolder;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private bool reversed;

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
        _index = reversed ? CalculatePreviousWaypointIndex(_index) : CalculateNextWaypointIndex(_index);

        yield return GoToPoint(_waypoints[_index]);
        yield return new WaitForSeconds(waitTime);

        int nextIndex = _index;
        nextIndex = CalculateNextWaypointIndex(nextIndex);
        yield return TurnToFace(_waypoints[nextIndex]);

        StartCoroutine(FollowPath());
    }

    private IEnumerator GoToPoint (Vector2 wayPoint)
    {
        while (Vector2.Distance(transform.position, wayPoint) > .1f)
        {
            Vector2 dir = (wayPoint - (Vector2)transform.position).normalized;
            transform.position += speed * Time.deltaTime * (Vector3)dir;
            yield return null;
        }
    }
    
    private IEnumerator TurnToFace (Vector3 lookTarget)
    {
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        float angle = (Mathf.Atan2(directionToLookTarget.y, directionToLookTarget.x) * Mathf.Rad2Deg) - 90;
        float currentAngle = transform.eulerAngles.z;
        float angleReachPercent = 0f;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, angle)) > 0.05f)
        {
            angleReachPercent += Time.deltaTime;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, angle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = new(0f, 0f, currentAngle);
            yield return null;
        }
    }
    

    private int CalculateNextWaypointIndex (int current)
    {
        return (current + 1) % _waypoints.Count;
    }

    private int CalculatePreviousWaypointIndex (int current)
    {
        if (current == 0) return 0;

        return (current - 1) % _waypoints.Count;
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
