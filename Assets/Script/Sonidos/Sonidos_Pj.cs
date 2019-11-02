using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidos_Pj : MonoBehaviour
{ 
   
    public List<AudioClip> Audios;
    public List<AudioSource> Sources;
    public bool moverse;
    public bool agua;
    public float nivel_agua;

    
    private enum tipo_audio {caminar_grass,dano,saltar_grass,muerte,agua};
    
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
        emitir_mientras((int)tipo_audio.caminar_grass, moverse);
        emitir_mientras((int)tipo_audio.agua, agua);

    }


    public void emitir_grito_dano(bool ena)
    {

        if(ena)
        {
            int i = (int)tipo_audio.dano;

            Sources[i].PlayOneShot(Sources[i].clip);
        }
           
    }

    public void emitir_grito_muerte(bool ena)
    {
        if(ena)
        {
            int i = (int)tipo_audio.muerte;

            Sources[i].PlayOneShot(Sources[i].clip);
        }   
    }

    public void emitir_salto()
    {
        if(!agua)
        {
            int i = (int)tipo_audio.saltar_grass;

            Sources[i].PlayOneShot(Sources[i].clip);
        }   
    }

    public void emitir_mientras(int i, bool condicion)
    {
        if (condicion)
        {
            if (Sources[i].isPlaying)
            {

            }
            else
            {
                Sources[i].Play();
            }
        }
        else
        {
            if (Sources[i].isPlaying)
            {
                Sources[i].Stop();
            }
        }
    }
}
