using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class acciones_compartidas : Convert_vector
{
    public Rigidbody rb;
    public Collider collider;
    public arma_melee_atacar ama;

    public GameObject prefab_barra;
    public barra_vida barra;

    public enum tipo { personaje_principal, personaje, enemigo };
    public enum tipo_muerte { normal, ahogamiento };
    public tipo mitipo;

    public int max_vidas;
    public int vidas;

    //animator
    public Animator anim;

    public List<arma_melee_atacar> lista_armas;
    public int index_arma;

    public Sonidos_Pj sound;

    public bool muerto;


    // Start is called before the first frame update
    void Start()
    {
        muerto = false;

        sound = GameObject.FindGameObjectWithTag("Sonido_main").GetComponent<Sonidos_Pj>();


        lista_armas = new List<arma_melee_atacar>();


        vidas = max_vidas;

        if (mitipo != tipo.personaje_principal)
        {
            barra = prefab_barra.GetComponent<barra_vida>();
        }
        else
        {
            prefab_barra.SetActive(false);
        }

        if (mitipo != tipo.enemigo)
        {
            lista_armas.Add(transform.GetChild(2).GetComponent<arma_melee_atacar>());//mazo
            lista_armas.Add(transform.GetChild(1).GetComponent<arma_melee_atacar>());//lanza
        }


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void empujon()
    {
        anim.SetTrigger("RecibirDano");
        sound.emitir_grito_dano(this.enabled);
    }

    public void ataque()
    {

        if (mitipo == tipo.enemigo)
        {
            ama.golpear_todos();
        }
        else
        {
            lista_armas[index_arma].golpear_todos();
        }

    }

    public int disminuir_vida(int dano)
    {
        vidas = vidas - dano;

        if (!muerto) 
        { 
            if (mitipo != tipo.personaje_principal)
            {
                barra.reduce(max_vidas, vidas);
            }

            if (vidas<1 )
            {
                muerto = true;
                morir(tipo_muerte.normal);
            }
        }

        return vidas;
    }

    public int disminuir_vida_ahogamiento()
    {
        vidas = vidas - 1;

        if (!muerto)
        {
            if (mitipo != tipo.personaje_principal)
            {
                barra.reduce(max_vidas, vidas);
            }

            if (vidas < 1)
            {
                muerto = true;
                morir(tipo_muerte.ahogamiento);
            }
        }

        return vidas;
    }

    public void morir(tipo_muerte tp)
    {
        sound.emitir_grito_muerte(this.enabled);

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
                Invoke("destruir_gameobject", 3.5f);
            }
                

            barra.reduce(max_vidas, 0);
        }
        else
        {
            if (tp == tipo_muerte.normal)
            {
                anim.SetBool("Morir_b", true);
                anim.SetTrigger("Morir");


                if (mitipo == tipo.personaje)
                {
                    barra.reduce(max_vidas, 0);
                }
            }
            else
            {
                anim.SetBool("Ahogar_b", true);
                anim.SetTrigger("Ahogar");


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
            Destroy(this.gameObject);
        }
        else
        {
            anim.SetBool("Morir_b", false);
            anim.SetBool("Ahogar_b", false);
            volver_al_inicio();
        }
  
    }

    public void personaje_principal()
    {
        mitipo = tipo.personaje_principal;
    }

    public void volver_al_inicio()
    {
        var script = gameObject.GetComponent<personaje_volver_inicio>();
        var obj = GameObject.Find("Server_Manager"); 

        if(obj!=null)
        {
            var server = obj.GetComponent<Server_script>();
            var bolsa = GameObject.FindGameObjectWithTag("Inventario_Main");
            var bolsa_script = bolsa.GetComponent<inventario_coleccionables>();

            if (gameObject.tag == "Personaje Principal")
            {
                var script_servidor = gameObject.GetComponent<Move_server>();
                script_servidor.no_arma_funcion();

                
                var lista = bolsa_script.limpiar_para_enviar();

                server.server.SendToAll("personaje_muerto", new data_botar_objetos(script_servidor.GetID(), vec_to_obj(collider.bounds.center), lista));

                bolsa_script.crear_varios(lista, collider.bounds.center);

                bolsa_script.limpiar_principal();
            }
            else
            {
                var script_cliente = gameObject.GetComponent<Move>();
                script_cliente.no_arma_funcion();

                var bolsa_2 = gameObject.GetComponent<bolsa_inventario>();

                var lista = bolsa_2.limpiar_para_enviar();

                server.server.SendToAll("personaje_muerto", new data_botar_objetos(script_cliente.GetID(), vec_to_obj(collider.bounds.center), lista));

                bolsa_script.crear_varios(lista, collider.bounds.center);

                bolsa_2.limpiar_principal();
            }
        }

        script.volver_al_inicio();

        vidas = max_vidas;

        if(mitipo != tipo.personaje_principal)
        {
            barra.reduce(max_vidas, vidas);
        }

        muerto = false;
            
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
