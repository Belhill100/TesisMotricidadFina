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

    // Offset para ajustar la punta del lápiz
    private Vector3 tipOffset = new Vector3(0, 0, -0.55f);

    // Distancia mínima entre puntos para registrar un nuevo trazo
    public float minDistance = 0.05f;

    // Grosor de la línea
    public float lineThickness = 0.01f; // Ajusta este valor según lo necesites

    // Inicializa el LineRenderer
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Establecer el grosor de la línea
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;

        // Opcional: establecer un material y color para la línea
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black; // Cambia a cualquier color que desees
        lineRenderer.endColor = Color.black; // Cambia a cualquier color que desees
    }

    // Dibuja en cada actualización del frame
    void Update()
    {
        DrawInSpace();
    }

    // Método para dibujar en el espacio 3D
    void DrawInSpace()
    {
        // Obtén la posición ajustada de la punta del lápiz (aplicando el offset)
        Vector3 currentPosition = penTip.position + penTip.TransformDirection(tipOffset);

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
