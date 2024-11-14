using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLockOnRelease : MonoBehaviour
{
    private Rigidbody rb;
    private bool isInContactWithTaggedObject = false; // Indica si el cubo está en contacto con uno de los objetos específicos
    private bool isHeld = true; // Indica si el cubo está siendo sostenido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Método para ser llamado desde el script de interacción (o controlador de manos) cuando el cubo es soltado
    public void OnRelease()
    {
        isHeld = false;

        // Si está en contacto con un objeto con uno de los tags específicos, bloquea el movimiento
        if (isInContactWithTaggedObject)
        {
            LockCube();
        }
    }

    // Detecta la colisión con objetos específicos por tag
    private void OnCollisionEnter(Collision collision)
    {
        if (IsSpecificTag(collision.gameObject.tag))
        {
            isInContactWithTaggedObject = true;

            // Si el cubo ya fue soltado y hace contacto con un objeto de los tags específicos, bloquea el movimiento
            if (!isHeld)
            {
                LockCube();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsSpecificTag(collision.gameObject.tag))
        {
            isInContactWithTaggedObject = false;
        }
    }

    // Método para verificar si el tag es uno de los específicos
    private bool IsSpecificTag(string tag)
    {
        return tag == "Elephant" || tag == "Tiger" || tag == "Dog" || tag == "Cat" || tag == "Titi";
    }

    // Método para bloquear el cubo (desactivar el movimiento y la rotación)
    private void LockCube()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Método para liberar el cubo (por si deseas reutilizar el script y volver a permitir movimiento)
    public void UnlockCube()
    {
        rb.constraints = RigidbodyConstraints.None;
        isHeld = true; // Permitir que el cubo vuelva a ser manipulado
    }
}
