using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensor_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemigo;
    public enemigo_1 script;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        { 
            script.nuevo_usuario(collision.gameObject.GetComponent<get_center>());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        {
            script.eliminar_usuario(collision.gameObject.GetComponent<get_center>());
        }
    }
}
