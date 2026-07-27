using System;
using System.Diagnostics;
using Lidgren.Network;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace LidgrenBasicUsage
{
    class Program
    {
        //**********   Window auto adjustment stuff. Just ignore ***********
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static public void AdjustWindow()
        {
            Rect rc = System.Windows.SystemParameters.WorkArea;

            IntPtr hwndFound = GetConsoleWindow(); // Console window adjustment
            MoveWindow(hwndFound, 0, 0, (int)rc.Width/2, (int)rc.Height/2, true);
        }
        //*******************************************************************

        static NetClient client;
        static int serverPort = 14240;

        static int msgcounter = 0; // only for class demo: using this to highlight the msgs processed

        static void InitClient()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();

            // ****** pick one ***********************

            //* Version #1: local subnet broadcast
            client.DiscoverLocalPeers(serverPort);
            //*/

            /* Verison #2A: specific host:port request
            client.DiscoverKnownPeer("localhost", serverPort);
            //*/

            /* Versio #2B: Equivalent to above, but with explicit call to DNS to resolve host to IP address.
            IPEndPoint ep = NetUtility.Resolve("localhost", serverPort);
            client.DiscoverKnownPeer(ep);
            //*/

            /* Version #3A: barging in unannounced... (bypassing discoveryRequest/Response)
            client.Connect("localhost", serverPort);
            //*/

            /* Version 3B
            IPEndPoint ep = NetUtility.Resolve("localhost", serverPort);
            client.Connect(ep);
            //*/
        }

        static void ProcessIncoming()
        {
            NetIncomingMessage im;

            while ((im = client.ReadMessage()) != null)
            {
                Console.Write("Lidgren msg#" + msgcounter + ": ");

                // First, the Lidgren-type of message we received
                switch (im.MessageType)
                {
                    //**********************************
                    // A server replied to out discovery request
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + im.SenderEndPoint + " name: " + im.ReadString());
                        client.Connect(im.SenderEndPoint);

                        Console.WriteLine("Enter to see status changes:");
                        Console.ReadLine();
                        break;

                    // Connection status to serverhas changed
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Console.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());
                        break;

                    // A client is sending application-related data
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Data message received");
                        //* simnple string data
                        string msg = im.ReadString();
                        Console.WriteLine(msg);
                        //*/

                        /* Complex data
                        ProcessMessage(im);
                        //*/
                        break;

                    //*****************************************

                    // These are other Lidgren status messages that we likely shouldn't have to deal with
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.UnconnectedData:
                        Console.WriteLine("Status Message:" + im.MessageType + " from [" + im.SenderEndPoint + "]: " + im.ReadString());
                        break;
                }

                client.Recycle(im);
                msgcounter++;
            }
        }

        static void SendMessage(string s)
        {
            Console.WriteLine("Sending string '" + s + "'");

            NetOutgoingMessage om = client.CreateMessage(s);
            client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
        }

        static void SendMessage( SuperImportantData data)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            data.Serialize(ref writer);

            NetOutgoingMessage om = client.CreateMessage();
            om.Write(stream.ToArray());


            Console.WriteLine("Sending structured data");
            client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
        }

        static void ProcessMessage(NetIncomingMessage im)
        {
            byte[] bytes = im.ReadBytes(im.LengthBytes);
            BinaryReader reader = new BinaryReader(new MemoryStream(bytes));

            SuperImportantData data = new SuperImportantData();
            data.Deserialize(ref reader);

            Console.WriteLine("SuperImportantData: " + data.thing1 + " & " + data.thing2 + " & [" + data.thing3 + "]");

            // Processing messages as commands
            //data.execute();
        }

        static void Main(string[] args)
        {
            AdjustWindow();

            SuperImportantData Data = new SuperImportantData();
            Data.thing1 = 5.5f;
            Data.thing2 = 2;

            InitClient();

            Console.Write("Connection Requested: Press Enter to see the reply: ");
            string s = Console.ReadLine();

            while (true)    // This will be the game loop's update eventually
            {
                ProcessIncoming();

                Console.Write("Message to send: ");
                s = Console.ReadLine();

                SendMessage(s);   // Simple string sent

                /* Complex data sent
                Data.thing3 = s;
                SendMessage(Data);  // Complex data sent
                //*/

                Console.Write("Message sent. Enter to see the reply: ");
                s = Console.ReadLine();
            }
        }
    }
}
