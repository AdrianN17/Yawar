using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coleccionable : MonoBehaviour
{
    public lista_coleccionables lista;
    public List<GameObject> coleccionables;
    public GameObject padre;

    void Start()
    {
        lista = new lista_coleccionables();

        coleccionables.AddRange(Resources.LoadAll<GameObject>("Coleccionables") as GameObject[]);
    }

    void Update()
    {
        
    }

    public void crear_nuevo_coleccionable(int tipo,Vector3 posicion)
    {

        if(tipo==0)
        {

        }
        else
        {

            var go = Instantiate(coleccionables[tipo - 1]);
            go.transform.SetParent(this.transform);
            go.transform.position = posicion;
            go.transform.localScale = new Vector3(0.1f,0.1f, 0.1f);


        }
    }
}
