using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CondorGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject condor; // El objeto del c�ndor
    public GameObject pencil; // El objeto del l�piz
    public Canvas gameCanvas; // El canvas principal de la escena

    [Header("Settings")]
    public float countdownTime = 3f; // Tiempo del contador
    public float drawTime = 10f; // Tiempo permitido para dibujar
    public float condorSpeed = 5f; // Velocidad del c�ndor al seguir el trazo
    public Text countdownText; // Texto para mostrar el contador (opcional)

    [Header("Custom Rotations")]
    public Quaternion condorRotation = Quaternion.identity; // Rotaci�n personalizada para el c�ndor
    public Quaternion pencilRotation = Quaternion.identity; // Rotaci�n personalizada para el l�piz

    [Header("Pencil Settings")]
    public DrawingTool drawingTool; // Script de dibujo para el l�piz

    private bool isGameStarted = false;

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
        // Ocultar el canvas
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }

        // Mostrar el contador si est� configurado
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

        // Posicionar y activar el l�piz y el c�ndor
        PositionAndActivateObjects();

        // Esperar el tiempo de dibujo
        yield return new WaitForSeconds(drawTime);

        // Iniciar el seguimiento del c�ndor
        FollowDrawingPath();
    }

    private void PositionAndActivateObjects()
    {
        // Obt�n la posici�n y direcci�n de la c�mara
        Transform cameraTransform = Camera.main.transform;

        // Posicionar el l�piz frente a la c�mara con la rotaci�n espec�fica
        Vector3 pencilPosition = cameraTransform.position + cameraTransform.forward * 1f; // 1 metro frente a la c�mara
        pencil.transform.position = pencilPosition;
        pencil.transform.rotation = pencilRotation;
        pencil.SetActive(true);

        // Posicionar el c�ndor a 0.5 unidades a la derecha del l�piz
        Vector3 condorOffset = cameraTransform.right * -0.5f; // Distancia lateral de 0.5 unidades
        condor.transform.position = pencilPosition + condorOffset;
        condor.transform.rotation = condorRotation;
        condor.SetActive(true);
    }

    private void FollowDrawingPath()
    {
        // Obtener los puntos del trazo
        List<Vector3> drawnPath = drawingTool.GetPoints();

        // Asegurarse de que hay un trazo v�lido
        if (drawnPath.Count > 0)
        {
            StartCoroutine(MoveCondorAlongPath(drawnPath));
        }
        else
        {
            Debug.LogWarning("No se dibuj� ning�n trazo.");
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
}
