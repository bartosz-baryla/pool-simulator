using Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DataLayer
{
    public interface IBall : IObservable<IBall>
    {
        void Stop();
        void MakeThread(int period, ConcurrentQueue<LoggerBall> queue);
        Position P { get; }
        Position V { get; set; }
        int ID { get; set; }
    }

    internal class Ball : IBall
    {
        private Position position;
        private Position velocity;
        private int id;
        private bool stop = false;
        private Thread thread;
        private readonly Mutex mutex = new Mutex();
        private readonly List<IObserver<IBall>> observers = new List<IObserver<IBall>>();

        public Ball(int id, float x, float y, double v_x, double v_y)
        {
            this.id = id;
            this.position = new Position(x, y);
            this.velocity = new Position(v_x, v_y);
            NotifyObservers();
        }

        public Position P => position;
        public Position V { get => velocity; set => velocity = value; }
        public int ID { get => id; set => id = value; }

        private void Move(double time, ConcurrentQueue<LoggerBall> queue)
        {
            mutex.WaitOne();
            double new_x = (position.X + V.X * time);
            double new_y = (position.Y + V.Y *  time);
            this.position = new Position(new_x, new_y);
            NotifyObservers();
            SaveDataInQueue(queue);
            mutex.ReleaseMutex();
        }
        public void SaveDataInQueue(ConcurrentQueue<LoggerBall> queue)
        {
            string hour = DateTime.Now.ToString("HH:mm:ss.fff");
            queue.Enqueue(new LoggerBall(this.ID, this.position.X, this.position.Y, this.velocity.X, this.velocity.Y, hour));
            // po przepełnieniu bufora tracimy dane diagnostyczne trzeba poinformować.
        }

        public IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new SubscriptionToken(observers, observer);
        }

        public void MakeThread(int period, ConcurrentQueue<LoggerBall> queue)
        {
            stop = false;
            thread = new Thread(() => Run(period, queue));
            thread.Start();
        }

        private void Run(int period, ConcurrentQueue<LoggerBall> queue)
        {
            double time = 1; // Czas do pierwszego wywołania move na sztywno.
            while (!stop)
            {
                Stopwatch stopwatch = new Stopwatch();
                
                stopwatch.Start();

                if (!stop)
                {
                    Move(time, queue);// Przy następnych wywołaniach move bierzemy zmierzony czas poprzedniego.
                }
                Thread.Sleep(period);

                stopwatch.Stop();

                time = (double)stopwatch.Elapsed.TotalSeconds;
            }
        }

        public void Stop()
        {
            stop = true;
            thread.Join();
        }

        private void NotifyObservers()
        {
            foreach (var observer in observers.ToArray())
            {
                observer.OnNext(this); // przekazywanie pozycji a nie całej kulki.
            }
        }

    }

    public class SubscriptionToken : IDisposable
    {
        private readonly ICollection<IObserver<IBall>> observers;
        private readonly IObserver<IBall> observer;

        public SubscriptionToken(ICollection<IObserver<IBall>> observers, IObserver<IBall> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            if (observer != null && observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}
