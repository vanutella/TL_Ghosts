using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBoxMeshes : MonoBehaviour
{
    public GameObject[] Walls;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Walls.Length; i++)
        {
            Walls[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
