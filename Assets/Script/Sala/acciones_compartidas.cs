using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class acciones_compartidas : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;
    public Collider collider_arma;
    public arma_melee_atacar ama;

    public TextMesh texto_vida;

    public enum tipo {personaje_principal, personaje, enemigo};
    public tipo mitipo;

    public int vidas;


    // Start is called before the first frame update
    void Start()
    {
        if(mitipo!=tipo.personaje_principal)
        {
            texto_vida = GetComponentInChildren<TextMesh>();
            texto_vida.text = vidas.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void empujon(Vector3 point)
    {
        Vector3 dir = collider.bounds.center - point;
        dir.Normalize();

        //rb.AddForce(dir * 2500 * rb.mass);
        rb.AddForce(dir * 900 * rb.mass);

    }

    public void ataque()
    {
        ama.golpear_todos();
    }

    public int disminuir_vida(int dano)
    {
        vidas = vidas - dano;

        if (mitipo != tipo.personaje_principal)
        {
            texto_vida.text = vidas.ToString();
        }

        if (vidas<1)
        {
            morir();
        }

        return vidas;
    }

    public void morir()
    {
        if (mitipo == tipo.enemigo)
        {
            Invoke("destruir_gameobject", 1);
            
        }
    }

    public void destruir_gameobject()
    {
        Destroy(this.gameObject);
    }

    public void personaje_principal()
    {
        mitipo = tipo.personaje_principal;
    }
}
