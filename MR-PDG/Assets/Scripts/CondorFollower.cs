using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondorFollower : MonoBehaviour
{
    public float speed = 5f;
    private List<Vector3> pathPoints;
    private int currentPointIndex = 0;

    public void SetPath(List<Vector3> points)
    {
        pathPoints = points;
        currentPointIndex = 0;
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while (currentPointIndex < pathPoints.Count)
        {
            // Mueve el c�ndor hacia el siguiente punto en el trazo
            Vector3 targetPosition = pathPoints[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }
    }
}