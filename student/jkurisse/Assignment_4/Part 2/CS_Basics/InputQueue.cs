using System.Collections.Generic;

namespace CS_Basics
{
    public class InputQueue
    {
        private Queue<QueueItem> queue = new Queue<QueueItem>();

        public void add(QueueItem queueData)
        {
            queue.Enqueue(queueData);
        }

        public void process()
        {
            Calc calc = Program.GetCalc();
            FSM fsm = Program.GetFSM();
            while (queue.Count > 0)
            {
                var queueData = queue.Dequeue();
                queueData.doWork(calc, fsm);
            }
        }
    }
}
