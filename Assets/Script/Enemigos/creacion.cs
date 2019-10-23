using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float count_envio;
    private float count_envio_max = 1;

    private List<int> ids_por_eliminar;

    private List<GameObject> lista_enemigos;

    void Start()
    {
        lista_enemigos = new List<GameObject>();
        data_pendiente = new List<data_enemigo_inicial>();
        ids_por_eliminar = new List<int>();
        id = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        time_creacion = time_creacion + dt;

        if(time_creacion>max_time_creacion && enemigos_count<max_enemigos)
        {
            enemigos_count++;

            GameObject go = (GameObject)Instantiate(prefab_enemigo, this.transform.position, Quaternion.identity);
            go.transform.SetParent(this.transform);
            var script = go.GetComponent<enemigo_1>();
            script.id = id;
            script.padre = this.gameObject;

            data_pendiente.Add(new data_enemigo_inicial(go.transform.position,id));


            lista_enemigos.Add(go);

            id++;

            time_creacion = 0;
        }

        count_envio = count_envio + dt;

        if (count_envio > count_envio_max)
        {
            enviar_datos();
            count_envio = 0;
        }

    }

    public void enviar_datos()
    {
        if (data_pendiente.Count != 0) 
        { 
            server_script.server.SendToAll("Creacion_enemigo", data_pendiente,true,1);
            data_pendiente.Clear();
        }

        if (ids_por_eliminar.Count != 0)
        {
            server_script.server.SendToAll("Eliminar_enemigo", ids_por_eliminar,true,1);
            ids_por_eliminar.Clear();
        }

        var data_enviar = new List<data_enemigo_por_segundos>();

        foreach (var data in lista_enemigos)
        {
            var script_enemigo = data.gameObject.GetComponent<enemigo_1>();
            var script_compartido = data.gameObject.GetComponent<acciones_compartidas>();

            data_enviar.Add(new data_enemigo_por_segundos(script_enemigo.id, data.transform.position, script_compartido.vidas,data.transform.rotation.eulerAngles));

        }

        server_script.server.SendToAll("Actualizar_enemigos", data_enviar,true,1);

    }


    public void saber_muertes(int id, GameObject obj)
    {
        enemigos_count--;

        ids_por_eliminar.Add(id);
        lista_enemigos.Remove(obj);
    }

    public List<data_enemigo_por_segundos> lista_enemigos_actual()
    {
        var data_enviar = new List<data_enemigo_por_segundos>();

        foreach (var data in lista_enemigos)
        {
            var script_enemigo = data.gameObject.GetComponent<enemigo_1>();
            var script_compartido = data.gameObject.GetComponent<acciones_compartidas>();

            data_enviar.Add(new data_enemigo_por_segundos(script_enemigo.id, data.transform.position, script_compartido.vidas, data.transform.rotation.eulerAngles));

        }

        return data_enviar;
    }
}
