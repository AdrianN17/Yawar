using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crear_enemigo_cliente : MonoBehaviour
{
    public GameObject prefab_enemigo_1;

    public List<GameObject> lista_enemigos;


    // Start is called before the first frame update
    void Start()
    {
        lista_enemigos = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void crear_enemigos_lista(List<data_enemigo_inicial> data_list)
    {
        foreach (var enemigo in data_list)
        {
            var dato_gameobject = buscar_item(enemigo.id);

            if (dato_gameobject == null)
            { 
                var ene = (GameObject)Instantiate(prefab_enemigo_1, enemigo.pos, Quaternion.identity);
                ene.transform.SetParent(this.transform);
                var script = ene.GetComponent<enemigo_1>();
                script.id = enemigo.id;
                script.padre = this.gameObject;

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
        foreach (var enemigo in data_list)
        {
            var dato_gameobject = buscar_item(enemigo.id);

            if(dato_gameobject != null)
            {
                var script1 = dato_gameobject.GetComponent<enemigo_1>();
                var script2 = dato_gameobject.GetComponent<acciones_compartidas>();
                

                script1.transform.position = enemigo.pos;
                script2.vidas = enemigo.vida;

                script1.transform.rotation = Quaternion.Euler(enemigo.radio);
                script1.rb.MoveRotation(Quaternion.Euler(enemigo.radio));
            }

        }
    }

    public void eliminar_enemigos(List<int> data_list)
    {
        foreach (var ids in data_list)
        {
            var dato_gameobject = buscar_item(ids);

            if (dato_gameobject != null)
            {
                lista_enemigos.Remove(dato_gameobject);
                Destroy(dato_gameobject);
            }
        }
    }

    public void crear_enemigo_creacion_player(List<data_enemigo_por_segundos> data_list)
    {
        foreach(var enemigo in data_list)
        {
            var ene = (GameObject)Instantiate(prefab_enemigo_1, enemigo.pos, Quaternion.identity);
            ene.transform.SetParent(this.transform);
            var script = ene.GetComponent<enemigo_1>();
            script.id = enemigo.id;
            script.padre = this.gameObject;

            lista_enemigos.Add(ene);
        }
    }

}
