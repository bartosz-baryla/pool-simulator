using DataLayer;
using System.Collections;
using System.ComponentModel;
using System.Threading;

namespace LogicLayer
{
    public abstract class LogicAbstractApi
    {
        public abstract int GetCount { get; }
        public abstract IList CreateBalls(int count);
        public abstract IBall GetBall(int id);
        public abstract void BallPositionChanged(object sender, PropertyChangedEventArgs args);
        public abstract void Start();
        public abstract void Stop();
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi();
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi dataLayer;
        private readonly Mutex mutex = new Mutex();

        public LogicApi()
        {
            dataLayer = DataAbstractApi.CreateApi();
        }

        public override IList CreateBalls(int number)
        {
            int previousCount = dataLayer.GetCount;
            IList temp = dataLayer.CreateBalls(number);
            for (int i = 0; i < dataLayer.GetCount - previousCount; i++)
            {
                dataLayer.GetBall(previousCount + i).PropertyChanged += BallPositionChanged;
            }
            return temp;
        }
        public override void Start()
        {
            for (int i = 0; i < dataLayer.GetCount; i++)
            {
                dataLayer.GetBall(i).MakeThread(15);

            }
        }

        public override void Stop()
        {
            for (int i = 0; i < dataLayer.GetCount; i++)
            {
                dataLayer.GetBall(i).Stop();

            }
        }

        public override IBall GetBall(int index)
        {
            return dataLayer.GetBall(index);
        }

        public override int GetCount { get => dataLayer.GetCount; }

        public override void BallPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            IBall ball = (IBall)sender;
            mutex.WaitOne();        // Sekcja krytyczna - sprawdzanie kolizji 
            //Sprawdzam przy każdym ruchu czy nie dochodzi do kolizji o ścianę lub inną piłkę
            //Collision(ball); 
            mutex.ReleaseMutex();
        }

    }
}
