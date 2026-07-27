using Lidgren.Network;
using System.Diagnostics;
using System.IO;

namespace OmegaRace.Data_Queues
{
    public class ServerTimeResponse : DataMessage
    {
        public float clientTime;
        public float serverTime;

        public ServerTimeResponse()
        {
            this.msgType = MessageType.ServerTimeResponse;
            this.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
        }

        public static ServerTimeResponse Create(float clientTime, float serverTime)
        {
            ServerTimeResponse msg = MessagePool<ServerTimeResponse>.Get();
            msg.msgType = MessageType.ServerTimeResponse;
            msg.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            msg.destination = DestinationType.Network;
            msg.clientTime = clientTime;
            msg.serverTime = serverTime;
            return msg;
        }

        public static new ServerTimeResponse Deserialize(ref BinaryReader reader)
        {
            ServerTimeResponse msg = MessagePool<ServerTimeResponse>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            msg.clientTime = reader.ReadSingle();
            msg.serverTime = reader.ReadSingle();
            return msg;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(clientTime);
            writer.Write(serverTime);
        }

        public override void Execute()
        {
            //float arrivalTime = TimeManager.GetCurrentTime();
            //float newTime = serverTime + ((arrivalTime - clientTime) / 2);
            //TimeManager.SetTime(newTime);
            Debug.Assert(false, "Execute not implemented on Server");
        }

        public override void Recycle()
        {
            MessagePool<ServerTimeResponse>.Recycle(this);
        }
    }
}
