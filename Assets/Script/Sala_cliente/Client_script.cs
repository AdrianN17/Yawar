using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client_script : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public int timeout;
    public Client client;
    public string ip;
    public bool parte_servidor;

    public GameObject prefab_adicional;
    public GameObject padre;
    public GameObject prefab_personaje;

    private List<GameObject> lista_personajes;


    void Start()
    {
        lista_personajes = new List<GameObject>();

        if (PlayerPrefs.HasKey("ip_address") && PlayerPrefs.HasKey("port"))
        {
            ip = PlayerPrefs.GetString("ip_address");
            port = (ushort)PlayerPrefs.GetInt("port");

            PlayerPrefs.DeleteKey("ip_address");
            PlayerPrefs.DeleteKey("port");
        }
        else
        {
            ip = new LocalIP().SetLocalIP();
        }

        //Debug.LogWarning("ip " + ip + " port " + port);

        client = new Client(ip, port, 0, timeout);

        if (!parte_servidor)
        {
            client.AddTrigger("Inicializador", delegate (ENet.Event net_event)
            {
                var data = client.JSONDecode(net_event.Packet);
                int index = int.Parse(data.value["id_inicial"].ToString());
                var personajes = data.value["lista_usuarios"].ToObject<List<Data_Personajes_Inicializador>>();


                foreach (var personaje in personajes)
                {
                    GameObject go = (GameObject)Instantiate(prefab_personaje, personaje.posicion, Quaternion.identity);
                    lista_personajes.Add(go);

                    go.transform.SetParent(padre.transform);

                    var go_script = go.GetComponent<Move>();
                    go_script.SetID(personaje.id);

                    if (go_script.GetID() == index)
                    {
                        var script = go.GetComponent<Move>();

                        script.client_manager = this.gameObject;
                        script.es_controlable = true;

                        var camaras = Instantiate(prefab_adicional);

                        camaras.transform.SetParent(go.transform);
                        camaras.transform.position = go.transform.position;
                        camaras.transform.localScale = new Vector3(1, 1, 1);
                    }




                }
            });


            client.AddTrigger("movimiento", delegate (ENet.Event net_event)
            {
                var data = client.JSONDecode(net_event.Packet);

                int id = int.Parse(data.value["id"].ToString());
                string tecla = data.value["tecla"].ToString();
                string orientacion = data.value["orientacion"].ToString();

                Debug.LogError("client : " + id + " array : " + lista_personajes.Count);

                var obj = lista_personajes[id].GetComponent<Move>();

                obj.movimiento_cambio(tecla, orientacion);

            });

            client.AddTrigger("salto", delegate (ENet.Event net_event)
            {
                var data = client.JSONDecode(net_event.Packet);

                int id = int.Parse(data.value["id"].ToString());
                string tecla = data.value["tecla"].ToString();

                var obj = lista_personajes[id].GetComponent<Move>();

                obj.salto_improvisado();
            });

            client.AddTrigger("Nuevo_Usuario", delegate (ENet.Event net_event)
            {
                var data = client.JSONDecode(net_event.Packet);
                var personaje = data.value["nuevo"].ToObject<Data_Personajes_Inicializador>();

                GameObject go = (GameObject)Instantiate(prefab_personaje, personaje.posicion, Quaternion.identity);
                lista_personajes.Add(go);

                go.transform.SetParent(padre.transform);

                var go_script = go.GetComponent<Move>();
                go_script.SetID(personaje.id);
            });

            client.AddTrigger("Ajustar_posicion", delegate (ENet.Event net_event)
            {
                var data = client.JSONDecode(net_event.Packet);
                var personajes_pos = data.value.ToObject<List<data_alterable>>();
                int i = 0;

                foreach (var personaje_pos in personajes_pos)
                {
                    //personaje.transform.position = personaje.posicion;
                    lista_personajes[i].transform.position = personaje_pos.posicion;
                    i++;
                }

            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        client.update();
        
    }

    void OnDestroy()
    {
        client.Destroy();
        Debug.LogWarning("Destroy gameobject");
        client = null;
    }
}
