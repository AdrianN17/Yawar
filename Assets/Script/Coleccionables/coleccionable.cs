using Assets.Script.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coleccionable : Convert_vector
{
    public lista_coleccionables lista;
    public List<GameObject> coleccionables;
    public List<Sprite> img_coleccionable;
    public GameObject padre;
    public List<coleccionable_data> listado_actual;
    private int id_coleccionable = 0;
    public float nivel_agua_y;
    public bool is_cliente;

    void Start()
    {
        lista = new lista_coleccionables();

        coleccionables.AddRange(Resources.LoadAll<GameObject>("Coleccionables") as GameObject[]);

        img_coleccionable = new List<Sprite>();
        img_coleccionable.AddRange(Resources.LoadAll<Sprite>("Foto_Coleccionables") as Sprite[]);

        listado_actual = new List<coleccionable_data>();
    }

    void Update()
    {

    }

    public coleccionable_data crear_nuevo_coleccionable(int tipo, Vector3 posicion, int cantidad=1)
    {
        if(posicion.y<nivel_agua_y)
        {
            return null;
        }



        if(tipo==-1)
        {
            return null;
        }
        else
        {

            var go = Instantiate(coleccionables[tipo]);
            go.transform.SetParent(this.transform);
            go.transform.position = posicion;
            go.transform.localScale = new Vector3(0.25f,0.25f, 0.25f);

            var script = go.GetComponent<coleccionable_data>();
            script.tipo = tipo;
            script.cantidad = cantidad;
            script.id_coleccionable = id_coleccionable;

            script.padre_script = this;
            listado_actual.Add(script);


            id_coleccionable++;

            return script;
        }
    }

    public void eliminar(coleccionable_data script)
    {
        listado_actual.Remove(script);
    }

    public List<data_colecionable_con_id> get_actual()
    {
        var listado = new List<data_colecionable_con_id>();

        foreach(var script in listado_actual)
        {
            if(!script.enviar)
            {
                listado.Add(new data_colecionable_con_id(script.id_coleccionable, script.tipo, script.cantidad, vec_to_obj(script.transform.position)));
                script.enviar = true;
            }
                
        }

        return listado;
    }

    public void actualizar(List<data_colecionable_con_id> listado) //para cliente
    {
        foreach(var data in listado)
        {
            var obj = buscar(data.id_colecionable);

            if(obj)
            {
                if(obj.tipo==data.tipo)
                {
                    obj.cantidad = data.cantidad;
                    obj.transform.position = obj_to_vec(data.vector);
                    validar(obj, obj.transform.position.y);

                }
                else
                {

                    listado_actual.Remove(obj);
                    Destroy(obj.gameObject);

                    var script = crear_nuevo_coleccionable(data.tipo, obj_to_vec(data.vector), data.cantidad);

                    if(script!=null)
                    {
                        script.actualizable_enviar = true;
                    }

                }
            }
            else
            {
                var script = crear_nuevo_coleccionable(data.tipo, obj_to_vec(data.vector), data.cantidad);

                if (script != null)
                {
                    script.actualizable_enviar = true;
                }
            }
        }


        limpiar_coleccionables();
    }

    public void validar(coleccionable_data data, float y)
    {
        if (y < nivel_agua_y)
        {
            try
            {
                listado_actual.Remove(data);
                Destroy(data.gameObject);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Colecionable error al borrar");
            }
        }
    }

    public coleccionable_data buscar(int id_coleccionable)
    {

        foreach (var script in listado_actual)
        {
            if(script.id_coleccionable==id_coleccionable)
            {
                return script;
            }
        }
        return null;
    }

    public List<data_colecionable_con_id> ejercer_envio()
    {
        var listado = new List<data_colecionable_con_id>();

        foreach (var script in listado_actual)
        {
            
            listado.Add(new data_colecionable_con_id(script.id_coleccionable, script.tipo, script.cantidad, vec_to_obj(script.transform.position)));
            

        }

        return listado;
    }

    public void crear_varios_coleccionalbes(List<data_colecionable_con_id> lista)
    {
        foreach(var data in lista)
        {
            crear_nuevo_coleccionable(data.tipo, obj_to_vec(data.vector), data.cantidad);
        }
    }

    public void limpiar_coleccionables()
    {
        for (var i = listado_actual.Count - 1; i >= 0; i--)
        {
            try
            {
                var script = listado_actual[i];

                if (!script.actualizable_enviar)
                {
                    listado_actual.RemoveAt(i);
                    Destroy(script.gameObject);
                }
            }
            catch(Exception ex)
            {
                listado_actual.RemoveAt(i);
            }
        }

        foreach (var script in listado_actual)
        {
            script.actualizable_enviar = false;
        }

    }
}
