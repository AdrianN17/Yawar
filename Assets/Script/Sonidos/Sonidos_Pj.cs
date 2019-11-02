using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidos_Pj : MonoBehaviour
{ 
   
    public List<AudioClip> Audios;
    public List<AudioSource> Sources;
    public bool moverse;
    public 
    void Start()
    {
        Audios.AddRange(Resources.LoadAll<AudioClip>("Sonidos") as AudioClip[]);
        Sources = new List<AudioSource>();

        foreach (var sonido in Audios)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audio.clip = sonido;
            Sources.Add(audio);
        }
        
        
    }
    public void Update()
    {
        if(moverse)
        {
            if(Sources[0].isPlaying)
            {

            }
            else
            {
                Sources[0].Play();
            }
        }
        else
        {
            if (Sources[0].isPlaying)
            {
                Sources[0].Stop();
            }
        }
    }


    public void emitir_grito_dano(bool ena)
    {
        if(ena)
        {
            Sources[1].PlayOneShot(Sources[1].clip);
        }
           
    }

    public void emitir_grito_muerte(bool ena)
    {
        if(ena)
        {
            Sources[2].PlayOneShot(Sources[2].clip);
        }   
    }
}
