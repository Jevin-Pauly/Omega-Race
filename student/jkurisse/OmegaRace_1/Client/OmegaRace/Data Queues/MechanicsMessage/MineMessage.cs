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
        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
        }

        public static new MineMessage Deserialize(ref BinaryReader reader)
        {
            MineMessage msg = new MineMessage();
            msg.msgType = (MessageType)reader.ReadInt32();
            msg.player = (PlayerID)reader.ReadInt32();
            return msg;
        }
        public override void Execute()
        {
            PlayerManager plMgr = GameSceneCollection.ScenePlay.PlayerMgr;

            if (player == PlayerID.Player1)
                plMgr.P1Data.LayMine();
            else
                plMgr.P2Data.LayMine();
        }

        public override void PrintMe()
        {
            Debug.WriteLine($"[MineMessage] Player: {player}");
        }
    }
}
