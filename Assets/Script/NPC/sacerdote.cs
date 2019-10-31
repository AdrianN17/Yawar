using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sacerdote : MonoBehaviour
{
    // Start is called before the first frame update
    public Text texto_npc;
    public List<string> lista_texto;
    public int index;
    void Start()
    {
        texto_npc = GameObject.FindGameObjectWithTag("Texto_Principal").GetComponent<Text>();


        lista_texto = new List<string> { "Que es lo que tienes para mi? "};

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mostrar()
    {
        texto_npc.text = lista_texto[index];
    }

    public void nomostrar()
    {
        texto_npc.text = " ";
    }
}

