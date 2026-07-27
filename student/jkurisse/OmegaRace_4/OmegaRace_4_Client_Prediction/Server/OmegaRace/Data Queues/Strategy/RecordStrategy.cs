using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    public class RecordStrategy : MessageQueueStrategy
    {
        private Queue<DataMessage> inputQueue = new Queue<DataMessage>();
        private Queue<DataMessage> outputQueue = new Queue<DataMessage>();
        private BinaryWriter writer;

        public RecordStrategy(string filename)
        {
            writer = new BinaryWriter(File.Open(filename, FileMode.Create));
        }

        public override void AddToInputQueue(DataMessage msg)
        {
            inputQueue.Enqueue(msg);
        }

        public override void AddToOutputQueue(DataMessage msg)
        {
            outputQueue.Enqueue(msg);
        }

        public override void ProcessIn()
        {
            while (inputQueue.Count > 0)
            {
                DataMessage msg = inputQueue.Dequeue();
                msg.Execute();

                // Timestamp
                writer.Write(TimeManager.GetCurrentTime());
                msg.Serialize(ref writer);
            }
        }

        public override void ProcessOut()
        {
            while (outputQueue.Count > 0)
            {
                DataMessage msg = outputQueue.Dequeue();
                NetworkManager NetMgr = GameSceneCollection.ScenePlay.NetMgr;

                switch (msg.destination)
                {
                    case DataMessage.DestinationType.Network:
                        NetMgr.SendMessage(msg, msg.channel);
                        break;
                    case DataMessage.DestinationType.Local:
                        AddToInputQueue(msg);
                        break;
                    case DataMessage.DestinationType.Both:
                        NetMgr.SendMessage(msg, msg.channel);
                        AddToInputQueue(msg);
                        break;
                    default:
                        Debug.Assert(false, "Unknown destination type for message");
                        break;
                }
                //msg.Serialize(ref writer);
                //AddToInputQueue(msg);
            }
        }

        public override void Close()
        {
            writer?.Close();
        }
    }
}
