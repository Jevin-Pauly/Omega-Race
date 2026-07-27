using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

                AddToInputQueue(msg);
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
