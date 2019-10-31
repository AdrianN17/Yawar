using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barra_vida : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 vector;
    public GameObject barra;
    private Camera camera;
    public float max_counter;
    private float counter;
    void Start()
    {
        vector = barra.transform.localScale;
        camera = GameObject.FindGameObjectWithTag("Camara").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enabled)
        {
            float dt = Time.deltaTime;

            counter = counter + dt;

            if (counter > max_counter)
            {
                barra.transform.rotation = camera.transform.rotation;
                counter = 0;
            }
        }          
        
    }

    public void reduce(int max_hp, int hp)
    {
        var vec = vector;
        float porcentaje = ((float)hp / (float)max_hp);
        vec.x = porcentaje  * vector.x;

        barra.transform.localScale = vec;
    }
}
