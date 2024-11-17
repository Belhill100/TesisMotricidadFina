using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public GameObject Condor;
    public DrawingTool drawingTool; // El script que contiene el trazo
    public Transform spawnPoint;   // El punto de aparici�n del c�ndor

    public void OnButtonPress()
    {
        // Instancia el c�ndor en el mundo
        GameObject condor = Instantiate(Condor, spawnPoint.position, Quaternion.identity);

        // Obt�n los puntos del trazo
        List<Vector3> drawnPath = drawingTool.GetPoints();
        LineRenderer lineRenderer = drawingTool.GetLineRenderer();

        // Haz que el c�ndor siga el trazo y pase el LineRenderer
        condor.GetComponent<CondorFollower>().SetPath(drawnPath, lineRenderer);
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public GameObject Condor;
    public DrawingTool drawingTool; // El script que contiene el trazo
    public Transform spawnPoint; // El punto de aparici�n del c�ndor

    public void OnButtonPress()
    {
        // Instancia el c�ndor en el mundo
        GameObject condor = Instantiate(Condor, spawnPoint.position, Quaternion.identity);

        // Obt�n los puntos del trazo
        List<Vector3> drawnPath = drawingTool.GetPoints();

        // Haz que el c�ndor siga el trazo
        condor.GetComponent<CondorFollower>().SetPath(drawnPath);
    }
}*/