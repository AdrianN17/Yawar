using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class acciones_compartidas : Convert_vector
{
    public Rigidbody rb;
    public Collider collider;
    public Collider collider_arma;
    public arma_melee_atacar ama;

    public GameObject prefab_barra;
    public barra_vida barra;

    public enum tipo {personaje_principal, personaje, enemigo};
    public enum tipo_muerte { normal,ahogamiento};
    public tipo mitipo;

    public int max_vidas;
    public int vidas;

    //animator
    public Animator anim;
    

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
            morir(tipo_muerte.normal);
        }

        return vidas;
    }

    public int  disminuir_vida_ahogamiento()
    {
        vidas = vidas - 1;

        if (mitipo != tipo.personaje_principal)
        {
            barra.reduce(max_vidas, vidas);
        }

        if (vidas < 1)
        {
            morir(tipo_muerte.ahogamiento);
        }

        return vidas;
    }

    public void morir(tipo_muerte tp)
    {
        if (mitipo == tipo.enemigo)
        {
            if(tp==tipo_muerte.normal)
            {
                anim.SetTrigger("Morir");
                Invoke("destruir_gameobject", 2.1f);
            }
            else
            {
                anim.SetTrigger("Ahogar");
            }
                

            barra.reduce(max_vidas, 0);
        }
        else
        {
            if (tp == tipo_muerte.normal)
            {
                anim.SetBool("Morir", true);

                if (mitipo == tipo.personaje)
                {
                    barra.reduce(max_vidas, 0);
                }
            }
            else
            {
                anim.SetBool("Ahogar", true);

                if (mitipo == tipo.personaje)
                {
                    barra.reduce(max_vidas, 0);
                }
            }
        }
            
    }

    public void muerte_final()
    {
        if (mitipo == tipo.enemigo)
        {
            
        }
        else
        {
            anim.SetBool("Morir", false);
            anim.SetBool("Ahogar", false);
            volver_al_inicio();
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

        if (gameObject.GetComponent("Move") != null)
        {
            var script_cliente = gameObject.GetComponent<Move>();
            script_cliente.no_arma_funcion();
            //script_cliente.client.client.Send("personaje_muerto", new data_botar_objetos(script_cliente.GetID(),vec_to_obj(transform.position),null));
        }
        else
        {
            var script_servidor = gameObject.GetComponent<Move_server>();
            script_servidor.no_arma_funcion();
            //script_servidor.server.server.SendToAll("personaje_muerto", new data_botar_objetos(script_servidor.GetID(), vec_to_obj(transform.position),null));
        }

        script.volver_al_inicio();

        vidas = max_vidas;

        if(mitipo != tipo.personaje_principal)
        {
            barra.reduce(max_vidas, vidas);
        }
            
    }

    public int get_tipo()
    {
        return (int)mitipo;
    }

    public tipo_muerte get_tipomuerte(int i)
    {
        return (tipo_muerte)i;
    }

}
