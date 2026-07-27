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
        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
        }
        public static new FireMessage Deserialize(ref BinaryReader reader)
        {
            FireMessage msg = new FireMessage();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
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
        }

        public override void PrintMe()
        {
            Debug.WriteLine($"[FireMessage] Player: {player}");
        }
    }
}
