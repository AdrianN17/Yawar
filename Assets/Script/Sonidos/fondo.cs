using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fondo : MonoBehaviour
{
    AudioSource aSource;
    public AudioClip SonidoFondo;
    void Start()
    {
        aSource.PlayOneShot(SonidoFondo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
