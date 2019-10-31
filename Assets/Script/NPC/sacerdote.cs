using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sacerdote : MonoBehaviour
{
    // Start is called before the first frame update
    public Text texto_npc;
    public List<string> lista_texto;
    public GameObject mensaje;
    public Animator animacionSacerdote;
    public int index;
    void Start()
    {
        lista_texto = new List<string> { "Que es lo que tienes para mi? - pulsa A para entregar objetos" };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mostrar()
    {

        animacionSacerdote.SetBool("PedirObjetos", true);
        mensaje.SetActive(true);
        texto_npc.text = lista_texto[index];
    }

    public void nomostrar()
    {
        animacionSacerdote.SetBool("PedirObjetos", false);
        mensaje.SetActive(false);
        texto_npc.text = " ";
    }
}

