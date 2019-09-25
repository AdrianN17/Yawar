using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client_script : MonoBehaviour
{
    // Start is called before the first frame update
    public ushort port;
    public int timeout;
    public Client client;
    public string ip;

    void Start()
    {

        ip = PlayerPrefs.GetString("ip_address");
        port = (ushort)PlayerPrefs.GetInt("port");
        
        PlayerPrefs.DeleteKey("ip_address");
        PlayerPrefs.DeleteKey("port");

        Debug.LogWarning("ip " + ip + " port " + port);

        client = new Client(ip,port,0,timeout);

        /*client.AddTrigger("Hola", delegate (ENet.Event net_event) {
            var obj = client.JSONDecode(net_event.Packet);
            Debug.Log(obj.value);
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        client.update();
    }

    void OnDestroy()
    {
        client.Destroy();
        Debug.LogWarning("Destroy gameobject");
        client = null;
    }
}
