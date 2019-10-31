using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crear_enemigo_cliente : Convert_vector
{
    public GameObject prefab_enemigo_1;

    public List<GameObject> lista_enemigos;

    public GameObject padre_puntos;
    private List<Vector3> puntos_creacion;
    public Text cantidad_enemigos;



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
                

                script1.transform.position = obj_to_vec(enemigo.pos);
                script2.vidas = enemigo.vida;

                script1.transform.rotation = Quaternion.Euler(obj_to_vec(enemigo.radio));
                script1.rb.MoveRotation(script1.transform.rotation);

            }
            else
            {
                var ene = (GameObject)Instantiate(prefab_enemigo_1, obj_to_vec(enemigo.pos), Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;
                script.coleccionable = enemigo.coleccionable;
                script.es_servidor = false;

                var compartido = ene.GetComponent<acciones_compartidas>();
                compartido.vidas = enemigo.vida;


                lista_enemigos.Add(ene);

                cantidad_enemigos.text = lista_enemigos.Count.ToString();
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
                var ene = (GameObject)Instantiate(prefab_enemigo_1, obj_to_vec(enemigo.pos), Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;
                script.coleccionable = enemigo.coleccionable;
                script.es_servidor = false;

                var compartido = ene.GetComponent<acciones_compartidas>();
                compartido.vidas = enemigo.vida;


                lista_enemigos.Add(ene);

                cantidad_enemigos.text = lista_enemigos.Count.ToString();
            }
        }
    }

    public void contar_muertes(GameObject enemigo)
    {
        lista_enemigos.Remove(enemigo);
        cantidad_enemigos.text = lista_enemigos.Count.ToString();
    }

}
