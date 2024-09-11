using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeStacker : MonoBehaviour
{
    // Definimos el orden correcto
    private string[] correctOrder = { /*"Elephant",*/ "Tiger", "Dog", "Cat", "Titi" };

    // Variable para la puntuaci�n
    public int score = 0;

    // Almacena el �ndice del cubo actual (el pr�ximo cubo a apilar)
    private int currentIndex = 0;

    // Componente de UI para mostrar la puntuaci�n
    public TMPro.TextMeshProUGUI scoreText;

    // M�todo que actualiza la puntuaci�n en pantalla
    void UpdateScore()
    {
        if (scoreText != null) // Verifica que el objeto de texto est� asignado
        {
            scoreText.text = "Puntuaci�n: " + score;
        }
    }

    // M�todo para detectar colisiones entre cubos
    void OnCollisionEnter(Collision col)
    {
        // Si el objeto colisionado no tiene un tag relevante, lo ignoramos
        if (col.gameObject.CompareTag("Untagged"))
        {
            Debug.Log("Colisi�n detectada con un objeto no relevante: " + col.gameObject.name);
            return; // Ignorar colisiones con objetos sin tag relevante
        }

        Debug.Log("Colisi�n detectada con: " + col.gameObject.tag); // Verifica si se detectan colisiones

        // Verifica si el cubo es el siguiente en el orden correcto
        if (col.gameObject.CompareTag(correctOrder[currentIndex]))
        {
            Debug.Log("Apilamiento correcto, �+10 puntos!");
            score += 10;
            UpdateScore();
            currentIndex++; // Avanza al siguiente cubo en el orden
        }
        else
        {
            Debug.Log("Apilamiento incorrecto. Fin del juego.");
            // Aqu� puedes mostrar un mensaje de error o restablecer el juego
        }
    }
}