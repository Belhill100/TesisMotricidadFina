using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Importar TextMeshPro

public class CondorGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject condor; // El objeto del cóndor
    public GameObject pencil; // El objeto del lápiz
    public Canvas gameCanvas; // El canvas principal con el botón para jugar

    [Header("Settings")]
    public float countdownTime = 5f; // Tiempo del contador antes de empezar
    public float drawTime = 30f; // Tiempo permitido para dibujar
    public float cleanupDelay = 5f; // Tiempo antes de borrar el trazo tras finalizar
    public float condorSpeed = 5f; // Velocidad del cóndor al seguir el trazo
    public TextMeshPro countdownText; // Texto TextMeshPro para mostrar el contador (3D Object)

    [Header("Custom Rotations")]
    public Quaternion condorRotation = Quaternion.identity; // Rotación personalizada para el cóndor
    public Quaternion pencilRotation = Quaternion.identity; // Rotación personalizada para el lápiz

    [Header("Pencil Settings")]
    public DrawingTool drawingTool; // Script de dibujo para el lápiz

    private bool isGameStarted = false;
    private bool isCondorFollowing = false; // Indica si el cóndor está siguiendo el trazo
    private float currentDrawTime;

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
        // Ocultar el canvas y mostrar el contador
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }
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
                countdownText.text = $"Comienza en: {Mathf.CeilToInt(currentTime)}";
            }
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Iniciar tiempo de dibujo
        currentDrawTime = drawTime;
        PositionAndActivateObjects();

        while (currentDrawTime > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"Tiempo: {Mathf.CeilToInt(currentDrawTime)}";
            }
            currentDrawTime -= Time.deltaTime;
            yield return null;
        }

        // Iniciar el seguimiento del cóndor
        FollowDrawingPath();

        // Esperar hasta que el cóndor termine su recorrido
        yield return new WaitUntil(() => !isCondorFollowing);

        // Borrar el trazo y reiniciar
        yield return new WaitForSeconds(cleanupDelay);
        CleanupAndReset();
    }

    private void PositionAndActivateObjects()
    {
        // Posicionar y activar los objetos frente a la cámara
        Transform cameraTransform = Camera.main.transform;

        // Posicionar el lápiz
        Vector3 pencilPosition = cameraTransform.position + cameraTransform.forward * 1f;
        pencil.transform.position = pencilPosition;
        pencil.transform.rotation = pencilRotation;
        pencil.SetActive(true);

        // Posicionar el cóndor
        Vector3 condorOffset = cameraTransform.right * 0.5f; // Separación lateral
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
            isCondorFollowing = true;
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

        isCondorFollowing = false; // Indicar que el cóndor terminó el recorrido
    }

    private void CleanupAndReset()
    {
        // Ocultar el lápiz y el cóndor
        condor.SetActive(false);
        pencil.SetActive(false);

        // Borrar el trazo
        drawingTool.ClearDrawing();

        // Reiniciar el canvas y el texto
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }
        if (countdownText != null)
        {
            countdownText.text = "";
            countdownText.gameObject.SetActive(false);
        }

        isGameStarted = false;
    }
}
