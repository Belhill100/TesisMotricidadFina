using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CondorGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject condor; // El objeto del cóndor
    public GameObject pencil; // El objeto del lápiz
    public Canvas gameCanvas; // El canvas principal de la escena

    [Header("Settings")]
    public float countdownTime = 3f; // Tiempo del contador
    public float drawTime = 10f; // Tiempo permitido para dibujar
    public float condorSpeed = 5f; // Velocidad del cóndor al seguir el trazo
    public Text countdownText; // Texto para mostrar el contador (opcional)

    [Header("Custom Rotations")]
    public Quaternion condorRotation = Quaternion.identity; // Rotación personalizada para el cóndor
    public Quaternion pencilRotation = Quaternion.identity; // Rotación personalizada para el lápiz

    [Header("Pencil Settings")]
    public DrawingTool drawingTool; // Script de dibujo para el lápiz

    private bool isGameStarted = false;

    void Start()
    {
        // Desactivar el lápiz y el cóndor al inicio
        condor.SetActive(false);
        pencil.SetActive(false);

        // Asegurarse de que el texto del contador esté desactivado
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // Buscar automáticamente el objeto que contiene el DrawingTool si no está asignado
        if (drawingTool == null)
        {
            drawingTool = FindObjectOfType<DrawingTool>();
            if (drawingTool == null)
            {
                Debug.LogError("No se encontró un objeto con el script DrawingTool en la escena.");
            }
        }
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            StartCoroutine(GameSequence());
        }
    }

    private IEnumerator GameSequence()
    {
        // Ocultar el canvas
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }

        // Mostrar el contador si está configurado
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        // Contador regresivo antes de iniciar
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(currentTime).ToString();
            }
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Ocultar el texto del contador
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // Posicionar y activar el lápiz y el cóndor
        PositionAndActivateObjects();

        // Esperar el tiempo de dibujo
        yield return new WaitForSeconds(drawTime);

        // Iniciar el seguimiento del cóndor
        FollowDrawingPath();
    }

    private void PositionAndActivateObjects()
    {
        // Obtén la posición y dirección de la cámara
        Transform cameraTransform = Camera.main.transform;

        // Posicionar el lápiz frente a la cámara con la rotación específica
        Vector3 pencilPosition = cameraTransform.position + cameraTransform.forward * 1f; // 1 metro frente a la cámara
        pencil.transform.position = pencilPosition;
        pencil.transform.rotation = pencilRotation;
        pencil.SetActive(true);

        // Posicionar el cóndor a 0.5 unidades a la derecha del lápiz
        Vector3 condorOffset = cameraTransform.right * -0.5f; // Distancia lateral de 0.5 unidades
        condor.transform.position = pencilPosition + condorOffset;
        condor.transform.rotation = condorRotation;
        condor.SetActive(true);
    }

    private void FollowDrawingPath()
    {
        // Obtener los puntos del trazo
        List<Vector3> drawnPath = drawingTool.GetPoints();

        // Asegurarse de que hay un trazo válido
        if (drawnPath.Count > 0)
        {
            StartCoroutine(MoveCondorAlongPath(drawnPath));
        }
        else
        {
            Debug.LogWarning("No se dibujó ningún trazo.");
        }
    }

    private IEnumerator MoveCondorAlongPath(List<Vector3> pathPoints)
    {
        int currentPointIndex = 0;

        while (currentPointIndex < pathPoints.Count)
        {
            Vector3 targetPosition = pathPoints[currentPointIndex];
            while (Vector3.Distance(condor.transform.position, targetPosition) > 0.1f)
            {
                condor.transform.position = Vector3.MoveTowards(condor.transform.position, targetPosition, condorSpeed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }

        Debug.Log("El cóndor completó el recorrido.");
    }
}
