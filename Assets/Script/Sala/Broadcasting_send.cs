using Assets.Libs.Esharknet.Broadcast;
using Assets.Libs.Esharknet.IP;
using Assets.Libs.Esharknet.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcasting_send : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public ushort port_send;
    private Broadcast_send broadcast;
    public GameObject server_manager;
    public int timedelay;

    private Server_script server_script;
    private Data_broadcast server_details;


    void Start()
    {
        string ip = new LocalIP().SetLocalIP();

        server_script = server_manager.GetComponent<Server_script>();

        server_details = new Data_broadcast(server_script.ip, server_script.port, server_script.server.GetListClientsCount(), server_script.max_clients, "room1");


        broadcast = new Broadcast_send(ip,port,port_send, timedelay, server_details);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnDestroy()
    {
        broadcast.Destroy();
    }

    public void actualizar(int players)
    {
        server_details.players = players;
    }


}
