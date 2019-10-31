using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coleccionable : MonoBehaviour
{
    public lista_coleccionables lista;
    public List<GameObject> coleccionables;
    public List<Sprite> img_coleccionable;
    public GameObject padre;


    void Start()
    {
        lista = new lista_coleccionables();

        coleccionables.AddRange(Resources.LoadAll<GameObject>("Coleccionables") as GameObject[]);

        img_coleccionable = new List<Sprite>();
        img_coleccionable.AddRange(Resources.LoadAll<Sprite>("Foto_Coleccionables") as Sprite[]);
    }

    void Update()
    {
        
    }

    public void crear_nuevo_coleccionable(int tipo, Vector3 posicion, int cantidad=1)
    {

        if(tipo==0)
        {

        }
        else
        {

            var go = Instantiate(coleccionables[tipo - 1]);
            go.transform.SetParent(this.transform);
            go.transform.position = posicion;
            go.transform.localScale = new Vector3(0.25f,0.25f, 0.25f);

            var script = go.GetComponent<coleccionable_data>();
            script.tipo = tipo-1;
            script.cantidad = cantidad;


        }
    }
}
