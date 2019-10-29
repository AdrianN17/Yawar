using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barra_vida : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 vector;
    public GameObject barra;
    void Start()
    {
        vector = barra.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reduce(int max_hp, int hp)
    {
        var vec = vector;
        vec.x = (hp / max_hp) * vec.x;

        barra.transform.localScale = vec;
    }
    
}
