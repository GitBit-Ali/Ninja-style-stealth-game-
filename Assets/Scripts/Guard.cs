using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    [SerializeField] private Transform pathHolder;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnSpeed = 90f;

    private int _index = -1;
    private Vector2[] waypoints;

    private void Start ()
    {
        waypoints = new Vector2[pathHolder.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }

        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath ()
    {
        _index = CalculateNextWaypointIndex(_index);
        yield return GoToPoint(waypoints[_index]);

        yield return new WaitForSeconds(waitTime);

        int nextIndex = _index;
        nextIndex = CalculateNextWaypointIndex(nextIndex);
        yield return TurnToFace(waypoints[nextIndex]);

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

    
    private IEnumerator TurnToFace (Vector2 lookTarget)
    {
        Vector2 directionToLookTarget = (lookTarget - (Vector2)transform.position).normalized;
        float angle = (Mathf.Atan2(directionToLookTarget.y, directionToLookTarget.x) * Mathf.Rad2Deg) - 90;
        float currentAngle = transform.eulerAngles.z;
        float angleReachPercent = 0f;

        while (Mathf.Abs(currentAngle - angle) > 0.05f)
        {
            angleReachPercent += Time.deltaTime;
            currentAngle = Mathf.Lerp(currentAngle, angle, angleReachPercent);
            transform.eulerAngles = new(0f, 0f, currentAngle);
            yield return null;
        }
    }
    

    private int CalculateNextWaypointIndex (int current)
    {
        return (current + 1) % waypoints.Length;
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
