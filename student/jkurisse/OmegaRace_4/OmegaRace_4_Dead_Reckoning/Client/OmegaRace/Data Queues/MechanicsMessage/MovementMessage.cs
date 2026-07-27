using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class MovementMessage : DataMessage
    {
        public int horzInput;
        public int vertInput;

        public MovementMessage()
        {
            msgType = MessageType.Movement;
        }
        public MovementMessage(PlayerID id, int horz, int vert)
        : this()
        {
            player = id;
            horzInput = horz;
            vertInput = vert;
        }
        public static MovementMessage Create(PlayerID player, int horz, int vert)
        {
            MovementMessage msg = MessagePool<MovementMessage>.Get();
            msg.msgType = MessageType.Movement;
            msg.deliveryMethod = NetDeliveryMethod.ReliableSequenced;
            msg.destination = DestinationType.Network;
            msg.channel = 10;
            msg.player = player;
            msg.horzInput = horz;
            msg.vertInput = vert;
            return msg;
        }

        public override void Recycle()
        {
            MessagePool<MovementMessage>.Recycle(this);
        }
        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer); // writes msgType and player
            writer.Write(horzInput);
            writer.Write(vertInput);
        }

        public static new MovementMessage Deserialize(ref BinaryReader reader)
        {
            MovementMessage msg = new MovementMessage();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            msg.horzInput = reader.ReadInt32();
            msg.vertInput = reader.ReadInt32();
            return msg;
        }
        public override void Execute()
        {
            PlayerManager plMgr = GameSceneCollection.ScenePlay.PlayerMgr;

            if (player == PlayerID.Player1)
            {
                plMgr.P1Data.ship.Rotate(horzInput);
                plMgr.P1Data.ship.Move(vertInput);
            }
            else
            {
                plMgr.P2Data.ship.Rotate(horzInput);
                plMgr.P2Data.ship.Move(vertInput);
            }

            // Environmental friendly
            Recycle();
        }

        public override void PrintMe()
        {
            Debug.WriteLine($"[MovementMessage] Player: {player}, Horz: {horzInput}, Vert: {vertInput}");
        }
    }
}
