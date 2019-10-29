using Assets.Script.Modelos;
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

    public GameObject prefab_barra;
    public barra_vida barra;

    public enum tipo {personaje_principal, personaje, enemigo};
    public tipo mitipo;

    public int max_vidas;
    public int vidas;


    // Start is called before the first frame update
    void Start()
    {
        vidas = max_vidas;

        if (mitipo!=tipo.personaje_principal)
        {
            barra = prefab_barra.GetComponent<barra_vida>();
        }
        else
        {
            prefab_barra.SetActive(false);
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
            barra.reduce(max_vidas, vidas);
        }

        if (vidas<1)
        {
            morir();
        }

        return vidas;
    }

    public bool disminuir_vida_ahogamiento()
    {
        disminuir_vida(1);

        if (vidas < 1)
        {
            return true;
        }

        return false;
    }

    public void morir()
    {
        if (mitipo == tipo.enemigo)
        {
            Invoke("destruir_gameobject", 1);
        }
        else if(mitipo == tipo.personaje_principal)
        {
            Invoke("volver_al_inicio", 1);
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

    public void volver_al_inicio()
    {
        var script = gameObject.GetComponent<personaje_volver_inicio>();
        


        if(gameObject.GetComponent("Move") != null)
        {
            var script_cliente = gameObject.GetComponent<Move>();
            script_cliente.no_arma_funcion();
            script_cliente.client.client.Send("personaje_muerto", new data_botar_objetos(script_cliente.GetID(),transform.position,null));
        }
        else
        {
            var script_servidor = gameObject.GetComponent<Move_server>();
            script_servidor.no_arma_funcion();
            script_servidor.server.server.SendToAll("personaje_muerto", new data_botar_objetos(script_servidor.GetID(), transform.position,null));
        }

        script.volver_al_inicio();

        vidas = max_vidas;

        barra.reduce(max_vidas, vidas);
    }

}
