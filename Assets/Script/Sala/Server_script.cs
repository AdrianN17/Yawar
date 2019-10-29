using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Server_script : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public int max_clients;
    public int timeout;
    public Server server;
    public string ip;

    public GameObject punto_creacion;

    public GameObject prefab_personaje;
    public GameObject padre;

    private List<GameObject> lista_personajes;

    public GameObject player_inicial_servidor;
    public Move_server player_inicial_servidor_script;

    private float counter_send = 0;
    public float max_counter;

    public creacion creador_enemigos;
    public Text texto;

    public GameObject water;
    public float nivel_agua;

    private float counter_dano_agua;
    public float max_counter_dano_agua;

    void Start()
    {

        ip = new LocalIP().SetLocalIP();

        server = new Server(ip, port, max_clients, 3, timeout);

        lista_personajes = new List<GameObject>();

        lista_personajes.Add(player_inicial_servidor);
        player_inicial_servidor_script = player_inicial_servidor.GetComponent<Move_server>();

        server.AddTrigger("Connect", delegate (ENet.Event net_event)
        {

            int index = server.AddPeer(net_event) + 1;

            var lista_para_enviar = new List<data_inicial>();

            GameObject go = (GameObject)Instantiate(prefab_personaje, punto_creacion.transform.position, Quaternion.identity);
            go.transform.SetParent(padre.transform);
            go.GetComponent<Move>().SetID(index);

            lista_personajes.Add(go);

            int i = 0;

            foreach (var player in lista_personajes)
            {
                if (i == 0)
                {
                    var script = player.GetComponent<Move_server>();
                    var datos = new data_inicial(script.GetID(), player.transform.position);

                    lista_para_enviar.Add(datos);
                }
                else
                {
                    var script = player.GetComponent<Move>();
                    var datos = new data_inicial(script.GetID(), player.transform.position);

                    lista_para_enviar.Add(datos);
                }

                i++;

            }

            server.Send("Inicializador", new Listado_Usuarios(index, lista_para_enviar), net_event.Peer);

            server.SendToAllBut("Nuevo_Usuario", lista_para_enviar[index], net_event.Peer);

        });

        server.AddTrigger("Disconnect", delegate (ENet.Event net_event)
        {
            int i = server.RemovePeer(net_event);

            lista_personajes.RemoveAt(i);

            server.SendToAll("Jugador_desconectado", new data_solo_id(i));

        });

        server.AddTrigger("Pedir_enemigos", delegate (ENet.Event net_event)
        {
            server.Send("Inicializador_enemigos", creador_enemigos.lista_enemigos_actual(), net_event.Peer);
        });

        server.AddTrigger("movimiento", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_tecla>(); ;

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.movimiento_cambio(obj.tecla, obj.orientacion);

            server.SendToAllBut("movimiento", net_event.Packet, net_event.Peer, false);
            
        });

        server.AddTrigger("enviar_posicion", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_por_segundos>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.normalizado(obj.posicion, Quaternion.Euler(obj.radio));
            gameobj.set_arma_actual(obj.arma);

            

            server.SendToAllBut("enviar_posicion", net_event.Packet, net_event.Peer, false);
        });

        server.AddTrigger("chat", delegate (ENet.Event net_event)
        {
            var data = server.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_chat>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.texto.text = obj.texto;
        });

        server.AddTrigger("personaje_muerto", delegate (ENet.Event net_event)
        {
            var data = server.JSONDecode(net_event.Packet);
            var obj = data.value.ToObject<data_botar_objetos>();
            var gameobj = lista_personajes[obj.id];
            gameobj.GetComponent<personaje_volver_inicio>().volver_al_inicio();
            gameobj.GetComponent<Move>().no_arma_funcion();

            ///falta mas
            ///
            server.SendToAllBut("personaje_muerto", net_event.Packet, net_event.Peer, false);
        });


        nivel_agua = transform.TransformPoint(water.transform.position).y;

    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        counter_dano_agua = counter_dano_agua + dt;

        if (counter_dano_agua > max_counter_dano_agua)
        {
            vida_dano_agua();
            counter_dano_agua = 0;
        }


        counter_send = counter_send + dt;

        if (counter_send > max_counter)
        {
            server.SendToAll("enviar_posicion", new data_por_segundos(player_inicial_servidor_script.GetID(), player_inicial_servidor_script.transform.position, player_inicial_servidor.transform.rotation.eulerAngles, player_inicial_servidor_script.get_arma_actual()));

            counter_send = 0;
        }

        if (player_inicial_servidor_script.escribiendo)
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

            if(!string.IsNullOrWhiteSpace(texto.text))
            {
                player_inicial_servidor_script.texto.text = texto.text;

                server.SendToAll("chat", new data_chat(player_inicial_servidor_script.GetID(), texto.text));

                texto.text = "";
            }
            player_inicial_servidor_script.escribiendo = !player_inicial_servidor_script.escribiendo;

        }

    }

    void vida_dano_agua()
    {
        //var lista_enemigos = creador_enemigos.lista_enemigos_actual();
        //lista_personajes

        foreach(var personaje in lista_personajes)
        {
            var collider = transform.TransformPoint( personaje.GetComponent<get_center>().get_center_position());

            if (collider.y< nivel_agua)
            {

                var acciones = personaje.GetComponent<acciones_compartidas>();
                acciones.disminuir_vida_ahogamiento();
            }
        }
    }

 
    void OnDestroy()
    {
        server.Destroy();
        Debug.LogWarning("Destroy gameobject");
        server = null;
    }
}
