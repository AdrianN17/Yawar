using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGame : MonoBehaviour
{
    public GameObject Pause;
    public GameObject panel_inventario;
    public inventario_coleccionables inventario;
    public GameObject context_panel_inventario;

    public List<string> lista_nombre_objetos;
    public GameObject prefab_coleccionables_vista;

    public List<GameObject> lista_objetos_gameobject;

    public coleccionable coleccionable_data;

    public GameObject alerta;
    public Text alerta_text;

    public float default_size_y;
    public float separacion;

    void Start()
    {
        lista_objetos_gameobject = new List<GameObject>();

        lista_nombre_objetos = new lista_coleccionables().get_colecionables_string();

        Pause.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(!Pause.activeSelf);
        }
        else if( Input.GetKeyDown(KeyCode.I))
        {
            boton_inventario();
        }
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
                    script.escribir();
                }
                else
                {
                    generar(item);
                }

            }
        }

        if (lista_objetos_gameobject.Count != 0)
        {
            redimensionar();
        }
        else
        {
            redimensionar_default();
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
        var y = lista_objetos_gameobject.Count * separacion;

        var go = (GameObject)Instantiate(prefab_coleccionables_vista);
        go.transform.SetParent(context_panel_inventario.transform,false);

        var vec = go.transform.localPosition;
        vec.y = vec.y - y;
        go.transform.localPosition = vec;

        var script = go.GetComponent<Item_Inventario_script>();
        script.tipo = item.tipo;
        script.cantidad = item.cantidad;
        script.cadena = lista_nombre_objetos[item.tipo];

        script.set_sprite(coleccionable_data.img_coleccionable[item.tipo]);

        lista_objetos_gameobject.Add(go);
    }

    public void limpiar()
    {
        foreach(var obj in lista_objetos_gameobject)
        {
            Destroy(obj);
        }


        lista_objetos_gameobject.Clear();
    }

    private void redimensionar()
    {
        var y = lista_objetos_gameobject.Count * separacion;
        var rect= context_panel_inventario.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
    }

    private void redimensionar_default()
    {
        var y = default_size_y;
        var rect = context_panel_inventario.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
    }

    public void alerta_llamar(string text)
    {
        if(!alerta.activeSelf)
        {
            alerta_text.text = text;
            alerta.SetActive(true);
        }
    }

    public void alerta_cerrar()
    {
        alerta_text.text = "";
        alerta.SetActive(false);
    }


}
