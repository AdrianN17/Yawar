using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Texto_Volatil : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject texto_gameobj;
    public Text texto;
    public float tiempo_maximo;
    private float tiempo;
    private Camera camera;
    private float counter;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("Camara").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            float dt = Time.deltaTime;

            if (texto.text != "")
            {
                tiempo = tiempo + dt;

                if (tiempo > tiempo_maximo)
                {
                    texto.text = "";
                    texto_gameobj.SetActive(false);
                }
            }
            else
            {
                tiempo = 0;
            }
        }
    }


    public void set_text(string text)
    {
        texto.text = text;
        texto_gameobj.SetActive(true);
        tiempo = 0;

    }




}
