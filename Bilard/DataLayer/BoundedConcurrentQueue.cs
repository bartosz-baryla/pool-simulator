using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public abstract class IBoundedConcurrentQueue<T>
    {
        public abstract void Enqueue(T item);
        public abstract bool TryDequeue(out T result);

        public static IBoundedConcurrentQueue<T> CreateQueue()
        {
            return new BoundedConcurrentQueue<T>();
        }
    }

    internal class BoundedConcurrentQueue<T> : IBoundedConcurrentQueue<T>
    {
        private ConcurrentQueue<T> _queue;
        private object locker = new object();

        public BoundedConcurrentQueue()
        {
            _queue = new ConcurrentQueue<T>();
        }

        // Metoda dodająca element do kolejki
        public override void Enqueue(T item)
        {
            lock (locker)
            {
                if (_queue.Count >= 100000)
                {
                    OnBufferOverflow();
                }
                else
                {
                    _queue.Enqueue(item);
                }
            }
        }

        public override bool TryDequeue(out T result)
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
