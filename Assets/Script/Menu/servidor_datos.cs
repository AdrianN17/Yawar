using Assets.Libs.Esharknet.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servidor_datos : MonoBehaviour
{
    // Start is called before the first frame update
    public string ip;
    public int port;
    public int max_players;
    public int players;
    public string name_server;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void set_values(Data_broadcast data)
    {
        this.ip = data.ip;
        this.port = data.port;
        this.max_players = data.max_players;
        this.players = data.players;
        this.name_server = data.name_server;
    }
}
