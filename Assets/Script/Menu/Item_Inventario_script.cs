using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Inventario_script : MonoBehaviour
{
    // Start is called before the first frame update
    public Text texto;
    public string cadena;
    public int tipo;
    public int cantidad;
    void Start()
    {
        escribir();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void escribir()
    {
        texto.text = cadena + " X" + cantidad;
    }
}
