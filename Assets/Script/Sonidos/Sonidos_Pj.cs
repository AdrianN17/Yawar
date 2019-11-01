using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidos_Pj : MonoBehaviour
{
    AudioSource aSource;
    
    public List<AudioClip> Audios;
    void Start()
    {
        Audios.AddRange(Resources.LoadAll<AudioClip>("Sonidos") as AudioClip[]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            aSource.PlayOneShot(Audios[1]);
        }
    }
}
