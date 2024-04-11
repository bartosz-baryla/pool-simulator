using DataLayer;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace LogicLayer
{
    public abstract class LogicAbstractApi
    {
        public abstract List<Ball> balls { get; }
        public abstract void CreateBalls(int ballCount);
        public abstract void Start();
        public abstract void Stop();
        //public abstract void UpdateBalls();

        public abstract int GetX(int i);
        public abstract int GetY(int i);
        public abstract int GetR(int i);
        public abstract int GetCount { get; }
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi(/*TimerApi Timer = default(TimerApi)*/);
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        public override List<Ball> balls { get; }
        private readonly DataAbstractApi dataLayer;


        public LogicApi(/*, TimerApi WPFTimer*/)
        {
            dataLayer = DataAbstractApi.CreateApi();
            //timer = WPFTimer;
            balls = new List<Ball>();
            //SetInterval(1);
            //timer.Tick += (sender, args) => UpdateBalls();
        }

        /** Metoda tworzy wybraną liczbę kul i nadaje im losowe położenie na stole oraz kąt pod jakim będą się poruszać. **/
        public override void CreateBalls(int ballCount)
        {
            Random random = new Random();
            for (int i = 0; i < ballCount; i++)
            {
                int r = 20;
                int x = random.Next(0, 800-(2*r));
                int y = random.Next(0, 400- (2 * r));
                double angle = random.NextDouble() * 90.0;
                Ball ball = new Ball(x, y, r, angle);
                balls.Add(ball);
            }
        }
        //public override event EventHandler Update { add => timer.Tick += value; remove => timer.Tick -= value; }

        public override int GetX(int i)
        {
            return balls[i].X;
        }

        public override int GetY(int i)
        {
            return balls[i].Y;
        }


        public override int GetR(int i)
        {
            return balls[i].R;
        }

        public override int GetCount { get => balls.Count; }


        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }

}