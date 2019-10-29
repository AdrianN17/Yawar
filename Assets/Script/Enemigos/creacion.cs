using Assets.Script.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creacion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab_enemigo;
    public int max_enemigos;
    public int enemigos_count;
    public float max_time_creacion;
    public float time_creacion;

    private int id;

    public Server_script server_script;
    private List<data_enemigo_inicial> data_pendiente;
    public float count_envio;
    public float count_envio_max;
    public float count_envio_creacion;
    public float count_envio_creacion_max;

    private List<GameObject> lista_enemigos;

    public GameObject padre_puntos;
    private List<contador_enemigos> lista_contador_enemigos;
    private List<Vector3> puntos_creacion;

    public int max_punto_cantidad;

    public Text text_enemigos;
    public coleccionable script_crear_coleccionables;

    void Start()
    {
        lista_enemigos = new List<GameObject>();
        data_pendiente = new List<data_enemigo_inicial>();


        id = 0;

        lista_contador_enemigos = new List<contador_enemigos>();
        puntos_creacion = new List<Vector3>();

        var allChildren = padre_puntos.GetComponentsInChildren<contador_enemigos>();

        foreach (var child in allChildren)
        {
            var script = child.gameObject.GetComponent<contador_enemigos>();
            lista_contador_enemigos.Add(script);
            puntos_creacion.Add(child.gameObject.transform.position);

        }


        max_enemigos = max_enemigos * lista_contador_enemigos.Count;
    }

    // Update is called once per frame
    void Update()
    {

        float dt = Time.deltaTime;

        time_creacion = time_creacion + dt;

        if(time_creacion>max_time_creacion && enemigos_count<max_enemigos)
        {
            enemigos_count++;

            int punto_nacimiento = verificar_punto_id();
            
            GameObject go = (GameObject)Instantiate(prefab_enemigo, puntos_creacion[punto_nacimiento], Quaternion.identity);
            go.transform.SetParent(this.transform);
            var script = go.GetComponent<enemigo_1>();
            script.id = id;
            script.padre = this.gameObject;
            script.punto_id = punto_nacimiento;

            script.coleccionable = new lista_coleccionables().get_coleccionable();

            data_pendiente.Add(new data_enemigo_inicial(punto_nacimiento, id, script.coleccionable));

            lista_enemigos.Add(go);

            id++;

            time_creacion = 0;

            text_enemigos.text = enemigos_count.ToString();

        }

        count_envio = count_envio + dt;

        if (count_envio > count_envio_max)
        {
            enviar_datos();
            count_envio = 0;
        }

        count_envio_creacion = count_envio_creacion + dt;

        if (count_envio_creacion > count_envio_creacion_max)
        {
            server_script.server.SendToAll("Creacion_enemigo", data_pendiente);
            data_pendiente.Clear();
            count_envio_creacion = 0;
        }



    }

    public void enviar_datos()
    {
        var data_enviar = new List<data_enemigo_por_segundos>();


        foreach (var data in lista_enemigos)
        {
            var script_enemigo = data.gameObject.GetComponent<enemigo_1>();
            var script_compartido = data.gameObject.GetComponent<acciones_compartidas>();

            if(script_enemigo.atacando)
            { 
                data_enviar.Add(new data_enemigo_por_segundos(script_enemigo.id, data.transform.position, script_compartido.vidas, data.transform.rotation.eulerAngles));
            }
        }

        if(data_enviar.Count!=0)
        { 
            server_script.server.SendToAll("Actualizar_enemigos", data_enviar);
        }
    }


    public void saber_muertes(int id, GameObject obj,int punto_id)
    {
        //crear_enemigo_cliente objeto;
        var script = obj.GetComponent<enemigo_1>();


        script_crear_coleccionables.crear_nuevo_coleccionable(script.coleccionable,script.collider.bounds.center);
            



        enemigos_count--;
        lista_contador_enemigos[punto_id].cantidad--;

        lista_enemigos.Remove(obj);


        try
        {
            text_enemigos.text = enemigos_count.ToString();
        }
        catch(Exception ex)
        {
            Debug.LogWarning("Error al acceder a una variable destruida");
        }
        
    }

    public List<data_enemigo_por_segundos> lista_enemigos_actual()
    {
        var data_enviar = new List<data_enemigo_por_segundos>();

        foreach (var data in lista_enemigos)
        {
            var script_enemigo = data.GetComponent<enemigo_1>();
            var script_compartido = data.GetComponent<acciones_compartidas>();

            data_enviar.Add(new data_enemigo_por_segundos(script_enemigo.id, data.transform.position, script_compartido.vidas, data.transform.rotation.eulerAngles));

        }

        return data_enviar;
    }

    public int verificar_punto_id()
    {

        for(int i=0; i< lista_contador_enemigos.Count ; i++)
        {
            if(lista_contador_enemigos[i].cantidad< max_punto_cantidad)
            {
                lista_contador_enemigos[i].cantidad++;

                return i;
            }
        }

        return 0;
    }


}
