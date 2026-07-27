using Lidgren.Network;
using OmegaRace.Data_Queues;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace OmegaRace
{
    public class NetworkManager
    {
        private NetPeer network;
        private bool isServer;

        private NetworkManager() { }

        // Initialize the server
        public static NetworkManager InitServer(int serverPort)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("OmegaRace")
            {
                Port = serverPort,
                AcceptIncomingConnections = true,
                MaximumConnections = 2,

                SimulatedLoss = 0.2f, // 20% packet loss simulation
                SimulatedMinimumLatency = 0.1f, // 100ms latency simulation
            };
            //config.SimulatedLoss = 0.2f;
            //config.SimulatedMinimumLatency = 0.1f;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetworkManager manager = new NetworkManager();
            manager.network = new NetServer(config);
            manager.isServer = true;
            manager.network.Start();
            return manager;
        }

        // Join the server as a client
        public static NetworkManager JoinServer(string host, int serverPort)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("OmegaRace");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);

            NetworkManager manager = new NetworkManager();
            manager.network = new NetClient(config);
            manager.isServer = false;
            manager.network.Start();

            IPEndPoint endPoint = NetUtility.Resolve(host, serverPort);
            manager.network.Connect(endPoint);
            //((NetClient)manager.network).Connect(endPoint);
            return manager;
        }

        public void Process(MessageQueueManager msgQueueMgr)
        {
            NetIncomingMessage msg;
            while ((msg = network.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        if (isServer)
                        {
                            NetOutgoingMessage response = network.CreateMessage();
                            response.Write("OmegaRace Server");
                            network.SendDiscoveryResponse(response, msg.SenderEndPoint);
                            Debug.WriteLine("Discovery request handled.");
                        }
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        string responseStr = msg.ReadString();
                        Debug.WriteLine($"Received discovery response: {responseStr}");
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte(); // test
                        Debug.WriteLine($"Status changed: {status}");
                        SendMessage(ServerTimeRequest.Create(), 31);
                        break;

                    case NetIncomingMessageType.Data:
                        ProcessDataMessage(msg, msgQueueMgr);
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        msg.SenderConnection.Approve();
                        Debug.WriteLine("Connection approved.");
                        break;

                    default:
                        Debug.WriteLine($"Unhandled message type: {msg.MessageType}");
                        break;
                }

                network.Recycle(msg);
            }
        }

        private void ProcessDataMessage(NetIncomingMessage msg, MessageQueueManager msgQueueMgr)
        {
            byte[] data = msg.ReadBytes(msg.LengthBytes);
            MemoryStream ms = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(ms);
            {
                DataMessage.MessageType type = DataMessage.Deserialize(ref reader).msgType;
                ms.Position = 0;

                //reader = new BinaryReader(new MemoryStream(data));
                switch (type)
                {
                    case DataMessage.MessageType.Movement:
                        msgQueueMgr.AddToInputQueue(MovementMessage.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.Fire:
                        msgQueueMgr.AddToInputQueue(FireMessage.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.Mine:
                        msgQueueMgr.AddToInputQueue(MineMessage.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.PosRot:
                        msgQueueMgr.AddToInputQueue(PosRotMessage.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.Collision:
                        msgQueueMgr.AddToInputQueue(CollisionMessage.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.ServerTimeRequest:
                        msgQueueMgr.AddToInputQueue(ServerTimeRequest.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.ServerTimeResponse:
                        msgQueueMgr.AddToInputQueue(ServerTimeResponse.Deserialize(ref reader));
                        break;
                    case DataMessage.MessageType.Prediction:
                        msgQueueMgr.AddToInputQueue(PredictionMessage.Deserialize(ref reader));
                        break;
                    default:
                        Debug.Assert(false, "Unhandled or uninitialized message type.");
                        break;
                }
            }
        }

        public void SendMessage(DataMessage msg, int msgChannel)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(ms);
                //msg.Serialize(ref writer); // might need individual
                switch (msg.msgType)
                {
                    case DataMessage.MessageType.Fire:
                        ((FireMessage)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.Mine:
                        ((MineMessage)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.Movement:
                        ((MovementMessage)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.PosRot:
                        ((PosRotMessage)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.Collision:
                        ((CollisionMessage)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.ServerTimeRequest:
                        ((ServerTimeRequest)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.ServerTimeResponse:
                        ((ServerTimeResponse)msg).Serialize(ref writer);
                        break;
                    case DataMessage.MessageType.Prediction:
                        ((PredictionMessage)msg).Serialize(ref writer);
                        break;
                    default:
                        Debug.Assert(false, "Uninitialized message type");
                        break;
                }

                NetOutgoingMessage netMsg = network.CreateMessage();
                netMsg.Write(ms.ToArray());

                if (network.ConnectionsCount > 0)
                {
                    network.SendMessage(netMsg, network.Connections[0], NetDeliveryMethod.ReliableOrdered, msgChannel);
                }

                // Recycle
                msg.Recycle();
            }
        }
    }
}