using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Transform cameraTransform; // La c�mara que se sigue
    public Transform leftCanvas; // Canvas izquierdo
    public Transform rightCanvas; // Canvas derecho
    public float distanceFromCamera = 2f; // Distancia fija al frente de la c�mara
    public float horizontalOffset = 1f; // Separaci�n horizontal entre los canvas

    void Update()
    {
        // Posiciona el canvas izquierdo
        Vector3 leftPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera - cameraTransform.right * horizontalOffset;
        leftCanvas.position = leftPosition;

        // Posiciona el canvas derecho
        Vector3 rightPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera + cameraTransform.right * horizontalOffset;
        rightCanvas.position = rightPosition;

        // Ajusta la rotaci�n de ambos canvas para que miren a la c�mara, solo en el eje horizontal
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Elimina la rotaci�n vertical

        leftCanvas.rotation = Quaternion.LookRotation(cameraForward);
        rightCanvas.rotation = Quaternion.LookRotation(cameraForward);
    }
}