using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    // LineRenderer para dibujar la línea
    private LineRenderer lineRenderer;

    // Lista para almacenar los puntos del dibujo
    private List<Vector3> pointsList = new List<Vector3>();

    // El lápiz o controlador que dibujará
    public Transform penTip;

    // Distancia mínima entre puntos para registrar un nuevo trazo
    public float minDistance = 0.05f;

    // Inicializa el LineRenderer
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Dibuja en cada actualización del frame
    void Update()
    {
        DrawInSpace();
    }

    // Método para dibujar en el espacio 3D
    void DrawInSpace()
    {
        // Obtén la posición actual de la punta del lápiz
        Vector3 currentPosition = penTip.position;

        // Si la distancia entre el último punto y el actual es mayor que la mínima, añade un nuevo punto
        if (pointsList.Count == 0 || Vector3.Distance(pointsList[pointsList.Count - 1], currentPosition) > minDistance)
        {
            pointsList.Add(currentPosition);
            lineRenderer.positionCount = pointsList.Count;
            lineRenderer.SetPosition(pointsList.Count - 1, currentPosition);
        }
    }

    // Método para obtener la lista de puntos del trazo
    public List<Vector3> GetPoints()
    {
        return pointsList;
    }
}

