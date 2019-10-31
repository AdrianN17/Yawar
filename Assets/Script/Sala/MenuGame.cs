using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{
    public GameObject Pause;
    public GameObject panel_inventario;
    public inventario_coleccionables inventario;
    public GameObject context_panel_inventario;

    public List<string> lista_nombre_objetos;
    public GameObject prefab_coleccionables_vista;

    public List<GameObject> lista_objetos_gameobject;

    void Start()
    {
        lista_objetos_gameobject = new List<GameObject>();

        lista_nombre_objetos = new lista_coleccionables().get_colecionables_string();

        Pause.SetActive(false);
    }


    public void pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(!Pause.activeSelf);
        }
    }

    void Update()
    {
        pause();
    }

    public void Volver_Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void boton_inventario()
    {
        var lista = inventario.dar_lista();

        panel_inventario.SetActive(!panel_inventario.activeSelf);

        if (panel_inventario.activeSelf)
        {
            generar_objects(lista);
        }
            
    }

    public void generar_objects(List<data_coleccionable> lista)
    {
        if(lista_objetos_gameobject.Count==0)
        {
            foreach (var item in lista)
            {
                generar(item);
            }
                
        }
        else
        {
            foreach (var item in lista)
            {
                var script = buscar(item.tipo);

                if(script!=null)
                {
                    script.cantidad = item.cantidad;
                }
                else
                {
                    generar(item);
                }

            }
        }       
    }

    private Item_Inventario_script buscar(int tipo)
    {
        foreach(var go in lista_objetos_gameobject)
        {
            var script = go.GetComponent<Item_Inventario_script>();

            if(script.tipo==tipo)
            {
                return script;
            }
        }

        return null;
    }



    private void generar(data_coleccionable item)
    {
        var y = lista_objetos_gameobject.Count * 40;

        var go = (GameObject)Instantiate(prefab_coleccionables_vista);
        go.transform.SetParent(context_panel_inventario.transform,false);

        var vec = go.transform.localPosition;
        vec.y = vec.y - y;
        go.transform.localPosition = vec;

        var script = go.GetComponent<Item_Inventario_script>();
        script.tipo = item.tipo;
        script.cantidad = item.cantidad;
        script.cadena = lista_nombre_objetos[item.tipo];

        lista_objetos_gameobject.Add(go);
    }
}
