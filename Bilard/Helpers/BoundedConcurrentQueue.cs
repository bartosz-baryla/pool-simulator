using System.Collections.Concurrent;
using System;
using System.IO;

namespace Helpers
{
    public class BoundedConcurrentQueue<T>
    {
        private ConcurrentQueue<T> _queue;
        private int _capacity;
        private object locker = new object();

        public BoundedConcurrentQueue()
        {
            _queue = new ConcurrentQueue<T>();
            _capacity = 100000; 
        }

        // Metoda dodająca element do kolejki
        public void Enqueue(T item)
        {
            if (_queue.Count >= _capacity)
            {
                OnBufferOverflow();
            } 
            else
            {
                _queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            return _queue.TryDequeue(out result);
        }

        protected virtual void OnBufferOverflow()
        {
            string directory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(directory, "LoggsErrors.json");

            lock (locker)
            {
                string hour = DateTime.Now.ToString("HH:mm:ss.fff");
                File.AppendAllText(filePath, hour + " : Buffer overflow detected! \n");
            }
        }
    }
}
