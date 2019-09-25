using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Libs.Esharknet.Broadcast
{
    public class Broadcast_send
    {

        public UdpClient udpServer;
        public Thread thread;

        private Dictionary<string, dynamic> broadcast_data;

        private bool loop = true;

        public Broadcast_send(string ip_address,ushort port, ushort port_send, int timedelay, Dictionary<string,dynamic> broadcast_data)
        {
            udpServer = new UdpClient();

            udpServer.Client.Bind(new IPEndPoint(IPAddress.Parse(ip_address), port));

            this.broadcast_data = broadcast_data;

            thread = new Thread(delegate ()
            {
                while (loop)
                {
                    var data = new Dictionary<string, dynamic>()
                    {
                        {"broadcast", broadcast_data}
                    };

                    var json_data = JsonConvert.SerializeObject(data, Formatting.Indented);

                    Debug.Log("Broadcast Send : "+ json_data);

                    var bytes = Encoding.ASCII.GetBytes(json_data);

                    udpServer.Send(bytes, bytes.Length, "255.255.255.255", port_send);

                    Thread.Sleep(timedelay);

                }
            });

            thread.Start();

        }

        public void UpdateDictionary(string key, dynamic value)
        {
            if(broadcast_data.ContainsKey(key))
            {
                broadcast_data[key] = value;
            }
        }

        public void Destroy()
        {
            Debug.LogWarning("Broadcast send finish");

            loop = false;

            udpServer.Close();

        }
    }
}
