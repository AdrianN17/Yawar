using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.Broadcast;
using Assets.Libs.Esharknet.IP;
using Assets.Libs.Esharknet.Model;
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
    public GameObject item;
    public GameObject contexto;
    public int posicion_y;
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

            var lista_servidores = broadcast.GetListObtained();

            var json_data = Newtonsoft.Json.JsonConvert.SerializeObject(lista_servidores, Newtonsoft.Json.Formatting.Indented);

            Debug.Log(json_data);


            crear_gameobjects(lista_servidores);

            broadcast.Destroy();
            broadcast = null;

            Debug.Log("HECHO");
        }
    }

    
     private void crear_gameobjects(List<Data_broadcast> lista_servidores)
    {
        int i = 0;
        
        foreach (var sdatos in lista_servidores)
        {
            GameObject go = (GameObject)Instantiate(item);
            go.transform.SetParent(contexto.transform, false);

            var valores = go.GetComponent<servidor_datos>();

            valores.set_values(sdatos);

            go.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1) + ". Sala " + valores.name_server+ " " +
            valores.max_players + " de " + valores.players;

            var boton = go.transform.GetChild(1).gameObject.GetComponent<Button>();
            boton.onClick.AddListener(() => delegar_dato(valores.ip,valores.port));

            posicion_y = posicion_y - 40;

            i++;
        }

        void delegar_dato(string ip, int port)
        {

            PlayerPrefs.SetString("ip_address", ip);
            PlayerPrefs.SetInt("port", port);

            SceneManager.LoadScene("Sala_Cliente");
        }
    }
     
}
