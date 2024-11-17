using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenario : MonoBehaviour
{
    public void changeScenarioMainMenu()
    {
        SceneManager.LoadScene("EscenarioInicial");
    }
    public void changeScenarioGame1()
    {
        SceneManager.LoadScene("EscenarioJuego1");
    }
    public void changeScenarioGame2()
    {
        SceneManager.LoadScene("EscenarioJuego2");
    }
    public void changeScenarioGame3()
    {
        SceneManager.LoadScene("EscenarioJuego3");
    }
    public void closeGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
