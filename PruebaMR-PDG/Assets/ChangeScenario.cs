using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenario : MonoBehaviour
{
    public void changePosition()
    {
        transform.position = transform.position - new Vector3(60f,0f,0f);
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
