using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventario_coleccionables : MonoBehaviour
{
    // Start is called before the first frame update
    private List<data_coleccionable> lista_coleccionables;
    private coleccionable coleccion;
    public MenuGame menugame;

    void Start()
    {
        var go = GameObject.Find("Objetos_Botados");
        coleccion = go.GetComponent<coleccionable>();

        lista_coleccionables = new List<data_coleccionable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void agregar(int tipo, int cantidad)
    {
        if(lista_coleccionables.Count==0)
        {
            lista_coleccionables.Add(new data_coleccionable(tipo, cantidad, ""));
        }
        else
        {
            var index = buscar(tipo);

            if(index != -1)
            {
                var obj = lista_coleccionables[index];
                obj.cantidad= obj.cantidad + cantidad;
            }
            else
            {
                lista_coleccionables.Add(new data_coleccionable(tipo, cantidad, ""));
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

        foreach(var data in lista_coleccionables)
        {
            if(data.tipo==tipo)
            {
                return i;
            }

            i++;
        }

        return -1;
    }

    public void agregar_lista(List<data_coleccionable> lista)
    {
        if (lista_coleccionables.Count == 0)
        {
            lista_coleccionables.AddRange(lista);
        }
        else
        {
            foreach(var lista_data in lista)
            {
                var index = buscar(lista_data.tipo);

                if (index != -1)
                {
                    var obj = lista_coleccionables[index];
                    obj.cantidad = obj.cantidad + lista_data.cantidad;
                }
                else
                {
                    lista_coleccionables.Add(lista_data);
                }
            }
        }
    }

    public List<data_coleccionable> limpiar_para_enviar()
    {
        return lista_coleccionables;
    }

    public void limpiar_principal()
    {
        lista_coleccionables.Clear();
        menugame.limpiar();
    }

    public void crear_varios(List<data_coleccionable> lista_para_crear, Vector3 pos)
    {
        foreach(var data in lista_para_crear)
        {
            coleccion.crear_nuevo_coleccionable(data.tipo-1, pos, data.cantidad);
        }
    }
}
