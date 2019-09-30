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
    public int id_ignorar;

    public GameObject prefab_personaje;
    public GameObject padre;

    public GameObject personaje_id_ignorar;

    private List<GameObject> lista_personajes;

    private float counter_send=0;
    public float max_counter;

    void Start()
    {
        ip = new LocalIP().SetLocalIP();

        server = new Server(ip,port,max_clients,0,timeout);

        lista_personajes = new List<GameObject>();

        lista_personajes.Add(personaje_id_ignorar);

        server.AddTrigger("Connect", delegate (ENet.Event net_event) {

            Debug.Log("Cliente Conectado");

            int index = server.AddPeer(net_event);

            if(index!=id_ignorar)
            {

                GameObject go = (GameObject)Instantiate(prefab_personaje, punto_creacion.transform.position, Quaternion.identity);
                go.transform.SetParent(padre.transform);
                go.GetComponent<Move>().SetID(index);

                lista_personajes.Add(go);

                var lista_para_enviar = new List<Data_Personajes_Inicializador>();

                foreach(var player in lista_personajes)
                {
                    var script = player.GetComponent<Move>();
                    var datos = new Data_Personajes_Inicializador(player.transform.position,script.GetID());

                    lista_para_enviar.Add(datos);
                }

                server.Send("Inicializador", new Dictionary<string, dynamic>()
                    { { "id_inicial" , index} ,{"lista_usuarios",lista_para_enviar}
                }, net_event.Peer);

                server.SendToAllBut("Nuevo_Usuario", new Dictionary<string, dynamic>()
                    {{"nuevo",lista_para_enviar[index]}
                }, net_event.Peer);

                
            }
        });

        server.AddTrigger("movimiento", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            int id = int.Parse(data.value["id"].ToString());
            string tecla = data.value["tecla"].ToString();
            string orientacion = data.value["orientacion"].ToString();

            var obj = lista_personajes[id].GetComponent<Move>();

            obj.movimiento_cambio(tecla, orientacion);

            server.SendToAllBut("movimiento", net_event.Packet, net_event.Peer, false);

        });

        server.AddTrigger("salto", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            int id = int.Parse(data.value["id"].ToString());
            string tecla = data.value["tecla"].ToString();

            var obj = lista_personajes[id].GetComponent<Move>();

            if(id!=id_ignorar)
            {
                obj.salto_improvisado(); 
            }

            server.SendToAllBut("salto", net_event.Packet, net_event.Peer, false);

        });

  
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        server.update();

        counter_send = counter_send + dt;

        if(counter_send>max_counter)
        {
            var lista = new List<data_alterable>();

            foreach(var personaje in lista_personajes)
            {
                lista.Add(new data_alterable(personaje.transform.position));
            }


            server.SendToAll("Ajustar_posicion", lista);

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
