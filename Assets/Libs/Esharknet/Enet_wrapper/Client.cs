using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ENet;
using System.Runtime.InteropServices;
using System.Threading;

namespace Assets.Libs.Esharknet
{
    public class Client: BaseClass
    {
        private Host client;
        private Peer peer;
        private bool isAlive;
        private Thread clientThread;


        public Client(string ip_address, ushort port, int channel, int timeout)
        {
            isAlive = true;

            AllocCallback OnMemoryAllocate = (size) => {
                return Marshal.AllocHGlobal(size);
            };

            FreeCallback OnMemoryFree = (memory) => {
                Marshal.FreeHGlobal(memory);
            };

            NoMemoryCallback OnNoMemory = () => {
                throw new OutOfMemoryException();
            };

            Callbacks callbacks = new Callbacks(OnMemoryAllocate, OnMemoryFree, OnNoMemory);

            if (ENet.Library.Initialize(callbacks))
                Debug.LogWarning("ENet successfully initialized using a custom memory allocator");

            client = new Host();

            Address address = new Address();

            address.Port = port;
            address.SetHost(ip_address);

            client.Create();
            client.EnableCompression();

            peer = client.Connect(address);

            clientThread = new Thread(Update);
            clientThread.Start();


        }

        public void Update()
        {
            while (isAlive)
            {

                bool polled = false;

                ENet.Event netEvent;

                while (!polled)
                {
                    if (client.CheckEvents(out netEvent) <= 0)
                    {
                        if (client.Service(timeout, out netEvent) <= 0)
                            break;

                        polled = true;
                    }

                    UnityMainThreadDispatcher.Instance().Enqueue(() => switch_callbacks(netEvent));
                }

                Thread.Sleep(1000);
            }

            client.Flush();
        }

        public void Send(string event_name, dynamic data_value, bool Encode = true, int channel = 0)
        {
            ENet.Packet packet;

            if (Encode)
            {
                packet = JSONEncode(new Data(event_name, data_value));
            }
            else
            {
                packet = data_value;
            }

            peer.Send((byte)channel, ref packet);
        }

        void switch_callbacks(ENet.Event netEvent)
        {
            switch (netEvent.Type)
            {
                case ENet.EventType.None:
                    break;

                case ENet.EventType.Connect:
                    //Debug.Log("Client connected to server");
                    ExecuteTrigger("Connect", netEvent);

                    break;

                case ENet.EventType.Disconnect:
                    //Debug.Log("Client disconnected from server");
                    ExecuteTrigger("Disconnect", netEvent);

                    break;

                case ENet.EventType.Timeout:
                    //Debug.Log("Client connection timeout");
                    ExecuteTrigger("Timeout", netEvent);

                    break;

                case ENet.EventType.Receive:
                    //Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);

                    ExecuteTriggerBytes(netEvent);
                    netEvent.Packet.Dispose();
                    break;

            }
        }

        public void Destroy()
        {
            isAlive = false;

            peer.Disconnect(0);
            ENet.Library.Deinitialize();
            Debug.LogWarning("Client finish");
        }
    }
}
