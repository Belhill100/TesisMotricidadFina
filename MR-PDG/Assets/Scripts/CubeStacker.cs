using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeStacker : MonoBehaviour
{
    // Definimos el orden correcto
    private string[] correctOrder = { /*"Elephant",*/ "Tiger", "Dog", "Cat", "Titi" };

    // Variable para la puntuación
    public int score = 0;

    // Almacena el índice del cubo actual (el próximo cubo a apilar)
    private int currentIndex = 0;

    // Componente de UI para mostrar la puntuación
    public TMPro.TextMeshProUGUI scoreText;

    // Método que actualiza la puntuación en pantalla
    void UpdateScore()
    {
        if (scoreText != null) // Verifica que el objeto de texto esté asignado
        {
            scoreText.text = "Puntuación: " + score;
        }
    }

    // Método para detectar colisiones entre cubos
    void OnCollisionEnter(Collision col)
    {
        // Si el objeto colisionado no tiene un tag relevante, lo ignoramos
        if (col.gameObject.CompareTag("Untagged"))
        {
            Debug.Log("Colisión detectada con un objeto no relevante: " + col.gameObject.name);
            return; // Ignorar colisiones con objetos sin tag relevante
        }

        Debug.Log("Colisión detectada con: " + col.gameObject.tag); // Verifica si se detectan colisiones

        // Verifica si el cubo es el siguiente en el orden correcto
        if (col.gameObject.CompareTag(correctOrder[currentIndex]))
        {
            Debug.Log("Apilamiento correcto, ¡+10 puntos!");
            score += 10;
            UpdateScore();
            currentIndex++; // Avanza al siguiente cubo en el orden
        }
        else
        {
            Debug.Log("Apilamiento incorrecto. Fin del juego.");
            // Aquí puedes mostrar un mensaje de error o restablecer el juego
        }
    }
}