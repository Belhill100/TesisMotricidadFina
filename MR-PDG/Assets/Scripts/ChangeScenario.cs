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
        //transform.position = transform.position - new Vector3(60f,0f,0f);
        SceneManager.LoadScene("EscenarioJuego1");
    }
    public void changeScenarioGame2()
    {
        //transform.position = transform.position - new Vector3(60f,0f,0f);
        SceneManager.LoadScene("EscenarioJuego2");
    }
    public void changeScenarioGame3()
    {
        //transform.position = transform.position - new Vector3(60f,0f,0f);
        SceneManager.LoadScene("EscenarioJuego3");
    }
    public void changeScenarioGame4()
    {
        //transform.position = transform.position - new Vector3(60f,0f,0f);
        SceneManager.LoadScene("EscenarioJuego4");
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