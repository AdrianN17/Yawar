using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server_script : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public int max_clients;
    public int timeout;
    public Server server;
    public string ip;

    void Start()
    {
        ip = new LocalIP().SetLocalIP();

        server = new Server(ip,port,max_clients,0,timeout);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            server.SendToAll("Hola", "que tal");
        }*/

        server.update();
    }

    void OnDestroy()
    {
        server.Destroy();
        Debug.LogWarning("Destroy gameobject");
        server = null;
    }
}
