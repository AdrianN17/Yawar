using Assets.Libs.Esharknet.Broadcast;
using Assets.Libs.Esharknet.IP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcasting_send : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public ushort port_send;
    private Broadcast_send broadcast;
    //public GameObject server_manager;
    public int timedelay;

    //private Server_script server_script;

    void Start()
    {
        string ip = new LocalIP().SetLocalIP();

        //server_script = server_manager.GetComponent<Server_script>();



        var server_details = new Dictionary<string, dynamic>();
        server_details.Add("ip", "server_script.ip");
        server_details.Add("port", "server_script.port");
        server_details.Add("players", "server_script.server.GetListClientsCount()");
        server_details.Add("max_players", "server_script.max_clients");
        server_details.Add("name_server", "room1");

        var data = new Dictionary<string, dynamic>() {
            { "broadcast",server_details }
        };

        broadcast = new Broadcast_send(ip,port,port_send, timedelay, data);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnDestroy()
    {
        broadcast.Destroy();
    }


}
