using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CondorGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject condor; // El objeto del c�ndor
    public GameObject pencil; // El objeto del l�piz
    public Canvas gameCanvas; // El canvas principal con el bot�n para jugar

    [Header("Settings")]
    public float countdownTime = 2f; // Tiempo del contador antes de empezar
    public float drawTime = 10f; // Tiempo permitido para dibujar
    public float cleanupDelay = 5f; // Tiempo antes de borrar el trazo tras finalizar
    public float condorSpeed = 5f; // Velocidad del c�ndor al seguir el trazo
    public TMP_Text countdownText; // Texto para mostrar el contador

    [Header("Custom Rotations")]
    public Quaternion pencilRotation = Quaternion.identity; // Rotaci�n personalizada para el l�piz

    [Header("Pencil Settings")]
    public DrawingTool drawingTool; // Script de dibujo para el l�piz

    private bool isGameStarted = false;
    private bool isCondorFlying = false; // Estado del vuelo del c�ndor
    private float currentDrawTime;

    void Start()
    {
        // Desactivar el l�piz y el c�ndor al inicio
        condor.SetActive(false);
        pencil.SetActive(false);

        // Asegurarse de que el texto del contador est� desactivado
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // Buscar autom�ticamente el objeto que contiene el DrawingTool si no est� asignado
        if (drawingTool == null)
        {
            drawingTool = FindObjectOfType<DrawingTool>();
            if (drawingTool == null)
            {
                Debug.LogError("No se encontr� un objeto con el script DrawingTool en la escena.");
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
                countdownText.text = $"Tiempo: {Mathf.CeilToInt(currentTime)}";
            }
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Iniciar tiempo de dibujo
        currentDrawTime = drawTime;
        PositionAndActivatePencil();

        while (currentDrawTime > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"Tiempo: {Mathf.CeilToInt(currentDrawTime)}";
            }
            currentDrawTime -= Time.deltaTime;
            yield return null;
        }

        // Mostrar el c�ndor y comenzar su vuelo
        condor.SetActive(true);
        yield return FollowDrawingPath();

        // Esperar a que el c�ndor termine el recorrido y borrar el trazo
        yield return new WaitForSeconds(cleanupDelay);
        CleanupAndReset();
    }

    private void PositionAndActivatePencil()
    {
        // Posicionar y activar el l�piz frente a la c�mara
        Transform cameraTransform = Camera.main.transform;

        Vector3 pencilPosition = cameraTransform.position + cameraTransform.forward * 1f;
        pencil.transform.position = pencilPosition;
        pencil.transform.rotation = pencilRotation;
        pencil.SetActive(true);
    }

    private IEnumerator FollowDrawingPath()
    {
        // Obtener los puntos del trazo
        List<Vector3> drawnPath = drawingTool.GetPoints();

        // Asegurarse de que hay un trazo v�lido
        if (drawnPath.Count > 0)
        {
            isCondorFlying = true; // Indicar que el c�ndor est� volando
            yield return MoveCondorAlongPath(drawnPath);
            isCondorFlying = false; // El c�ndor termin� de volar
        }
        else
        {
            Debug.LogWarning("No se dibuj� ning�n trazo.");
            condor.SetActive(false); // Ocultar el c�ndor si no hay trazo
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

        Debug.Log("El c�ndor complet� el recorrido.");
    }

    private void CleanupAndReset()
    {
        // Verificar si el c�ndor todav�a est� volando
        if (isCondorFlying)
        {
            Debug.LogWarning("El c�ndor sigue volando. Esperando a que termine...");
            return; // No limpiar hasta que el c�ndor termine
        }

        // Ocultar el l�piz y el c�ndor
        condor.SetActive(false);
        pencil.SetActive(false);

        // Borrar el trazo
        drawingTool.ClearDrawing();

        // Reiniciar el canvas y el contador
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        isGameStarted = false;
    }
}
