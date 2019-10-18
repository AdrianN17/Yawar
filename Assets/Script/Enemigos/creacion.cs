using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creacion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab_enemigo;
    public int max_enemigos;
    public int enemigos_count;
    public float max_time_creacion;
    public float time_creacion;

    private int id;

    void Start()
    {
        id = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        time_creacion = time_creacion + dt;

        if(time_creacion>max_time_creacion && enemigos_count<max_enemigos)
        {
            enemigos_count++;

            GameObject go = (GameObject)Instantiate(prefab_enemigo, this.transform.position, Quaternion.identity);
            go.transform.SetParent(this.transform);
            var script = go.GetComponent<enemigo_1>();
            script.id = id;
            script.padre = this.gameObject;

            id++;


            time_creacion = 0;
        }
    }

    public void saber_muertes()
    {
        enemigos_count--;
    }
}
