using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get_center : MonoBehaviour
{
    public Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 get_center_position()
    {
        return collider.bounds.center;
    }
}
