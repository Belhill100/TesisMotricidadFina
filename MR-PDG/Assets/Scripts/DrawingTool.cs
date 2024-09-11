using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    // LineRenderer para dibujar la l�nea
    private LineRenderer lineRenderer;

    // Lista para almacenar los puntos del dibujo
    private List<Vector3> pointsList = new List<Vector3>();

    // El l�piz o controlador que dibujar�
    public Transform penTip;

    // Distancia m�nima entre puntos para registrar un nuevo trazo
    public float minDistance = 0.05f;

    // Inicializa el LineRenderer
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Dibuja en cada actualizaci�n del frame
    void Update()
    {
        DrawInSpace();
    }

    // M�todo para dibujar en el espacio 3D
    void DrawInSpace()
    {
        // Obt�n la posici�n actual de la punta del l�piz
        Vector3 currentPosition = penTip.position;

        // Si la distancia entre el �ltimo punto y el actual es mayor que la m�nima, a�ade un nuevo punto
        if (pointsList.Count == 0 || Vector3.Distance(pointsList[pointsList.Count - 1], currentPosition) > minDistance)
        {
            pointsList.Add(currentPosition);
            lineRenderer.positionCount = pointsList.Count;
            lineRenderer.SetPosition(pointsList.Count - 1, currentPosition);
        }
    }

    // M�todo para obtener la lista de puntos del trazo
    public List<Vector3> GetPoints()
    {
        return pointsList;
    }
}

