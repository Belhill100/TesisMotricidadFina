using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLockOnRelease : MonoBehaviour
{
    private Rigidbody rb;
    private bool isInContactWithTaggedObject = false; // Indica si el cubo est� en contacto con uno de los objetos espec�ficos
    private bool isHeld = true; // Indica si el cubo est� siendo sostenido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // M�todo para ser llamado desde el script de interacci�n (o controlador de manos) cuando el cubo es soltado
    public void OnRelease()
    {
        isHeld = false;

        // Si est� en contacto con un objeto con uno de los tags espec�ficos, bloquea el movimiento
        if (isInContactWithTaggedObject)
        {
            LockCube();
        }
    }

    // Detecta la colisi�n con objetos espec�ficos por tag
    private void OnCollisionEnter(Collision collision)
    {
        if (IsSpecificTag(collision.gameObject.tag))
        {
            isInContactWithTaggedObject = true;

            // Si el cubo ya fue soltado y hace contacto con un objeto de los tags espec�ficos, bloquea el movimiento
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

    // M�todo para verificar si el tag es uno de los espec�ficos
    private bool IsSpecificTag(string tag)
    {
        return tag == "Elephant" || tag == "Tiger" || tag == "Dog" || tag == "Cat" || tag == "Titi";
    }

    // M�todo para bloquear el cubo (desactivar el movimiento y la rotaci�n)
    private void LockCube()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // M�todo para liberar el cubo (por si deseas reutilizar el script y volver a permitir movimiento)
    public void UnlockCube()
    {
        rb.constraints = RigidbodyConstraints.None;
        isHeld = true; // Permitir que el cubo vuelva a ser manipulado
    }
}
