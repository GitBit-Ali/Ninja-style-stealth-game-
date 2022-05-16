using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    [SerializeField] private Transform pathHolder;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnSpeed = 90f;

    private void Start ()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }

        StartCoroutine(FollowPath(waypoints));
    }

    private IEnumerator FollowPath (Vector3[] wayPoints)
    {
        transform.position = wayPoints[0];
        transform.eulerAngles = new Vector3(0f, 0f, -90f);

        int targetWaypointIndex = 1;
        Vector3 targetPosition = wayPoints[targetWaypointIndex];

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPosition) < .1f)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % wayPoints.Length;
                targetPosition = wayPoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetPosition));
            }

            yield return null;
        }
    }

    private IEnumerator TurnToFace (Vector2 lookTarget)
    {
        Vector2 dirToLookTarget = (lookTarget - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirToLookTarget.y, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.forward * angle;
            yield return null;
        }
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
