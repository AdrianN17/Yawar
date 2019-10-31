using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Inventario_script : MonoBehaviour
{
    // Start is called before the first frame update
    public Text texto;
    public Image img;
    public string cadena;
    public int tipo;
    public int cantidad;
    public bool check;
    void Start()
    {
        check = false;
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

    public void set_sprite(Sprite sprite)
    {
        if(check==false)
        {
            img.sprite = sprite;
            img.rectTransform.sizeDelta = new Vector2(210, 150);
            check = true;
        }
            
    }
}
