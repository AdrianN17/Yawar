using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client_script : Convert_vector
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

    public inventario_coleccionables inventario_cliente;

    public coleccionable coleccionable_script;
    public Text ping_text;

    public Control_dia_noche control_periodo;
    public MenuGame menu_game;

    public Text texto;
    public GameObject texto_panel;
    public int max_length_texto;

    public Slider mivida;


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

            var obj = (Listado_Usuarios)data.value;

            int index = obj.id;
            var personajes = obj.lista;

            control_periodo.set_periodo(obj.periodo);


            foreach (var personaje in personajes)
            {
                GameObject go = (GameObject)Instantiate(prefab_personaje, obj_to_vec(personaje.posicion), Quaternion.identity);
                lista_personajes.Add(go);

                go.transform.SetParent(padre.transform);

                var go_script = go.GetComponent<Move>();
                go_script.SetID(personaje.id);
                go.GetComponent<get_id>().id = personaje.id;

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
                    script_compartido.set_slider(mivida);

                    go.tag = "Personaje Principal";

                    

                }
            }

            client.Send("Pedir_enemigos", null);
        });

        client.AddTrigger("Inicializador_enemigos", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var enemigos = (List<data_enemigo_por_segundos>)data.value;

            script_crearenemigo.crear_enemigo_creacion_player(enemigos);

            start_send = true;

            client.Send("Pedir_coleccionables", null);
        });

        client.AddTrigger("Inicializador_coleccionables", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var coleccionables_data = (List<data_colecionable_con_id>)data.value;

            coleccionable_script.crear_varios_coleccionalbes(coleccionables_data);
        });

        client.AddTrigger("Nuevo_Usuario", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var personaje = (data_inicial)data.value;

            GameObject go = (GameObject)Instantiate(prefab_personaje, obj_to_vec(personaje.posicion), Quaternion.identity);
            lista_personajes.Add(go);

            go.transform.SetParent(padre.transform);

            var go_script = go.GetComponent<Move>();
            go_script.SetID(personaje.id);

        });

        client.AddTrigger("movimiento", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var obj = (data_tecla)data.value;
            var buscado = buscar_usuario(obj.id);

            if (buscado != null)
            {
                var gameobj = buscado.GetComponent<Move>();

                gameobj.movimiento_cambio(obj.tecla, obj.orientacion);
            }

        });

        client.AddTrigger("enviar_posicion", delegate (ENet.Event net_event) {
            var data = client.JSONDecode(net_event.Packet);

            var obj_list = (List<data_por_segundos>)data.value;


            foreach(var obj in obj_list)
            { 
                var buscado = buscar_usuario(obj.id);

                if (buscado != null)
                {
                    var gameobj = buscado.GetComponent<Move>();

                    gameobj.normalizado(obj_to_vec(obj.posicion), Quaternion.Euler(obj_to_vec(obj.radio)));

                    gameobj.set_arma_actual(obj.arma);
                }
            }

        });

        client.AddTrigger("Actualizar_enemigos", delegate (ENet.Event net_event)
        {

            var data = client.JSONDecode(net_event.Packet);

            var obj = (List<data_enemigo_por_segundos>)data.value;

            script_crearenemigo.actualizar_enemigos(obj);


        });

        client.AddTrigger("chat", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);

            var obj = (data_chat)data.value;

            var buscado = buscar_usuario(obj.id);

            if(buscado!=null)
            {
                var gameobj = buscado.GetComponent<Move>();

                gameobj.texto_volatil.set_text(obj.texto);
            }
                
        });

        client.AddTrigger("personaje_muerto", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = (data_botar_objetos)data.value;

            var obj_buscado = buscar_usuario(obj.id);

            if (obj_buscado != null)
            {
                obj_buscado.GetComponent<personaje_volver_inicio>().volver_al_inicio();
                obj_buscado.GetComponent<Move>().no_arma_funcion();

                inventario_cliente.crear_varios(obj.objetos, obj_to_vec(obj.posicion));
                inventario_cliente.limpiar_principal();


            }
                

            ///falta mas
        });

        client.AddTrigger("Jugador_desconectado", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = (data_solo_id)data.value;

            var obj_buscado = buscar_usuario(obj.id);

            if(obj_buscado!=null)
            {
                Destroy(obj_buscado);

                lista_personajes.Remove(obj_buscado);
            }
        });

        client.AddTrigger("enviar_vidas", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = (List<data_vidas>)data.value;

            foreach(var gameobj in obj)
            {
                var buscado = buscar_usuario(gameobj.id);

                if(buscado!=null)
                {
                    var script1 = buscado.GetComponent<acciones_compartidas>();
                    script1.vidas = gameobj.vidas;
                    script1.slider_set_value(gameobj.vidas);
                    script1.barra_reduce_try(gameobj.vidas);

                    if (gameobj.vidas<1) 
                    {
                        var script =  buscado.GetComponent<Move>();
                        script1.generar_muerte_cliente_personaje(script.calcular_ahogo(), script.anim);
                    }
                }
            }
        });

        client.AddTrigger("inventario_actualizar", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = (List<data_coleccionable>)data.value;

            inventario_cliente.agregar_lista(obj);
            
        });

        client.AddTrigger("Actualizar_Coleccionables", delegate (ENet.Event net_event)
        {
            var data = client.JSONDecode(net_event.Packet);
            var obj = (List<data_colecionable_con_id>)data.value;
            coleccionable_script.actualizar(obj);

        });

        client.AddTrigger("Timeout", delegate (ENet.Event net_event)
        {
            menu_game.alerta_llamar("Tiempo de espera finalizado");
        });

        client.AddTrigger("Disconnect", delegate (ENet.Event net_event)
        {
            menu_game.alerta_llamar("Servidor Desconectado");
        });

    }

    // Update is called once per frame
    void Update()
    {
        client.update();

        ping_text.text = client.RountTripTimer()+" ms";

        float dt = Time.deltaTime;

        if (start_send)
        {
            /*counter_send = counter_send + dt;

            /if (counter_send > max_counter)
            {
                client.Send("enviar_posicion", new data_por_segundos(player_object_script.GetID(), vec_to_obj(player_object_script.transform.position), vec_to_obj(player_object_script.transform.rotation.eulerAngles),player_object_script.get_arma_actual()));

                counter_send = 0;
            }*/

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
                        if (texto.text.Length < max_length_texto)
                        {
                            texto.text += c;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {

                if (!string.IsNullOrWhiteSpace(texto.text))
                {
                    player_object_script.texto_volatil.set_text(texto.text);

                    client.Send("chat", new data_chat(player_object_script.GetID(), texto.text));

                    texto.text = "";
                }
                player_object_script.escribiendo = !player_object_script.escribiendo;
                texto_panel.SetActive(!texto_panel.activeSelf);

            }
        } 
    }

    public GameObject buscar_usuario(int id)
    {
        foreach(var go in lista_personajes)
        {
            var script = go.GetComponent<get_id>();
            if(script.id == id)
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


        client.Destroy();
        Debug.LogWarning("Destroy gameobject");
        client = null;
    }
}
