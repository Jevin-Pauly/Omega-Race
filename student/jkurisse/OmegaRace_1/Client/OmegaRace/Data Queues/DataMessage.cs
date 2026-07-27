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
            Uninitialized
        }

        public enum PlayerID
        {
            Player1,
            Player2
        }

        //public int horzInput;
        //public int vertInput;
        public MessageType msgType = MessageType.Uninitialized;
        public PlayerID player = PlayerID.Player1;

        public virtual void Serialize(ref BinaryWriter writer)
        {
            writer.Write((int)msgType);
            writer.Write((int)player);
        }

        public static DataMessage Deserialize(ref BinaryReader reader)
        {
            DataMessage output = new DataMessage();
            output.msgType = (MessageType)reader.ReadInt32();
            output.player = (PlayerID)reader.ReadInt32();
            //output.horzInput = reader.ReadInt32();
            //output.vertInput = reader.ReadInt32();

            return output;
        }

        public virtual void Execute()
        {
            //// Locate player manager
            //PlayerManager plMgr = GameSceneCollection.ScenePlay.PlayerMgr;
            //
            //// Hard-coded for P2 for demo
            //plMgr.P2Data.ship.Move(vertInput);
            //plMgr.P2Data.ship.Rotate(horzInput);
            Debug.Assert(false, "Base Execute should not be called directly.");
        }

        //public static MessageType PeekType(ref BinaryReader reader)
        //{
        //    return (MessageType)reader.ReadInt32(); // First int should always be msgType
        //}


        public virtual void PrintMe()
        {
            Debug.WriteLine($"[DataMessage] Type: {msgType}, Player: {player}");
        }

    }
}

