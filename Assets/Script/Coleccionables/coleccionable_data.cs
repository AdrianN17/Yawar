using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coleccionable_data : MonoBehaviour
{
    public int id;
    public int tipo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        {
            if (GameObject.Find("Server_Manager"))
            {
                if(other.gameObject.tag == ("Personaje Principal"))
                {
                    var script = GameObject.FindGameObjectWithTag("Inventario_Main").GetComponent<inventario_coleccionables>();
                    script.agregar(tipo);
                }
                else
                {
                    var script = other.gameObject.GetComponent<bolsa_inventario>();
                    script.agregar(tipo);
                }
                    
            }
              
            Destroy(this.gameObject);
        }

        
    }
}
