using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;

    void Start()
    {
        // Llama a Spawn para generar la cuerda al inicio
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            foreach (GameObject tmp in GameObject.FindGameObjectsWithTag("Rope"))
            {
                Destroy(tmp);
            }

            reset = false;
        }

        if (spawn)
        {
            Spawn();

            spawn = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for (int i = 0; i < count; i++)
        {
            GameObject tmp;

            tmp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (i + 1), transform.position.z), Quaternion.identity, parentObject.transform);

            tmp.transform.eulerAngles = new Vector3(180, 0, 0);

            tmp.name = parentObject.transform.childCount.ToString();

            if(i == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
            }
            else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>(); 
            }
        }
    }
}
