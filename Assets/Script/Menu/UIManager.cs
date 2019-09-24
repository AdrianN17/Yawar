using Assets.Libs.Esharknet.Broadcast;
using Assets.Libs.Esharknet.IP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject Panel_Servidores;
    public GameObject Btn_Atras;
    public GameObject Btn_Buscar;
    public int timedelay;
    private Broadcast_receive broadcast;
    private string ip;
    // Start is called before the first frame update
    void Start()
    {
        this.ip = new LocalIP().SetLocalIP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On_Nuevo()
    {
        SceneManager.LoadScene("Sala");
    }

    public void On_Unirse()
    {
        //SceneManager.LoadScene("Sala_cliente");
        Buttons.SetActive(false);
        Panel_Servidores.SetActive(true);
    }

    public void On_Buscar()
    {
        Btn_Buscar.GetComponent<Button>().interactable = false;
        broadcast = new Broadcast_receive(ip,22124,timedelay);
        Invoke("finalizar_listado", 5);
    }

    public void On_Atras()
    {
        Panel_Servidores.SetActive(false);
        Buttons.SetActive(true);
    }

    public void On_Configuracion()
    {

    }

    private void finalizar_listado()
    {
        if (broadcast != null)
        {
            Btn_Buscar.GetComponent<Button>().interactable = true;
            //crear_gameobjects(broadcast.getLista());
            broadcast.Destroy();
            broadcast = null;

            Debug.Log("HECHO");
        }
    }

    /*
     private void crear_gameobjects(List<servidor_datos> lista_servidores)
    {
        int i = 0;
        int posicion_y = 80;
        foreach (servidor_datos sdatos in lista_servidores)
        {
            GameObject go = (GameObject)Instantiate(item_servidor, new Vector3(0, 0, 0), Quaternion.identity);
            go.transform.SetParent(contexto.transform);

            go.transform.position = new Vector3(300, posicion_y + 75, 0);

            var valores = go.GetComponent<servidor_datos>();
            valores.ip = sdatos.ip;
            valores.clientes = sdatos.clientes;
            valores.max_clientes = sdatos.max_clientes;



            go.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1) + ". Partida " + sdatos.max_clientes + " de " + sdatos.clientes;

            var boton = go.transform.GetChild(1).gameObject.GetComponent<Button>();
            boton.onClick.AddListener(() => delegar_dato(sdatos.ip));

            posicion_y = posicion_y - 40;

            i++;
        }

        void delegar_dato(string ip)
        {

            PlayerPrefs.SetString("direccion_ip", ip);
            SceneManager.LoadScene("NuevaPartida_Cliente");
        }
    }
     */
}
