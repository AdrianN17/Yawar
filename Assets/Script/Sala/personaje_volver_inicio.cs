using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personaje_volver_inicio : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 inicial;
    void Start()
    {
        inicial = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void volver_al_inicio()
    {
        this.transform.position = inicial;
    }
}
