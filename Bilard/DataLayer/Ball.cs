using Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DataLayer
{
    public interface IBall : IObservable<IBall>
    {
        void Stop();
        void MakeThread(int period);
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

        private void Move(double time)
        {
            mutex.WaitOne();
            double new_x = (position.X + V.X * time);
            double new_y = (position.Y + V.Y *  time);
            this.position = new Position(new_x, new_y);
            NotifyObservers(); 
            mutex.ReleaseMutex();
        }

        public IDisposable Subscribe(IObserver<IBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new SubscriptionToken(observers, observer);
        }

        public void MakeThread(int period)
        {
            stop = false;
            thread = new Thread(() => Run(period));
            thread.Start();
        }

        private void Run(int period)
        {
            while (!stop)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Thread.Sleep(period);

                stopwatch.Stop();
                double time = (double)stopwatch.Elapsed.TotalSeconds;

                if (!stop)
                {
                    Move(time);
                }
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
