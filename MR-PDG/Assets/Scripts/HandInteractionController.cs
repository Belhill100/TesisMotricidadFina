using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandInteractionController : MonoBehaviour
{
    // Vincula esta acción desde el Inspector
    public InputActionProperty pinchAction;

    void Update()
    {
        // Usa el nuevo sistema de Input
        if (pinchAction.action != null)
        {
            if (pinchAction.action.WasPressedThisFrame())
            {
                Debug.Log("Pinch started");
                // Lógica para cuando se inicia el pinch
            }

            if (pinchAction.action.WasReleasedThisFrame())
            {
                Debug.Log("Pinch released");
                // Llama a OnRelease() aquí si necesitas soltar el objeto
            }
        }
    }

    private void OnEnable()
    {
        pinchAction.action?.Enable();
    }

    private void OnDisable()
    {
        pinchAction.action?.Disable();
    }
}
