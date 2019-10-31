using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texto_Volatil : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh texto;
    public float tiempo_maximo;
    public float tiempo;
    private Camera camera;
    public float max_counter;
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
                }
            }
            else
            {
                tiempo = 0;
            }
        }
    }




}
