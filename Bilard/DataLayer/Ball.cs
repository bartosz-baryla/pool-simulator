
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DataLayer
{
    public interface IBall : IObservable<IBall>
    {
        void Stop();
        void MakeThread(int period, IBoundedConcurrentQueue<LoggerBall> queue);
        IVector P { get; }
        IVector V { get; set; }
        int ID { get; set; }
    }

    internal class Ball : IBall
    {
        private IVector position;
        private IVector velocity;
        private int id;
        private bool stop = false;
        private Thread thread;
        private readonly Mutex mutex = new Mutex();
        private readonly List<IObserver<IBall>> observers = new List<IObserver<IBall>>();

        public Ball(int id, float x, float y, double v_x, double v_y)
        {
            this.id = id;
            this.position = IVector.CreateVector(x, y);
            this.velocity = IVector.CreateVector(v_x, v_y);
            NotifyObservers();
        }

        public IVector P => position;
        public IVector V { get => velocity; set => velocity = value; }
        public int ID { get => id; set => id = value; }

        private void Move(double time, IBoundedConcurrentQueue<LoggerBall> queue)
        {
            mutex.WaitOne();
            double new_x = (position.X + V.X * time);
            double new_y = (position.Y + V.Y *  time);
            this.position = IVector.CreateVector(new_x, new_y);
            NotifyObservers();
            SaveDataInQueue(queue);
            mutex.ReleaseMutex();
        }
        public void SaveDataInQueue(IBoundedConcurrentQueue<LoggerBall> queue)
        {
            string hour = DateTime.Now.ToString("HH:mm:ss.fff");
            queue.Enqueue(new LoggerBall(this.ID, this.position.X, this.position.Y, this.velocity.X, this.velocity.Y, hour));
        }

        public IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new SubscriptionToken(observers, observer);
        }

        public void MakeThread(int period, IBoundedConcurrentQueue<LoggerBall> queue)
        {
            stop = false;
            thread = new Thread(() => Run(period, queue));
            thread.Start();
        }

        private void Run(int period, IBoundedConcurrentQueue<LoggerBall> queue)
        {
            double time = 0;
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
                observer.OnNext(this);
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
