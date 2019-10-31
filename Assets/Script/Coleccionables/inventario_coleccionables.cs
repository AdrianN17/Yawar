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
            var data = buscar(tipo);

            if(data!=null)
            {
                var obj = lista_coleccionables[lista_coleccionables.IndexOf(data)];
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

    private data_coleccionable buscar(int tipo)
    {
        foreach(var data in lista_coleccionables)
        {
            if(data.tipo==tipo)
            {
                return data;
            }
        }

        return null;
    }
}
