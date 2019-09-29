using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibilidad : MonoBehaviour
{
    // Start is called before the first frame update

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    public void Disable()
    {
        enabled = false;
    }
}
