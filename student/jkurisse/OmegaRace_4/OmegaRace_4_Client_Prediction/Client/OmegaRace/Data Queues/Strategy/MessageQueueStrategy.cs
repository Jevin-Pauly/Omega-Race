namespace OmegaRace
{
    public abstract class MessageQueueStrategy
    {
        public enum Mode
        {
            Normal,
            Record,
            Playback
        }

        public abstract void AddToInputQueue(DataMessage msg);
        public abstract void AddToOutputQueue(DataMessage msg);
        public abstract void ProcessIn();
        public abstract void ProcessOut();
        public virtual void Close() { } // Optional override
    }
}
