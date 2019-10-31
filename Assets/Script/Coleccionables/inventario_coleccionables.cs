using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventario_coleccionables : MonoBehaviour
{
    // Start is called before the first frame update
    private List<data_coleccionable> lista_coleccionables;

    void Start()
    {
        lista_coleccionables = new List<data_coleccionable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void agregar(int tipo)
    {
        if(lista_coleccionables.Count==0)
        {
            lista_coleccionables.Add(new data_coleccionable(tipo,1,""));
        }
        else
        {
            var index = buscar(tipo);

            if(index != -1)
            {
                var obj = lista_coleccionables[index];
                obj.cantidad= obj.cantidad + 1;
            }
            else
            {
                lista_coleccionables.Add(new data_coleccionable(tipo, 1, ""));
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
                    obj.cantidad = obj.cantidad + 1;
                }
                else
                {
                    lista_coleccionables.Add(lista_data);
                }
            }
        }
    }
}
