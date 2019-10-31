using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Server_script : Convert_vector
{
    public Broadcasting_send broadcasting;

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

    private float counter_send;
    public float max_counter;

    public creacion creador_enemigos;
    public Text texto;

    public GameObject water;
    private float nivel_agua;

    private float counter_dano_agua;
    public float max_counter_dano_agua;

    private float counter_envio_total;
    public float max_envio_total;

    private int id_personajes=1;

    public float counter_envio_bolsa_max;

    public inventario_coleccionables script_inventario_coleccionable;

    void Start()
    {

        ip = new LocalIP().SetLocalIP();

        server = new Server(ip, port, max_clients, 3, timeout);

        lista_personajes = new List<GameObject>();

        lista_personajes.Add(player_inicial_servidor);
        player_inicial_servidor_script = player_inicial_servidor.GetComponent<Move_server>();

        server.AddTrigger("Connect", delegate (ENet.Event net_event)
        {

            int index_peer = server.AddPeer(net_event);

            var lista_para_enviar = new List<data_inicial>();

            GameObject go = (GameObject)Instantiate(prefab_personaje, punto_creacion.transform.position, Quaternion.identity);
            go.transform.SetParent(padre.transform);
            go.GetComponent<Move>().SetID(id_personajes);
            go.GetComponent<get_id>().id = id_personajes;

            //bolsa guardado
            var script_bolsa = go.AddComponent<bolsa_inventario>();
            script_bolsa.max_counter = counter_envio_bolsa_max;
            script_bolsa.server = this;
            script_bolsa.index_peer = index_peer;


            lista_personajes.Add(go);

            var data_nueva = new data_inicial(id_personajes, vec_to_obj( punto_creacion.transform.position));

            int i = 0;

            foreach (var player in lista_personajes)
            {
                if (i == 0)
                {
                    var script = player.GetComponent<Move_server>();
                    var datos = new data_inicial(script.GetID(), vec_to_obj(player.transform.position));

                    lista_para_enviar.Add(datos);
                }
                else
                {
                    var script = player.GetComponent<Move>();
                    var datos = new data_inicial(script.GetID(), vec_to_obj(player.transform.position));

                    lista_para_enviar.Add(datos);
                }

                i++;

            }

            broadcasting.actualizar(server.GetListClientsCount());

            server.Send("Inicializador", new Listado_Usuarios(id_personajes, lista_para_enviar), net_event.Peer);

            server.SendToAllBut("Nuevo_Usuario", data_nueva, net_event.Peer);

            id_personajes++;

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

            var obj = (data_tecla)data.value;

            var buscado = buscar_usuario(obj.id);

            if(buscado!=null)
            {
                var gameobj = buscado.GetComponent<Move>();

                gameobj.movimiento_cambio(obj.tecla, obj.orientacion);

                server.SendToAllBut("movimiento", net_event.Packet, net_event.Peer, false);
            }

        });

        server.AddTrigger("enviar_posicion", delegate (ENet.Event net_event) {
            var data = server.JSONDecode(net_event.Packet);

            var obj = (data_por_segundos)data.value;

            var buscado = buscar_usuario(obj.id);

            if (buscado != null)
            {
                var script = buscado.GetComponent<Move>();

                script.normalizado(obj_to_vec(obj.posicion), Quaternion.Euler(obj_to_vec(obj.radio)));
                script.set_arma_actual(obj.arma);

                server.SendToAllBut("enviar_posicion", net_event.Packet, net_event.Peer, false);
            }

                
        });

        server.AddTrigger("chat", delegate (ENet.Event net_event)
        {
            var data = server.JSONDecode(net_event.Packet);

            var obj = (data_chat)data.value;

            var buscado = buscar_usuario(obj.id);

            if (buscado != null)
            {
                var gameobj = buscado.GetComponent<Move>();

                gameobj.texto.text = obj.texto;
            }

            
        });

        /*server.AddTrigger("personaje_muerto", delegate (ENet.Event net_event)
        {
            var data = server.JSONDecode(net_event.Packet);
            var obj = (data_botar_objetos)data.value;

            var buscado = buscar_usuario(obj.id);

            if (buscado != null)
            {

                buscado.GetComponent<personaje_volver_inicio>().volver_al_inicio();
                buscado.GetComponent<Move>().no_arma_funcion();

                script_inventario_coleccionable.crear_varios(obj.objetos,obj_to_vec(obj.posicion));



                //server.SendToAllBut("personaje_muerto", net_event.Packet, net_event.Peer, false);
            }
                
        });*/

        


        nivel_agua = transform.TransformPoint(water.transform.position).y;

    }

    // Update is called once per frame
    void Update()
    {

        server.update();

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
            server.SendToAll("enviar_posicion", new data_por_segundos(player_inicial_servidor_script.GetID(), vec_to_obj(player_inicial_servidor_script.transform.position), vec_to_obj(player_inicial_servidor.transform.rotation.eulerAngles), player_inicial_servidor_script.get_arma_actual()));

            counter_send = 0;
        }

        counter_envio_total = counter_envio_total + dt;

        if (counter_envio_total > max_envio_total)
        {
            var listado = new List<data_vidas>();
            foreach(var obj in lista_personajes)
            {
                var script = obj.GetComponent<acciones_compartidas>();
                int id;

                if(obj.tag== "Personaje Principal")
                {
                    id = obj.GetComponent<Move_server>().GetID();
                }
                else
                {
                    id = obj.GetComponent<Move>().GetID();
                }

                listado.Add(new data_vidas(id,script.vidas));
            }


            server.SendToAll("enviar_vidas", listado);
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

    public GameObject buscar_usuario(int id)
    {
        foreach (var go in lista_personajes)
        {
            var script = go.GetComponent<get_id>();
            if (script.id == id)
            {
                return go;
            }
        }

        return null;
    }

    public int buscar_usuario_index(int id)
    {
        int i = 0;

        foreach (var go in lista_personajes)
        {
            var script = go.GetComponent<get_id>();
            if (script.id == id)
            {
                return i;
            }

            i++;
        }

        return -1;
    }


    void OnDestroy()
    {
        server.Destroy();
        Debug.LogWarning("Destroy gameobject");
        server = null;
    }
}
