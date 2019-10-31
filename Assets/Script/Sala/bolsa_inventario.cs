using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bolsa_inventario : MonoBehaviour
{
    private float counter;
    public float max_counter;

    public List<data_coleccionable> lista_coleccionables;
    private List<data_coleccionable> lista_cosas_nuevas;

    public Server_script server;
    public int index_peer;

    // Start is called before the first frame update
    void Start()
    {
        lista_coleccionables = new List<data_coleccionable>();
        lista_cosas_nuevas = new List<data_coleccionable>();
    }

    // Update is called once per frame
    void Update()
    {

        if(lista_cosas_nuevas.Count!=0)
        {
            float dt = Time.deltaTime;

            counter = counter + dt;

            if (counter > max_counter)
            {
                server.server.SendToPeerIndex("inventario_actualizar", lista_cosas_nuevas, index_peer);

                lista_cosas_nuevas.Clear();
                counter = 0;
            }
        }
            
    }


    public void agregar(int tipo, int cantidad)
    {
        if (lista_coleccionables.Count == 0)
        {
            var obj = new data_coleccionable(tipo, cantidad, "");
            lista_coleccionables.Add(obj);
            lista_cosas_nuevas.Add(obj);

        }
        else
        {
            var index = buscar(tipo);

            if (index != -1)
            {
                var obj = lista_coleccionables[index];
                obj.cantidad = obj.cantidad + cantidad;
            }
            else
            {
                var obj = new data_coleccionable(tipo, cantidad, "");
                lista_coleccionables.Add(obj);
                lista_cosas_nuevas.Add(obj);
            }
        }

    }

    public List<data_coleccionable> dar_lista()
    {
        return lista_coleccionables;
    }

    private int buscar(int tipo)
    {
        int i = 0;

        foreach (var data in lista_coleccionables)
        {
            if (data.tipo == tipo)
            {
                return i;
            }

            i++;
        }

        return -1;
    }

    public List<data_coleccionable> limpiar_para_enviar()
    {
        lista_cosas_nuevas.Clear();
        return lista_coleccionables;
    }

    public void limpiar_principal()
    {
        lista_coleccionables.Clear();
    }
}
