using OmegaRace.Data_Queues;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OmegaRace
{
    public class PlaybackStrategy : MessageQueueStrategy
    {
        private Queue<DataMessage> inputQueue = new Queue<DataMessage>();
        private BinaryReader reader;

        public PlaybackStrategy(string filename)
        {
            reader = new BinaryReader(File.Open(filename, FileMode.Open));
        }

        public override void AddToInputQueue(DataMessage msg)
        {
            msg.Recycle();
        }

        public override void AddToOutputQueue(DataMessage msg)
        {
            msg.Recycle();
        }

        public override void ProcessOut()
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                long pos = reader.BaseStream.Position;
                float timestamp = reader.ReadSingle();

                if (timestamp > TimeManager.GetCurrentTime())
                {
                    // Rewind so we can re-read this message later
                    reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                    break;
                }

                pos = reader.BaseStream.Position;
                DataMessage.MessageType msg = DataMessage.Deserialize(ref reader).msgType;

                // Reset the stream position to read the data again
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);

                switch (msg)
                {
                    case DataMessage.MessageType.Movement:
                        MovementMessage.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.Fire:
                        FireMessage.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.Mine:
                        MineMessage.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.Collision:
                        CollisionMessage.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.PosRot:
                        PosRotMessage.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.ServerTimeRequest:
                        ServerTimeRequest.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.ServerTimeResponse:
                        ServerTimeResponse.Deserialize(ref reader).Execute();
                        break;
                    case DataMessage.MessageType.Uninitialized:
                        Debug.Assert(false, "Uninitialized message type");
                        break;
                }
            }
        }

        public override void ProcessIn()
        {
            //while (inputQueue.Count > 0)
            //{
            //    DataMessage msg = inputQueue.Dequeue();
            //    msg.Execute();
            //}
        }

        public override void Close()
        {
            reader?.Close();
        }
    }
}
