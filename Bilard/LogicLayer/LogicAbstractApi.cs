using DataLayer;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System;
using System.Collections.Generic;
using Helpers;

namespace LogicLayer
{
    public abstract class LogicAbstractApi : IObserver<IBall>
    {
        public abstract int GetCount { get; }

        public abstract IList<ILogicBall> CreateBalls(int count);
        public abstract IBall GetBall(int id);
        public abstract ILogicBall GetLogicBall(int index);
        public abstract void BallPositionChanged(IBall ball);
        public abstract double DistanceBetweenBalls(double x1, double y1, double x2, double y2);
        public abstract void Start();
        public abstract void Stop();
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi();
        }

        public abstract void OnNext(IBall value);

        public abstract void OnError(Exception error);

        public abstract void OnCompleted();
    }

    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi dataLayer;
        private readonly Mutex mutex = new Mutex();
        private IList<ILogicBall> balls = new List<ILogicBall>();

        public LogicApi()
        {
            dataLayer = DataAbstractApi.CreateApi();

        }

        public override ILogicBall GetLogicBall(int index)
        {
            return balls[index];
        }

        public override IList<ILogicBall> CreateBalls(int number)
        {
            dataLayer.CreateBalls(number); 
            for (int i = 0; i < number; i++)
            {
                IBall ball = dataLayer.GetBall(i);
                LogicBall logicBall = new LogicBall();
                logicBall.P = ball.P;
                balls.Add(logicBall);
                ball.Subscribe(logicBall);
                ball.Subscribe(this);
            }
            return balls;
        }

        public override void OnCompleted(){}

        public override void OnError(Exception error){}

        public override void OnNext(IBall value)
        {
            BallPositionChanged(value);
        }

        public override void Start()
        {
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                dataLayer.GetBall(i).MakeThread(15);
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                dataLayer.GetBall(i).Stop();
            }
        }

        public override IBall GetBall(int index)
        {
            return dataLayer.GetBall(index);
        }

        public override int GetCount { get => dataLayer.BallsCount; }

        public override void BallPositionChanged(IBall ball)
        {
            mutex.WaitOne();        // Sekcja krytyczna - sprawdzanie kolizji (zamek)
            CheckCollisions(ball); 
            mutex.ReleaseMutex();
        }

        void CheckCollisions(IBall ball)
        {
            //Sprawdzam zderzenia ze ścianą:    
            int rightWall = 800 - 10 * 2;
            int leftWall = 0;
            int topWall = 0;
            int bottomWall = 400 - 10 * 2;


            // Sprawdzenie kolizji z górną i dolną krawędzią
            if (ball.P.Y <= topWall || ball.P.Y >= bottomWall)
            {
                // Zmiana kierunku prędkości na osi Y
                ball.V = new Position(ball.V.X, -ball.V.Y);
            }

            // Sprawdzenie kolizji z lewą i prawą krawędzią
            if (ball.P.X <= leftWall || ball.P.X >= rightWall)
            {
                // Zmiana kierunku prędkości na osi X
                ball.V = new Position(-ball.V.X, ball.V.Y);
            }

            // Sprawdzamy, czy nie dochodzi do kolizji między piłkami, zmieniamy pozycje kuli
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                IBall secondBall = dataLayer.GetBall(i);

                // Sprawdzam kolizję dwóch różnych piłek
                if (ball.ID != secondBall.ID && DistanceBetweenBalls(ball.P.X, ball.P.Y, secondBall.P.X, secondBall.P.Y) <= (2 * 12)) // bo 10 to promień
                {
                    double v1x = ball.V.X;
                    double v1y = ball.V.Y;
                    double v2x = secondBall.V.X;
                    double v2y = secondBall.V.Y;

                    //zakładamy że masa każdej kuli wynosi 1
                    int mass = 1;
                    double u1x = (v1x * (1 - mass) + 2 * mass * v2x) / (mass * 2);
                    double u1y = (v1y * (1 - mass) + 2 * mass * v2y) / (mass * 2);
                    double u2x = (v2x * (1 - mass) + 2 * mass * v1x) / (mass * 2);
                    double u2y = (v2y * (1 - mass) + 2 * mass * v1y) / (mass * 2);

                    ball.V = new Position(u1x, u1y);
                    secondBall.V = new Position(u2x, u2y);
                }
            }
        }


        public override double DistanceBetweenBalls(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

    }
}
