namespace OmegaRace
{
    public class MessageQueueManager
    {
        private MessageQueueStrategy strategy;

        public MessageQueueManager(MessageQueueStrategy.Mode mode, string fileName = "recording.lol")
        {
            switch (mode)
            {
                case MessageQueueStrategy.Mode.Record:
                    strategy = new RecordStrategy(fileName);
                    break;

                case MessageQueueStrategy.Mode.Playback:
                    strategy = new PlaybackStrategy(fileName);
                    break;

                case MessageQueueStrategy.Mode.Normal:
                default:
                    strategy = new NormalStrategy();
                    break;
            }
        }

        public void AddToInputQueue(DataMessage msg)
        {
            strategy.AddToInputQueue(msg);
        }

        public void AddToOutputQueue(DataMessage msg)
        {
            strategy.AddToOutputQueue(msg);
        }

        public void Process()
        {
            strategy.ProcessOut();
            strategy.ProcessIn();
        }

        public void Close()
        {
            strategy.Close(); // Only does something in Playback/Recording
        }
    }
}
