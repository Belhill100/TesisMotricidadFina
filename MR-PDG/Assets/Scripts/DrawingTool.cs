using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; // Privado con opción de ajuste desde el inspector
    private List<Vector3> pointsList = new List<Vector3>();
    public Transform penTip;
    private Vector3 tipOffset = new Vector3(0, 0, -0.14f);
    public float minDistance = 0.025f;
    public float lineThickness = 0.05f;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        DrawInSpace();
    }

    private void DrawInSpace()
    {
        Vector3 currentPosition = penTip.position + penTip.TransformDirection(tipOffset);

        if (pointsList.Count == 0 || Vector3.Distance(pointsList[pointsList.Count - 1], currentPosition) > minDistance)
        {
            pointsList.Add(currentPosition);
            lineRenderer.positionCount = pointsList.Count;
            lineRenderer.SetPosition(pointsList.Count - 1, currentPosition);
        }
    }

    public List<Vector3> GetPoints()
    {
        return new List<Vector3>(pointsList);
    }

    public LineRenderer GetLineRenderer()
    {
        return lineRenderer;
    }

    public void ClearDrawing()
    {
        pointsList.Clear();           // Limpia los puntos almacenados
        lineRenderer.positionCount = 0; // Reinicia el LineRenderer
    }
}