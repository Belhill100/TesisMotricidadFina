using UnityEngine;

public class CubeMatcher : MonoBehaviour
{
    public string correctTag; // La etiqueta correcta que debe coincidir
    public AudioClip successSound; // Sonido al acertar
    public Transform background; // Referencia al fondo para verificar la rotación
    public float rotationTolerance = 5f; // Margen de error en grados para la comparación de rotaciones

    private bool isMatched = false; // Para bloquear el cubo tras una coincidencia
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si ya está bloqueado
        if (isMatched) return;

        // Verificar si el objeto colisionado tiene la etiqueta correcta
        if (other.CompareTag(correctTag))
        {
            // Verificar si la rotación coincide
            if (IsRotationMatching(other.transform))
            {
                // Reproducir sonido de éxito
                if (successSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(successSound);
                }

                // Bloquear el movimiento del cubo
                isMatched = true;
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Desactiva la física
                }

                // Opcional: Cambiar visualmente el cubo al acertar
                GetComponent<Renderer>().material.color = Color.green;

                Debug.Log("¡Cara y rotación coinciden correctamente!");
            }
            else
            {
                Debug.LogWarning("La rotación no coincide con el fondo.");
            }
        }
    }

    private bool IsRotationMatching(Transform otherTransform)
    {
        // Obtener la rotación del fondo y del objeto en ángulos Euler
        Vector3 backgroundRotation = background.eulerAngles;
        Vector3 objectRotation = otherTransform.eulerAngles;

        // Comparar las rotaciones con una tolerancia
        return Mathf.Abs(Mathf.DeltaAngle(backgroundRotation.x, objectRotation.x)) <= rotationTolerance &&
               Mathf.Abs(Mathf.DeltaAngle(backgroundRotation.y, objectRotation.y)) <= rotationTolerance &&
               Mathf.Abs(Mathf.DeltaAngle(backgroundRotation.z, objectRotation.z)) <= rotationTolerance;
    }
}
