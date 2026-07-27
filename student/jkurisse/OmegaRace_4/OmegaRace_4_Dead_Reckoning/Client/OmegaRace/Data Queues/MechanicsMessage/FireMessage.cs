using Lidgren.Network;
using OmegaRace.Data_Queues;
using System;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    [Serializable]
    public class FireMessage : DataMessage
    {
        public FireMessage()
        {
            msgType = MessageType.Fire;
        }
        public FireMessage(PlayerID id)
        : this()
        {
            player = id;
        }
        public static FireMessage Create(PlayerID player)
        {
            FireMessage msg = MessagePool<FireMessage>.Get();
            msg.msgType = MessageType.Fire;
            msg.deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            msg.destination = DestinationType.Both;
            msg.channel = 11;
            msg.player = player;
            return msg;
        }
        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
        }
        public static new FireMessage Deserialize(ref BinaryReader reader)
        {
            FireMessage msg = MessagePool<FireMessage>.Get();
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
            //var plMgr = PlayerManager.Instance;

            if (player == PlayerID.Player1)
                plMgr.P1Data.FireMissile();
            else
                plMgr.P2Data.FireMissile();

            Recycle();
        }
        public override void Recycle()
        {
            MessagePool<FireMessage>.Recycle(this);
        }
        public override void PrintMe()
        {
            Debug.WriteLine($"[FireMessage] Player: {player}");
        }
    }
}
