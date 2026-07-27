using System.Collections.Concurrent;

namespace OmegaRace.Data_Queues
{
    public static class MessagePool<T> where T : OmegaRace.DataMessage, new()
    {
        private static readonly ConcurrentBag<T> pool = new ConcurrentBag<T>();

        public static T Get()
        {
            if (pool.TryTake(out T item))
                return item;

            return new T();
        }

        public static void Recycle(T item)
        {
            pool.Add(item);
        }
    }
}
