using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AzulNetworkBase
{
    [Serializable]
    class MoveMessage
    {
        public float xdelta;
        public float ydelta;

        public MoveMessage()
        {
            xdelta = 0;
            ydelta = 0;
        }

        public void Serialize(ref BinaryWriter writer)
        {
            writer.Write(this.xdelta);
            writer.Write(this.ydelta);
        }

        public void Deserialize(ref BinaryReader reader)
        {
            this.xdelta = reader.ReadSingle();
            this.ydelta = reader.ReadSingle();
        }
    }
}
