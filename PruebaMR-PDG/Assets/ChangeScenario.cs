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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
