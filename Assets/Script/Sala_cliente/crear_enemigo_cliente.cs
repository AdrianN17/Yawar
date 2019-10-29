using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crear_enemigo_cliente : MonoBehaviour
{
    public GameObject prefab_enemigo_1;

    public List<GameObject> lista_enemigos;

    public GameObject padre_puntos;
    private List<Vector3> puntos_creacion;



    // Start is called before the first frame update
    void Start()
    {
        lista_enemigos = new List<GameObject>();
        puntos_creacion = new List<Vector3>();

        var allChildren = padre_puntos.GetComponentsInChildren<contador_enemigos>();

        foreach (var child in allChildren)
        {
            puntos_creacion.Add(child.gameObject.transform.position);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void crear_enemigos(List<data_enemigo_inicial> data_list)
    {
        foreach (var enemigo in data_list)
        {
            var dato_gameobject = buscar_item(enemigo.id);

            if (dato_gameobject == null)
            {
                var ene = (GameObject)Instantiate(prefab_enemigo_1, puntos_creacion[enemigo.id_posicion], Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;
                script.es_servidor = false;

                script.coleccionable = enemigo.coleccionable;

                lista_enemigos.Add(ene);
            }
        }
        
    }

    public GameObject buscar_item(int id)
    {

        foreach(var enemigo in lista_enemigos)
        {
            if(enemigo.GetComponent<enemigo_1>().id == id)
            {
                return enemigo;
            }
        }

        return null;
    }


    public void actualizar_enemigos(List<data_enemigo_por_segundos> data_list)
    {
        for (var i = data_list.Count - 1; i >= 0; i--)
        {
            var enemigo = data_list[i];
            var dato_gameobject = buscar_item(enemigo.id);

            if(dato_gameobject != null)
            {
                var script1 = dato_gameobject.GetComponent<enemigo_1>();
                var script2 = dato_gameobject.GetComponent<acciones_compartidas>();
                

                script1.transform.position = enemigo.pos;
                script2.vidas = enemigo.vida;

                script1.transform.rotation = Quaternion.Euler(enemigo.radio);
                script1.rb.MoveRotation(Quaternion.Euler(enemigo.radio));

                if (enemigo.vida < 1)
                {
                    script2.morir();
                }
            }
            else
            {
                var ene = (GameObject)Instantiate(prefab_enemigo_1, enemigo.pos, Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;
                script.coleccionable = enemigo.coleccionable;
                script.es_servidor = false;

                var compartido = ene.GetComponent<acciones_compartidas>();
                compartido.vidas = enemigo.vida;


                lista_enemigos.Add(ene);
            }

        }
    }

    public void crear_enemigo_creacion_player(List<data_enemigo_por_segundos> data_list)
    {
        foreach(var enemigo in data_list)
        {
            var dato_gameobject = buscar_item(enemigo.id);

            if (dato_gameobject == null)
            {
                var ene = (GameObject)Instantiate(prefab_enemigo_1, enemigo.pos, Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;
                script.coleccionable = enemigo.coleccionable;
                script.es_servidor = false;

                var compartido = ene.GetComponent<acciones_compartidas>();
                compartido.vidas = enemigo.vida;


                lista_enemigos.Add(ene);
            }
        }
    }

}
