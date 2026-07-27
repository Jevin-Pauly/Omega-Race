using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace OmegaRace
{

    [Serializable]
    public class DataMessage
    {

        public int horzInput;
        public int vertInput;

        public virtual void Serialize(ref BinaryWriter writer)
        {
            writer.Write(horzInput);
            writer.Write(vertInput);
        }

        public static DataMessage Deserialize(ref BinaryReader reader)
        {
            DataMessage output = new DataMessage();
            output.horzInput = reader.ReadInt32();
            output.vertInput = reader.ReadInt32();

            return output;
        }

        public void Execute()
        {
            // Locate player manager
            PlayerManager plMgr = GameSceneCollection.ScenePlay.PlayerMgr;

            // Hard-coded for P2 for demo
            plMgr.P2Data.ship.Move(vertInput);
            plMgr.P2Data.ship.Rotate(horzInput);
        }

    }
}

