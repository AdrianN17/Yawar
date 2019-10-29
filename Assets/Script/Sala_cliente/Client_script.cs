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

    public crear_enemigo_cliente script_crearenemigo;

    public Text texto;

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

            var obj = data.value.ToObject<Listado_Usuarios>();

            int index = obj.id;
            var personajes = obj.lista;


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


                    var script_compartido = go.GetComponent<acciones_compartidas>();
                    script_compartido.personaje_principal();

                }
            }

            client.Send("Pedir_enemigos", null);
        });

        client.AddTrigger("Inicializador_enemigos", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            
            var enemigos = data.value.ToObject<List<data_enemigo_por_segundos>>();

            script_crearenemigo.crear_enemigo_creacion_player(enemigos);

            start_send = true;
        });

        client.AddTrigger("Nuevo_Usuario", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var personaje = data.value.ToObject<data_inicial>();

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

            gameobj.normalizado(obj.posicion, Quaternion.Euler(obj.radio));

            gameobj.set_arma_actual(obj.arma);
        });

        client.AddTrigger("Actualizar_enemigos", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<List<data_enemigo_por_segundos>>();

            script_crearenemigo.actualizar_enemigos(obj);


        });

        client.AddTrigger("chat", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_chat>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.texto.text = obj.texto;
        });

        client.AddTrigger("personaje_muerto", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = data.value.ToObject<data_botar_objetos>();
            var gameobj = lista_personajes[obj.id];
            gameobj.GetComponent<personaje_volver_inicio>().volver_al_inicio();
            gameobj.GetComponent<Move>().no_arma_funcion();

            ///falta mas
        });

        client.AddTrigger("Jugador_desconectado", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = data.value.ToObject<data_solo_id>();

            lista_personajes.Remove(obj.id);

        });

    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (start_send)
        {
            counter_send = counter_send + dt;

            if (counter_send > max_counter)
            {
                client.Send("enviar_posicion", new data_por_segundos(player_object_script.GetID(), player_object_script.transform.position,player_object_script.transform.rotation.eulerAngles,player_object_script.get_arma_actual()));

                counter_send = 0;
            }

            if (player_object_script.escribiendo)
            {
                foreach (char c in Input.inputString)
                {
                    if (c == '\b')
                    {
                        if (texto.text.Length != 0)
                        {
                            texto.text = texto.text.Substring(0, texto.text.Length - 1);
                        }
                    }
                    else if ((c == '\n') || (c == '\r'))
                    {

                    }
                    else
                    {
                        texto.text += c;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {

                if (!string.IsNullOrWhiteSpace(texto.text))
                {
                    player_object_script.texto.text = texto.text;

                    client.Send("chat", new data_chat(player_object_script.GetID(), texto.text));

                    texto.text = "";
                }
                player_object_script.escribiendo = !player_object_script.escribiendo;

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
