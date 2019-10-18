using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acciones_compartidas : MonoBehaviour
{
    public Rigidbody rb;
    public Collider collider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void empujon(Vector3 point)
    {
        Vector3 dir = collider.bounds.center - point;
        dir.Normalize();

        rb.AddForce(dir * 2500 * rb.mass);

        Debug.LogError("aaaa");


    }
}
