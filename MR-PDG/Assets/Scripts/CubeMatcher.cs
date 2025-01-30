using UnityEngine;

public class CubeMatcher : MonoBehaviour
{
    public GameObject cube; // Referencia al cubo
    public Transform background; // Referencia al fondo
    public float rotationTolerance = 15f; // Tolerancia para comparar rotaciones en Z
    public Vector3 positionTolerance = new Vector3(0.1f, 0.1f, 0.1f); // Tolerancia para comparar posiciones
    public AudioSource successSound; // Sonido al colocar correctamente un cubo
    public static int frozenCubes = 0; // Contador de cubos congelados
    public static int totalCubes = 3; // Total de cubos en la escena
    public AudioSource winSound; // Sonido al completar todos los cubos
    private Rigidbody cubeRb; // Referencia al Rigidbody del cubo
    private bool isFrozen = false; // Para evitar que el cubo se congele m�s de una vez

    private void Start()
    {
        cubeRb = cube.GetComponent<Rigidbody>();
        if (cubeRb == null)
        {
            Debug.LogError("El cubo necesita un Rigidbody para poder congelarse.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == background.gameObject && !isFrozen)
        {
            Debug.Log("El cubo ha colisionado con el fondo.");
            CheckFaceAndRotation();
        }
    }

    private void CheckFaceAndRotation()
    {
        Vector3 forwardDirection = -Vector3.forward; // Referencia para la cara frontal (Z negativo)
        float normalTolerance = 0.1f; // Tolerancia para la comparaci�n de normales
        Transform cubeTransform = cube.transform;

        // Obtener las normales de las caras del cubo
        Vector3[] faceNormals =
        {
            cubeTransform.TransformDirection(Vector3.forward),
            cubeTransform.TransformDirection(Vector3.back),
            cubeTransform.TransformDirection(Vector3.up),
            cubeTransform.TransformDirection(Vector3.down),
            cubeTransform.TransformDirection(Vector3.left),
            cubeTransform.TransformDirection(Vector3.right)
        };

        // Verificar si alguna cara est� alineada con el eje Z negativo
        bool faceMatched = false;
        for (int i = 0; i < faceNormals.Length; i++)
        {
            if (Mathf.Abs(Vector3.Dot(faceNormals[i], forwardDirection) - 1f) < normalTolerance)
            {
                Debug.Log($"La cara {i} del cubo est� alineada con el eje Z negativo.");
                faceMatched = true;
                break;
            }
        }

        if (!faceMatched)
        {
            Debug.Log("Ninguna cara del cubo est� alineada con el eje Z negativo.");
            return;
        }

        bool positionMatched = IsCubeInsideBackground();
        Debug.Log($"Posici�n del cubo dentro del fondo: {positionMatched}");

        // Comparar solo la rotaci�n en Z usando valores absolutos
        float cubeRotationZ = Mathf.Abs(cube.transform.rotation.eulerAngles.z);
        float backgroundRotationZ = Mathf.Abs(background.rotation.eulerAngles.z);
        float rotationDifference = Mathf.Abs(cubeRotationZ - backgroundRotationZ);
        bool rotationMatched = rotationDifference < rotationTolerance;

        Debug.Log($"Rotaci�n Z - Cubo: {cubeRotationZ}, Fondo: {backgroundRotationZ}");
        Debug.Log($"Diferencia de rotaci�n Z: {rotationDifference} | Coincidencia: {rotationMatched}");

        if (positionMatched && rotationMatched)
        {
            Debug.Log("El cubo est� correctamente alineado con el fondo. Se congelar�.");
            FreezeCube();
        }
        else
        {
            Debug.Log("El cubo no est� correctamente alineado con el fondo.");
        }
    }

    private bool IsCubeInsideBackground()
    {
        Bounds backgroundBounds = background.GetComponent<Collider>().bounds;
        Vector3 cubePosition = cube.transform.position;

        return (cubePosition.x > backgroundBounds.min.x - positionTolerance.x &&
                cubePosition.x < backgroundBounds.max.x + positionTolerance.x &&
                cubePosition.y > backgroundBounds.min.y - positionTolerance.y &&
                cubePosition.y < backgroundBounds.max.y + positionTolerance.y &&
                cubePosition.z > backgroundBounds.min.z - positionTolerance.z &&
                cubePosition.z < backgroundBounds.max.z + positionTolerance.z);
    }

    private void FreezeCube()
    {
        if (cubeRb != null)
        {
            cubeRb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("El cubo ha sido congelado y ya no puede moverse.");
            isFrozen = true;

            // Reproducir sonido de �xito
            if (successSound != null)
            {
                successSound.Play();
            }

            // Contar cubos congelados
            frozenCubes++;

            // Verificar si todos los cubos han sido colocados correctamente
            if (frozenCubes >= totalCubes)
            {
                Debug.Log("�Todos los cubos est�n alineados! Se ha ganado el juego.");
                if (winSound != null)
                {
                    winSound.Play();
                }
            }
        }
    }
}
