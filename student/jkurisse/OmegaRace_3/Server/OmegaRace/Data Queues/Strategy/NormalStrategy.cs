using System.Collections.Generic;
using System.Diagnostics;

namespace OmegaRace
{
    public class NormalStrategy : MessageQueueStrategy
    {
        private Queue<DataMessage> inputQueue = new Queue<DataMessage>();
        private Queue<DataMessage> outputQueue = new Queue<DataMessage>();

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
                //AddToInputQueue(msg); // loopback behavior
            }
        }
    }
}
