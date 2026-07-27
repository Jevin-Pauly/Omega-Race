using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace LidgrenBasicUsage
{
    [Serializable]
    class SuperImportantData
    {
        public float thing1 = 0;
        public int thing2 = 0;
        public string thing3 = "";

        public void Serialize(ref BinaryWriter writer)
        {
            writer.Write(this.thing1);
            writer.Write(this.thing2);
            writer.Write(this.thing3);
        }
        public void Deserialize(ref BinaryReader reader)
        {
            this.thing1 = reader.ReadSingle();
            this.thing2 = reader.ReadInt32();
            this.thing3 = reader.ReadString();
        }

        public void execute()
        {
            Console.WriteLine("Executing work on data: " + thing1 + " & " + thing2 + " & [" + thing3 + "]");
            for (int i = 0; i < this.thing2; i++)
            {
                Console.WriteLine("\t[" + thing3 + "]");
            }
            Console.WriteLine("Execution done");
        }

    }
}
