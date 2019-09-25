using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Libs.Esharknet.Broadcast
{
    public class Broadcast_receive
    {
        public UdpClient udpClient;
        private IPEndPoint ip_point;
        private Thread thread;
        private Dictionary<string, dynamic> servers_list;

        private bool loop = true;

        public Broadcast_receive(string ip_address, ushort port_send, int timedelay)
        {
            udpClient = new UdpClient();
            ip_point = new IPEndPoint(IPAddress.Parse(ip_address), port_send);
            udpClient.Client.Bind(ip_point);

            servers_list = new Dictionary<string, dynamic>();

            thread = new Thread(delegate ()
            {
                while (loop)
                {
                    try
                    {

                        var bytes = udpClient.Receive(ref ip_point);

                        if(bytes.Length>0)
                        {
                            var json_data = Encoding.ASCII.GetString(bytes);
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json_data);

                            Debug.Log("Broadcast receive : " + json_data);
                        }

                        //var data = dictionary["Broadcast"];
                        //validate(data["ip"], data);
                    }
                    catch (SocketException ex) // or whatever the exception is that you're getting
                    {
                        Debug.LogWarning(ex.Message);
                    }


                    Thread.Sleep(timedelay);
                }
            });

            thread.Start();

        }

        private void validate(string ip,dynamic data)
        {
            if(servers_list.ContainsKey(ip))
            {
                var dictionary = data;
                servers_list[ip]["players"] = data["players"];
            }
            else
            {
                servers_list.Add(ip,data);
            }
        }

        Dictionary<string, dynamic> GetListObtained()
        {
            return this.servers_list;
        }

        public void Destroy()
        {
            Debug.LogWarning("Broadcast receive finish");

            loop = false;

            udpClient.Close();
            servers_list.Clear();
 
        }
    }
}
