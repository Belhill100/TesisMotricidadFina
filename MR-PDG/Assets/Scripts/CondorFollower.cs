using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondorFollower : MonoBehaviour
{
    public float speed = 5f;                     // Velocidad del cóndor
    public float disappearDelay = 2f;           // Tiempo antes de que el cóndor desaparezca
    private List<Vector3> pathPoints;           // Puntos del trazo
    private LineRenderer lineRenderer;          // LineRenderer del trazo
    private int currentPointIndex = 0;

    public void SetPath(List<Vector3> points, LineRenderer trailLineRenderer)
    {
        pathPoints = points;
        lineRenderer = trailLineRenderer;
        currentPointIndex = 0;
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while (currentPointIndex < pathPoints.Count)
        {
            // Mueve el cóndor hacia el siguiente punto en el trazo
            Vector3 targetPosition = pathPoints[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }

        // Espera un tiempo antes de desaparecer
        yield return new WaitForSeconds(disappearDelay);

        // Borra el trazo y destruye el cóndor
        ClearLine();
        Destroy(gameObject);
    }

    private void ClearLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // Borra las posiciones del LineRenderer
        }
    }
}



/*using System.Collections;
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
            // Mueve el cóndor hacia el siguiente punto en el trazo
            Vector3 targetPosition = pathPoints[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }
    }
}*/