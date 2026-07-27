using System.Collections.Generic;
using System.Diagnostics;

namespace OmegaRace
{
    public class MessageQueueManager
    {
        protected Queue<DataMessage> pInputQueue;
        protected Queue<DataMessage> pOutputQueue;

        public MessageQueueManager()
        {
            pInputQueue = new Queue<DataMessage>();
            pOutputQueue = new Queue<DataMessage>();
        }

        public void AddToInputQueue(DataMessage msg)
        {
            pInputQueue.Enqueue(msg);
        }

        public void AddToOutputQueue(DataMessage msg)
        {
            pOutputQueue.Enqueue(msg);
        }

        void ProcessOut()
        {
            while (pOutputQueue.Count > 0)
            {
                DataMessage msg = pOutputQueue.Dequeue();
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
                //AddToInputQueue(msg);
            }

            //ScreenLog.Add("Net msg count: " + msgcounter);
        }

        void ProcessIn()
        {
            while (pInputQueue.Count > 0)
            {
                DataMessage msg = pInputQueue.Dequeue();

                msg.Execute();
            }
        }

        public void Process()
        {
            ProcessOut();
            ProcessIn();
        }
    }
}
