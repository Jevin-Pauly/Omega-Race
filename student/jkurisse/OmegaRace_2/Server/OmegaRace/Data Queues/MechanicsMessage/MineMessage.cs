using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class MineMessage : DataMessage
    {
        public MineMessage()
        {
            msgType = MessageType.Mine;
        }
        public MineMessage(PlayerID id)
        : this()
        {
            player = id;
        }
        public static MineMessage Create(PlayerID player)
        {
            MineMessage msg = MessagePool<MineMessage>.Get();
            msg.msgType = MessageType.Mine;
            msg.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            msg.destination = DestinationType.Both;
            msg.channel = 12;
            msg.player = player;
            return msg;
        }
        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
        }
        public static new MineMessage Deserialize(ref BinaryReader reader)
        {
            MineMessage msg = MessagePool<MineMessage>.Get();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            msg.destination = (DestinationType)reader.ReadInt32();
            msg.deliveryMethod = (NetDeliveryMethod)reader.ReadInt32();
            msg.channel = reader.ReadInt32();
            return msg;
        }
        public override void Execute()
        {
            PlayerManager plMgr = GameSceneCollection.ScenePlay.PlayerMgr;

            if (player == PlayerID.Player1)
                plMgr.P1Data.LayMine();
            else
                plMgr.P2Data.LayMine();

            Recycle();
        }
        public override void Recycle()
        {
            MessagePool<MineMessage>.Recycle(this);
        }
        public override void PrintMe()
        {
            Debug.WriteLine($"[MineMessage] Player: {player}");
        }
    }
}
