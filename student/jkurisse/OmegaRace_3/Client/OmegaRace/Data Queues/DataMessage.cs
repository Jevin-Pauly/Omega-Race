using Lidgren.Network;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class DataMessage
    {
        public enum MessageType
        {
            Movement,
            Fire,
            Mine,
            Collision,
            PosRot,
            ServerTimeRequest,
            ServerTimeResponse,
            Uninitialized
        }

        public enum PlayerID
        {
            Player1,
            Player2
        }

        public enum DestinationType
        {
            Network,
            Local,
            Both
        }

        public MessageType msgType = MessageType.Uninitialized;
        public PlayerID player = PlayerID.Player1;
        public DestinationType destination = DestinationType.Both;
        public NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableOrdered;
        public int channel = 0;

        public virtual void Serialize(ref BinaryWriter writer)
        {
            writer.Write((int)msgType);
            writer.Write((int)player);
            writer.Write((int)destination);
            writer.Write((int)deliveryMethod);
            writer.Write(channel);
        }

        public static DataMessage Deserialize(ref BinaryReader reader)
        {
            DataMessage output = new DataMessage();
            output.msgType = (MessageType)reader.ReadInt32();
            output.player = (PlayerID)reader.ReadInt32();
            output.destination = (DestinationType)reader.ReadInt32();
            output.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            output.channel = reader.ReadInt32();
            return output;
        }

        public virtual void Execute()
        {
            Debug.Assert(false, "Base Execute should not be called directly.");
        }
        public virtual void Recycle()
        {
            Debug.Assert(false, "Recycle should be overridden");
        }
        public virtual void PrintMe()
        {
            Debug.WriteLine($"[DataMessage] Type: {msgType}, Player: {player}");
        }

    }
}

