using DataLayer;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace LogicLayer
{
    public abstract class LogicAbstractApi
    {
        public abstract event EventHandler Update;
        public abstract List<IBall> balls { get; }
        public abstract void CreateBalls(int ballCount);
        public abstract void Start();
        public abstract void Stop();
        public abstract void UpdateBalls();
        public abstract void SetInterval(int ms);
        public abstract int GetX(int i);
        public abstract int GetY(int i);
        public abstract int GetR(int i);
        public abstract int GetCount { get; }
        public abstract DispatcherTimer timer { get; }
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi();
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        public override DispatcherTimer timer { get; }
        public override List<IBall> balls { get; }
        private readonly DataAbstractApi dataLayer;

        public LogicApi()
        {
            dataLayer = DataAbstractApi.CreateApi();
            timer = new DispatcherTimer();
            balls = new List<IBall>();
            SetInterval(1);
            timer.Tick += (sender, args) => UpdateBalls();
        }

        /** Metoda tworzy wybraną liczbę kul i nadaje im losowe położenie na stole oraz kąt pod jakim będą się poruszać. **/
        public override void CreateBalls(int ballCount)
        {
            Random random = new Random();
            for (int i = 0; i < ballCount; i++)
            {
                int r = 20;
                int x = random.Next(0 + (2 * r), 800 - (2 * r));
                int y = random.Next(0 + (2 * r), 400 - (2 * r));
                double angle = random.NextDouble() * 90.0;
                IBall ball = new Ball(x, y, r, angle);
                balls.Add(ball);
            }
        }
        public override event EventHandler Update { add => timer.Tick += value; remove => timer.Tick -= value; }
        public TimeSpan Interval { get => timer.Interval; set => timer.Interval = value; }

        public event EventHandler Tick { add => timer.Tick += value; remove => timer.Tick -= value; }

        public override int GetX(int i)
        {
            return balls[i].X;
        }

        public override int GetY(int i)
        {
            return balls[i].Y;
        }

        public override void UpdateBalls()
        {
            foreach (IBall ball in balls)
            {
                ball.Move();
            }
        }

        public override int GetR(int i)
        {
            return balls[i].R;
        }

        public override int GetCount { get => balls.Count; }


        public override void Start()
        {
            timer.Start();
        }

        public override void Stop()
        {
            timer.Stop();
        }

        public override void SetInterval(int ms)
        {
            timer.Interval = TimeSpan.FromMilliseconds(ms);
        }
    }
}
