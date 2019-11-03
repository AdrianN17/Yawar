using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arma_melee_atacar : MonoBehaviour
{
    // Start is called before the first frame update
    public List<acciones_compartidas> lista_personajes_golpe;
    public Collider collider_mazo;
    public string layer_receptor;
    public int dano;


    void Start()
    {
        lista_personajes_golpe = new List<acciones_compartidas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(layer_receptor))
        {
            nuevo_receptor(other.gameObject.GetComponent<acciones_compartidas>());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layer_receptor))
        {
            eliminar_receptor(other.gameObject.GetComponent<acciones_compartidas>());
        }
    }

    public void nuevo_receptor(acciones_compartidas obj)
    {
        if (lista_personajes_golpe.Count == 0)
        {
            lista_personajes_golpe.Add(obj);

        }
        else
        {
            foreach (var player in lista_personajes_golpe)
            {
                if (player == obj)
                {
                    return;
                }
            }

            lista_personajes_golpe.Add(obj);
        }
    }

    public void eliminar_receptor(acciones_compartidas obj)
    {
        if (lista_personajes_golpe.Count != 0)
        {
            lista_personajes_golpe.Remove(obj);
        }
    }

    public void golpear_todos()
    { 
        for (var i = lista_personajes_golpe.Count-1; i >= 0; i--)
        {
            try
            {
                var pj = lista_personajes_golpe[i];

                var script = pj.gameObject.GetComponent<acciones_compartidas>();
                
                int vida = script.disminuir_vida(dano);

                if (vida < 1)
                {
                    lista_personajes_golpe.Remove(pj);
                }
                else
                {
                    script.empujon();
                }
            }
            catch(Exception ex)
            {
                lista_personajes_golpe.Remove(lista_personajes_golpe[i]);
            }
                
        }
        
    }

   
}
