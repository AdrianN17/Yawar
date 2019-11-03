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
    public GameObject Partida;
    public GameObject Panel_Servidores;
    public GameObject Panel_Creditos;
    public GameObject Btn_Atras;
    public GameObject Btn_Buscar;
    public GameObject Culturas;
    public int timedelay;
    private Broadcast_receive broadcast;
    private string ip;
    public GameObject item;
    public GameObject contexto;
    public float default_size_y;
    public float separacion;

    private bool buscando;
    //audio
    public AudioClip click;
    AudioSource fuenteAudio;

    void Start()
    {
        buscando = false;
        this.ip = new LocalIP().SetLocalIP();
        fuenteAudio = GetComponent<AudioSource>();
        fuenteAudio.clip = click;
    }

    // Update is called once per frame
    void Update()
    {
        if(buscando)
        {

        }
    }

    public void On_Nuevo()
    {
        SceneManager.LoadScene("Sala");
    }

    public void On_Unirse()
    {
        //SceneManager.LoadScene("Sala_cliente");
        fuenteAudio.Play();
        Partida.SetActive(false);
        Panel_Servidores.SetActive(true);
    }

    public void On_Buscar()
    {
        fuenteAudio.Play();
        Btn_Buscar.GetComponent<Button>().interactable = false;
        broadcast = new Broadcast_receive(ip,22124,timedelay);
        Invoke("finalizar_listado", 5);
    }

    public void On_Atras()
    {
        fuenteAudio.Play();

        Panel_Creditos.SetActive(false);
        Panel_Servidores.SetActive(false);
        Partida.SetActive(true);
    }

    public void On_Configuracion()
    {
        fuenteAudio.Play();
    }

    public void On_Credito()
    {
        fuenteAudio.Play();
        Panel_Creditos.SetActive(true);
        Partida.SetActive(false);
        Panel_Servidores.SetActive(false);
    }

    public void On_Culturas()
    {
        fuenteAudio.Play();
        Partida.SetActive(false);
        Culturas.SetActive(true);
    }


    private void finalizar_listado()
    {
        if (broadcast != null)
        {
            Btn_Buscar.GetComponent<Button>().interactable = true;

            var lista_servidores = broadcast.GetListObtained();

            if (lista_servidores.Count!=0)
            {
                redimensionar(lista_servidores.Count);
            }
            else
            {
                redimensionar_default();
            }


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

            var y = i * separacion;

            var vec = go.transform.localPosition;
            vec.y = vec.y - y;
            go.transform.localPosition = vec;

            i++;
        }

        void delegar_dato(string ip, int port)
        {

            PlayerPrefs.SetString("ip_address", ip);
            PlayerPrefs.SetInt("port", port);

            SceneManager.LoadScene("Sala_Cliente");
        }
    }

    private void redimensionar(int count)
    {
        var rect = contexto.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, count*50);
    }

    private void redimensionar_default()
    {
        var y = default_size_y;
        var rect = contexto.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
    }

}
