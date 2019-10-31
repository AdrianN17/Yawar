using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class area_texto_collider : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pisando;
    public sacerdote npc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Personaje") && other.gameObject.tag == ("Personaje Principal")) 
        {
                npc.mostrar();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Personaje") && other.gameObject.tag == ("Personaje Principal")) 
        {
            npc.nomostrar();
        }
    }
}
