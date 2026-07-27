using Lidgren.Network;
using System.IO;

namespace OmegaRace.Data_Queues
{
    public class ServerTimeRequest : DataMessage
    {
        float clientTime;

        public ServerTimeRequest()
        {
            this.msgType = MessageType.ServerTimeRequest;
            this.deliveryMethod = NetDeliveryMethod.ReliableUnordered;
            this.clientTime = TimeManager.GetCurrentTime();
        }

        public static ServerTimeRequest Create()
        {
            ServerTimeRequest msg = MessagePool<ServerTimeRequest>.Get();
            msg.msgType = MessageType.ServerTimeRequest;
            msg.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            msg.destination = DestinationType.Network;
            msg.clientTime = TimeManager.GetCurrentTime();
            return msg;
        }

        public static new ServerTimeRequest Deserialize(ref BinaryReader reader)
        {
            ServerTimeRequest msg = MessagePool<ServerTimeRequest>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            msg.clientTime = reader.ReadSingle();
            return msg;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(clientTime);
        }

        public override void Execute()
        {
            // Executed only by server
            ServerTimeResponse response = ServerTimeResponse.Create(clientTime, TimeManager.GetCurrentTime());
            GameSceneCollection.ScenePlay.MsgQueueMgr.AddToOutputQueue(response);
        }

        public override void Recycle()
        {
            MessagePool<ServerTimeRequest>.Recycle(this);
        }
    }
}
