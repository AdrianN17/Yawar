using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransporte : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject punto;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        {
            collision.gameObject.transform.position = punto.transform.position;

        }
    }
}
