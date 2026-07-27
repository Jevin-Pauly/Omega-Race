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
            MoveWindow(hwndFound, (int)rc.Width / 2, 0, (int)rc.Width / 2, (int)rc.Height / 2, true);
        }
        //*******************************************************************

        static NetServer server;  
        static int ServerPort = 14240;

        static int msgcounter = 0; // only for class demo: using this to highlight the msgs processed
        static void InitServer()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = ServerPort;

            server = new NetServer(config);
            server.Start();
        }
        static void ProcessIncoming()
        {
            NetIncomingMessage im;

            while ((im = server.ReadMessage()) != null)
            {
                Console.Write("Lidgren msg#" + msgcounter + ": ");

                // First, the Lidgren-type of message we received
                switch (im.MessageType)
                {
                    //**********************************
                    // A client is enquiring about a possible connection
                    case NetIncomingMessageType.DiscoveryRequest:
                        Console.WriteLine("Answering Discovery Request from " + im.SenderEndPoint);
                        NetOutgoingMessage om = server.CreateMessage();
                        om.Write("Welcome to this cool server");
                        server.SendDiscoveryResponse(om, im.SenderEndPoint);
                        break;

                    // A client's connection status changed
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Console.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());
                        break;

                    // A client is sending application-related data
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Data message received");
                        //* String demo
                        string msg = im.ReadString();
                        Console.WriteLine(msg);
                        ProcessMessage(msg, im.SenderConnection);
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

                server.Recycle(im);
                msgcounter++;
            }
        }

        static void ProcessMessage(string s, NetConnection clientConn )
        {
            Console.WriteLine("Sending string '" + s + "'");

            NetOutgoingMessage om = server.CreateMessage("Echoing back the string: [" + s + "]");
            server.SendMessage(om, clientConn, NetDeliveryMethod.ReliableOrdered);
        }

        static void ProcessMessage(NetIncomingMessage im)
        {
            SuperImportantData data = new SuperImportantData();

            byte[] bytes = im.ReadBytes(im.LengthBytes);
            BinaryReader reader = new BinaryReader(new MemoryStream(bytes));

            data.Deserialize(ref reader);
            Console.WriteLine("SuperImportantData: " + data.thing1 + " & " + data.thing2 + " & [" + data.thing3 + "]");

            // Processing messages as commands
            //data.execute();

            Console.WriteLine("Sending structure data");
            SuperImportantData ReturnData = new SuperImportantData();
            ReturnData.thing1 = 3.141592653f;
            ReturnData.thing2 = 6;
            ReturnData.thing3 = "Apple";

            NetOutgoingMessage om = server.CreateMessage();
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            ReturnData.Serialize(ref writer);
            om.Write(stream.ToArray());

            server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered);

        }

        static void Main(string[] args)
        {
            AdjustWindow();

            InitServer();

            while (true)    // This will be the game loop's update eventually
            {
                ProcessIncoming();
            }
        }           
    }
}
