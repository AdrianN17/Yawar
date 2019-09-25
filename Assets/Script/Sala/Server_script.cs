using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
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

    public Text fps_text;

    void Start()
    {
        //Application.targetFrameRate = 60;

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

        fps_text.text = "FPS : " + (int)(1.0f / Time.smoothDeltaTime); 
    }

    void OnDestroy()
    {
        server.Destroy();
        Debug.LogWarning("Destroy gameobject");
        server = null;
    }
}
