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

    public GameObject player_object;
    public Move player_object_script;

    private List<GameObject> lista_personajes;

    private float counter_send = 0;
    public float max_counter;

    private bool start_send = false;

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

        client = new Client(ip, port, 0, timeout);

        Debug.LogWarning("ip " + ip + " port " + port);

        client.AddTrigger("Inicializador", delegate (ENet.Event net_event)
        {

            var data = client.JSONDecode(net_event.Packet);

            Debug.LogWarning("recibi :  " + data);


            int index = int.Parse(data.value["id_inicial"].ToString());
            var personajes = data.value["lista_usuarios"].ToObject<List<data_inicial>>();


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

                    player_object = go;
                    player_object_script = script;

                    var camaras = Instantiate(prefab_adicional);

                    camaras.transform.SetParent(go.transform);
                    camaras.transform.position = go.transform.position;
                    camaras.transform.localScale = new Vector3(1, 1, 1);

                    start_send = true;
                }
            }
        });

        client.AddTrigger("Nuevo_Usuario", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var personaje = data.value["nuevo"].ToObject<data_inicial>();

            GameObject go = (GameObject)Instantiate(prefab_personaje, personaje.posicion, Quaternion.identity);
            lista_personajes.Add(go);

            go.transform.SetParent(padre.transform);

            var go_script = go.GetComponent<Move>();
            go_script.SetID(personaje.id);
        });

        client.AddTrigger("movimiento", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_tecla>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.movimiento_cambio(obj.tecla, obj.orientacion);

        });

        client.AddTrigger("enviar_posicion", delegate (ENet.Event net_event) {
            var data = client.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_por_segundos>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.normalizado(obj.posicion);
        });

    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        client.update();

        if (start_send)
        {
            counter_send = counter_send + dt;

            if (counter_send > max_counter)
            {
                client.Send("enviar_posicion", new data_inicial(player_object_script.GetID(), player_object_script.transform.position));

                counter_send = 0;
            }
        }
    }

    void OnDestroy()
    {
        client.Destroy();
        Debug.LogWarning("Destroy gameobject");
        client = null;
    }
}
