using DataLayer;
using System;
using System.Collections.Generic;

namespace LogicLayer
{
    public interface ILogicBall : IObserver<IBall>,  IObservable<ILogicBall>
    {
        double X { get; set; }
        double Y { get; set; }
    }

    internal class LogicBall : ILogicBall
    {
        private readonly List<IObserver<ILogicBall>> observers = new List<IObserver<ILogicBall>>();
        private double x;
        private double y;

        public double X
        {
            get => x;
            set
            {
                if (value.Equals(x))
                {
                    return;
                }

                x = value;
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (value.Equals(y))
                {
                    return;
                }

                y = value;
            }
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(IBall value)
        {
            X = value.P.X;
            Y = value.P.Y;
            NotifyObservers();
        }

        public IDisposable Subscribe(IObserver<ILogicBall> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new SubscriptionToken(observers, observer);
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
        private readonly ICollection<IObserver<ILogicBall>> observers;
        private readonly IObserver<ILogicBall> observer;

        public SubscriptionToken(ICollection<IObserver<ILogicBall>> observers, IObserver<ILogicBall> observer)
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
