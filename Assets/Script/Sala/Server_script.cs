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

    void Start()
    {
        ip = new LocalIP().SetLocalIP();

        server = new Server(ip, port, max_clients, 0, timeout);

        lista_personajes = new List<GameObject>();

        lista_personajes.Add(player_inicial_servidor);
        player_inicial_servidor_script = player_inicial_servidor.GetComponent<Move_server>();

        server.AddTrigger("Connect", delegate (ENet.Event net_event)
        {

            int index = server.AddPeer(net_event) + 1;

            GameObject go = (GameObject)Instantiate(prefab_personaje, punto_creacion.transform.position, Quaternion.identity);
            go.transform.SetParent(padre.transform);
            go.GetComponent<Move>().SetID(index);

            lista_personajes.Add(go);

            var lista_para_enviar = new List<data_inicial>();

            int i = 0;

            foreach (var player in lista_personajes)
            {
                if(i==0)
                {
                    var script = player.GetComponent<Move_server>();
                    var datos = new data_inicial(script.GetID(), player.transform.position);

                    lista_para_enviar.Add(datos);
                }
                else
                { 
                    var script = player.GetComponent<Move>();
                    var datos = new data_inicial(script.GetID(),player.transform.position);

                    lista_para_enviar.Add(datos);
                }

                i++;
            }



            server.Send("Inicializador", new Dictionary<string, dynamic>()
                    { { "id_inicial" , index} ,{"lista_usuarios",lista_para_enviar}
                }, net_event.Peer);

            server.SendToAllBut("Nuevo_Usuario", new Dictionary<string, dynamic>()
                    {{"nuevo",lista_para_enviar[index]}
                }, net_event.Peer);
        });

        server.AddTrigger("movimiento", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_tecla>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.movimiento_cambio(obj.tecla, obj.orientacion);

            server.SendToAllBut("movimiento", net_event.Packet, net_event.Peer, false);
            
        });

        server.AddTrigger("enviar_posicion", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            var obj = data.value.ToObject<data_por_segundos>();

            var gameobj = lista_personajes[obj.id].GetComponent<Move>();

            gameobj.normalizado(obj.posicion);

            server.SendToAllBut("enviar_posicion", net_event.Packet, net_event.Peer, false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        server.update();


        counter_send = counter_send + dt;

        if (counter_send > max_counter)
        {
            server.SendToAll("enviar_posicion", new data_inicial(player_inicial_servidor_script.GetID(), player_inicial_servidor_script.transform.position));

            counter_send = 0;
        }
        
    }

    void OnDestroy()
    {
        server.Destroy();
        Debug.LogWarning("Destroy gameobject");
        server = null;
    }
}
