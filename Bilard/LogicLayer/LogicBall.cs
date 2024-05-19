using DataLayer;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface ILogicBall : IObserver<IBall>,  IObservable<ILogicBall>
    {
        Position P { get; set; }
    }

    internal class LogicBall : ILogicBall
    {
        private readonly List<IObserver<ILogicBall>> observers = new List<IObserver<ILogicBall>>();
        private Position position;

        public Position P 
        { 
            get => position;
            set
            {
                if (value.Equals(position))
                {
                    return;
                }

                position = value;
                NotifyObservers();
            }

        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(IBall value)
        {
            P = value.P;
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
