using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CondorGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject condor; // Objeto del c�ndor
    public GameObject pencil; // Objeto del l�piz
    public Canvas gameCanvas; // Canvas con el bot�n para jugar

    [Header("Settings")]
    public float countdownTime = 2f; // Tiempo del contador antes de empezar
    public float drawTime = 10f; // Tiempo permitido para dibujar
    public float cleanupDelay = 5f; // Tiempo antes de borrar el trazo tras finalizar
    public float condorSpeed = 5f; // Velocidad del c�ndor

    [Header("UI")]
    public TextMeshPro countdownText; // Texto para el contador

    [Header("Condor Animation & Sound")]
    public Animator condorAnimator; // Componente Animator del c�ndor
    public AudioSource condorSound; // Sonido del c�ndor mientras vuela

    [Header("Pencil Settings")]
    public DrawingTool drawingTool; // Script del l�piz para el trazo

    private bool isGameStarted = false;
    private float currentDrawTime;
    private bool condorFlying = false;

    void Start()
    {
        // Desactivar condor y l�piz al inicio
        condor.SetActive(false);
        pencil.SetActive(false);

        // Ocultar contador de tiempo
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // Buscar autom�ticamente el DrawingTool si no est� asignado
        if (drawingTool == null)
        {
            drawingTool = FindObjectOfType<DrawingTool>();
            if (drawingTool == null)
            {
                Debug.LogError("No se encontr� un objeto con el script DrawingTool.");
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
        // Ocultar men� y mostrar contador
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        // Contador antes de empezar
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            countdownText.text = $"Comienza en: {Mathf.CeilToInt(currentTime)}";
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Iniciar tiempo de dibujo
        currentDrawTime = drawTime;
        pencil.SetActive(true);
        while (currentDrawTime > 0)
        {
            countdownText.text = $"Tiempo: {Mathf.CeilToInt(currentDrawTime)}";
            currentDrawTime -= Time.deltaTime;
            yield return null;
        }

        // Iniciar el vuelo del c�ndor
        yield return StartCoroutine(StartCondorFlight());

        // Esperar cleanupDelay antes de reiniciar
        yield return new WaitForSeconds(cleanupDelay);

        // Reiniciar juego
        CleanupAndReset();
    }

    private IEnumerator StartCondorFlight()
    {
        List<Vector3> drawnPath = drawingTool.GetPoints();
        if (drawnPath.Count > 0)
        {
            condor.SetActive(true);
            condorFlying = true;

            // Activar animaci�n
            if (condorAnimator != null)
            {
                condorAnimator.SetBool("isFlying", true);
            }

            // Activar sonido y hacer que se repita
            if (condorSound != null)
            {
                condorSound.loop = true; // Hace que el sonido se repita autom�ticamente
                condorSound.Play();
            }

            // Mover el c�ndor a lo largo del trazo
            yield return StartCoroutine(MoveCondorAlongPath(drawnPath));

            // Cuando termine el trazo, detener animaci�n y sonido
            condorFlying = false;
            if (condorAnimator != null)
            {
                condorAnimator.SetBool("isFlying", false);
            }
            if (condorSound != null)
            {
                condorSound.loop = false; // Desactivar el loop
                condorSound.Stop(); // Detener el sonido
            }

            Debug.Log("El c�ndor complet� el recorrido.");
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
    }

    private void CleanupAndReset()
    {
        // Ocultar el c�ndor y el l�piz
        condor.SetActive(false);
        pencil.SetActive(false);

        // Borrar el trazo
        drawingTool.ClearDrawing();

        // Mostrar el men� de nuevo
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
